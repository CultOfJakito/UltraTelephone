using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class CoinCollectorManager : MonoSingleton<CoinCollectorManager>
{
    private GameObject coinPrefab;
    private GameObject coinCollectFX;
    private GameObject coinUIPrefab;
    private AudioClip coinFanfare;

    private CollectableCoinUI coinUI;

    private List<CollectableCoin> coins = new List<CollectableCoin>();

    public static int CollectedCoins { get; private set; } = 0;

    private Transform coinParent;

    public int maxCoins = 420;

    public int positionResolveAttempts = 6;

    public float initalRange = 1000.0f, resolveRange = 45.0f;

    protected override void Awake()
    {
        base.Awake();
        HydraLoader.prefabRegistry.TryGetValue("CollectableCoin", out coinPrefab);
        HydraLoader.prefabRegistry.TryGetValue("CollectableCoinUI", out coinUIPrefab);
        HydraLoader.prefabRegistry.TryGetValue("CollectableCoinFX", out coinCollectFX);
        if(HydraLoader.dataRegistry.TryGetValue("CoinFanfare", out UnityEngine.Object cFObj))
        {
            coinFanfare = (AudioClip) cFObj;
        }
        
    }

    private void Reset(bool sceneLoad)
    {
        CollectedCoins = 0;

        if(!sceneLoad)
        {
            if(coinParent != null)
                GameObject.Destroy(coinParent.gameObject);
            if(coinUI != null)
                GameObject.Destroy(coinUI.gameObject);
        }

        coins = new List<CollectableCoin>();
        coinUI = null;
        coinParent = null;
    }

    public void ManualReset()
    {
        Reset(false);
        DeployPrefabs();
    }

    private void OnLevelChanged(BestUtilityEverCreated.UltrakillLevelType ltype)
    {
        Reset(true);
        DeployPrefabs();
    }

    private void DeployPrefabs()
    {  
        if(!BestUtilityEverCreated.InLevel())
        {
            return;
        }

        if(coinPrefab != null && coinUIPrefab != null)
        {
            DeployUI();
            DeployCoins();
        }
    }

    private void DeployUI()
    {
        GameObject newUI = GameObject.Instantiate<GameObject>(coinUIPrefab, Vector3.zero, Quaternion.identity);
        coinUI = newUI.GetComponent<CollectableCoinUI>();
        coinUI.Refresh();
    }

    private void DeployCoins()
    {
        if (!NavMesh.SamplePosition(Vector3.zero, out NavMeshHit navHit, Mathf.Infinity, 1))
        {
            SimpleLogger.Log("Nav mesh missing. No coins spawned.");
            return;
        }

        GameObject newCoinParent = new GameObject("CollectableCoins");
        coinParent = newCoinParent.transform;

        for (int i=0; i<maxCoins; i++)
        {
            if (TryGetCoinPlacementPosition(out Vector3 pos))
            {
                GameObject newCoinObj = GameObject.Instantiate<GameObject>(coinPrefab, pos, Quaternion.identity);
                CollectableCoin newCoin = newCoinObj.GetComponent<CollectableCoin>();
                newCoin.SetManager(this);
                newCoin.transform.parent = coinParent;
                coins.Add(newCoin);
            }
            else
            {
                i = Mathf.Clamp(i-1,0,maxCoins);
            }
        }

        SimpleLogger.Log($"{coins.Count} coins placed.");
    }

    private bool TryGetCoinPlacementPosition(out Vector3 pos)
    {
        Vector3 randPoint = UnityEngine.Random.insideUnitSphere * initalRange;
        pos = Vector3.zero;
        if (NavMesh.SamplePosition(randPoint, out NavMeshHit navHit, initalRange, 1)) //On Navmesh.
        {
            pos = navHit.position;

            for(int i=0; i<positionResolveAttempts; i++)
            {
                Vector3 offset = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(1.0f,resolveRange);
                if(NavMesh.SamplePosition(pos + offset, out NavMeshHit newNavHit, resolveRange, 1))
                {
                    pos = newNavHit.position;
                }
            }

            return true;
        }
        return false;
    }


    public void CoinCollected(CollectableCoin coin)
    {
        if(coin != null)
        {
            if(coins.Contains(coin))
            {
                Vector3 fxPos = coin.transform.position;
                GameObject.Instantiate(coinCollectFX, fxPos, Quaternion.identity);
                ++CollectedCoins;

                if(CollectedCoins % 100 == 0)
                {
                    if (coinFanfare != null)
                    {
                        AudioSource.PlayClipAtPoint(coinFanfare, fxPos);
                    }
                }
                

                if (coinUI != null)
                {
                    coinUI.Refresh();
                }
            }
        }        
    }

    public bool SpendCoins(int amountToSpend)
    {
        if (amountToSpend > CollectedCoins)
            return false;
        CollectedCoins -= amountToSpend;
        if (coinUI != null)
        {
            coinUI.Refresh();
        }
        return true;
    }



    private void OnEnable()
    {
        BestUtilityEverCreated.OnLevelChanged += OnLevelChanged;
    }

    private void OnDisable()
    {
        BestUtilityEverCreated.OnLevelChanged -= OnLevelChanged;
    }
}
