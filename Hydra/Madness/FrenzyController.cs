using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrenzyController : MonoBehaviour
{
    public static FrenzyController Instance { get; private set; }

    private GameObject madmassPrefab, massExplosion;
    private GameObject frenzyUIPrefab;

    public MadMass MadnessMonster { get; private set; }
    public FrenzyMeter FrenzyUI { get; private set; }

    private float currentFrenzy = 0.0f;
    private float maxFrenzy = 180.0f;

    private float displayedFrenzy = 0.0f;

    private float frenzyDamageTime = 1.8f;
    private float frenzyDamageTimer = 0.0f;

    private bool madMassKilled = false;

    private int currentDiff = 0;

    private void Init()
    {
        if (FrenzyController.Instance == null)
        {
            FrenzyController.Instance = this;

            BestUtilityEverCreated.OnPlayerActivated += OnLevelChanged;
            BestUtilityEverCreated.OnLevelComplete += KillMadMass;

            HydraLoader.prefabRegistry.TryGetValue("MadMass", out madmassPrefab);
            HydraLoader.prefabRegistry.TryGetValue("FrenzyUI", out frenzyUIPrefab);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if(BestUtilityEverCreated.InLevel())
        {
            if(MadnessMonster != null)
            {
                if(MadnessMonster.Alive)
                {
                    frenzyDamageTimer -= Time.deltaTime;

                    if (currentFrenzy >= maxFrenzy)
                    {
                        FrenziedEffect();
                    }

                    if (frenzyDamageTimer < 0.0f)
                    {
                        FrenzyHit();
                    }
                }  
            }
        }
    }

    private void FrenziedEffect()
    {
        currentFrenzy = (maxFrenzy * 0.50f);
        int damage = (int)(100.0f - (100.0f * ((6.0f - (currentDiff + 1.0f))/6.0f)));
        NewMovement.Instance.GetHurt(damage, false, 5.0f, false, false);
        FrenzyUI.ShowStatusInflicted();
        //play frenzy sound
    }

    private void FrenzyHit()
    {
        frenzyDamageTimer = frenzyDamageTime;
        currentFrenzy += frenzyDamageTime;
        NewMovement.Instance.GetHurt(2 * (currentDiff + 1), false, 0.0f, false, false);
        displayedFrenzy = currentFrenzy;
        //Play a sound or throw a spear at player
    }

    private void OnLevelChanged()
    {
        madMassKilled = false;

        if (BestUtilityEverCreated.InLevel())
        {
            if(MadnessMonster != null)
            {
                if(MadnessMonster.Alive)
                {
                    return;
                }
            }

            SpawnMadMass();
        }
    }

    private void SpawnMadMass()
    {
        if(madMassKilled || madmassPrefab == null)
        {
            return;
        }

        currentFrenzy = 0.0f;
        displayedFrenzy = 0.0f;
        currentDiff = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty");

        string sceneName = SceneManager.GetActiveScene().name;

        if(TryGetMadMassSpawnPoint(sceneName, out MadMassSpawnPoint spawnPoint))
        {
            maxFrenzy = (spawnPoint.maxFrenzy/(((float)currentDiff+1.0f)*0.5f));
            SimpleLogger.Log("Max frenzy: " + maxFrenzy);
            GameObject newGO = GameObject.Instantiate<GameObject>(madmassPrefab, spawnPoint.position, Quaternion.identity);
            MadnessMonster = newGO.GetComponent<MadMass>();
            SimpleLogger.Log("WE spawned a mass! at " + spawnPoint.position);
            HudMessageReceiver.Instance.SendHudMessage("You feel a maddening presence.");

            if(frenzyUIPrefab != null)
            {
                FrenzyUI = GameObject.Instantiate<GameObject>(frenzyUIPrefab, Vector3.zero, Quaternion.identity).GetComponent<FrenzyMeter>();
            }
        }

    }

    

    private bool TryGetMadMassSpawnPoint(string levelName, out MadMassSpawnPoint spawnPoint)
    {
        spawnPoint = null;

        for(int i=0; i<spawnPoints.Length; i++)
        {
            if(spawnPoints[i].levelName == levelName)
            {
                spawnPoint = spawnPoints[i];
                return true;
            }
        }

        return false;
    }


    public void MassKilled(Vector3 position)
    {
        madMassKilled = true;
        FrenzyUI.Flush();
        currentFrenzy = 0.0f;
        displayedFrenzy = 0.0f;
        HudMessageReceiver.Instance.SendHudMessage("The maddening presence dissipates.");
        Destroy(MadnessMonster.gameObject);
        NewMovement.Instance.hp = 100;
        //Spawn explosion
    }

    public void KillMadMass()
    {
        if(MadnessMonster != null)
        {
            if(MadnessMonster.Alive)
            {
                MadnessMonster.Die();
            }
        }
    }

    public float GetFrenzyAmount()
    {
        return currentFrenzy / maxFrenzy;
    }

    public float GetDisplayedFrenzy()
    {
        return displayedFrenzy / maxFrenzy;
    }

    private MadMassSpawnPoint[] spawnPoints = new MadMassSpawnPoint[]
    {
        new MadMassSpawnPoint(39.8f,30.0f,585.7f, "Level 0-1", 120.0f),
        new MadMassSpawnPoint(-59.6f, 0.8f, 254.2f, "Level 0-2"),
        new MadMassSpawnPoint(-19.6f,85.0f,314.8f, "Level 0-3"),
        new MadMassSpawnPoint(-0.2f,-2.4f,573.1f, "Level 0-4"),
        new MadMassSpawnPoint(259.7f,-16.0f,381.9f, "Level 0-5"),
        new MadMassSpawnPoint(81.0f,12.0f,168.8f, "Level 1-1"),
        new MadMassSpawnPoint(-95.0f,8.8f,310f, "Level 1-2"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 1-3"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 1-4"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 2-1"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 2-2"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 2-3"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 2-4"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 3-1"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 3-2"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 4-1"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 4-2"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 4-3"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 4-4"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 5-1"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 5-2"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 5-3"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 5-4"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 6-1"),
        new MadMassSpawnPoint(0.0f,0.0f,0.0f, "Level 6-2"),
        new MadMassSpawnPoint(-104.6f, -7.2f, 341.0f, "uk_construct", 20.0f)

    };

    public class MadMassSpawnPoint
    {
        public Vector3 position;
        public string levelName;
        public float maxFrenzy;

        public MadMassSpawnPoint() { }
        public MadMassSpawnPoint(float x, float y, float z, string levelName, float maxFrenzy = 180.0f)
        {
            this.position = new Vector3(x, y, z);
            this.levelName = levelName;
            this.maxFrenzy = maxFrenzy;
        }
    }
}