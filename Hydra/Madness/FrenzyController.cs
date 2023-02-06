using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UltraTelephone.Hydra
{
    public class FrenzyController : MonoBehaviour
    {
        public static FrenzyController Instance { get; private set; }

        private GameObject madmassPrefab, massExplosion;
        private GameObject frenzyUIPrefab;
        private AudioClip frenzyStabSFX, frenzyStatusSFX;

        public MadMass MadnessMonster { get; private set; }
        public FrenzyMeter FrenzyUI { get; private set; }

        public float currentFrenzy = 0.0f;
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
                HydraLoader.prefabRegistry.TryGetValue("MadnessExplosion", out massExplosion);
                HydraLoader.prefabRegistry.TryGetValue("FrenzyUI", out frenzyUIPrefab);
                if (HydraLoader.dataRegistry.TryGetValue("FrenzyStabSFX", out UnityEngine.Object fssfx))
                {
                    frenzyStabSFX = (AudioClip)fssfx;
                }
                if (HydraLoader.dataRegistry.TryGetValue("FrenzyStatusSFX", out UnityEngine.Object fsfx))
                {
                    frenzyStatusSFX = (AudioClip)fsfx;
                }
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
            if (BestUtilityEverCreated.InLevel())
            {
                if (MadnessMonster != null)
                {
                    if (MadnessMonster.Alive)
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
            int damage = (int)(100.0f - (100.0f * ((6.0f - (currentDiff + 1.0f*HydrasConfig.Madness_FullDamage)) / 6.0f)));
            NewMovement.Instance.GetHurt(damage, false, 5.0f, false, false);
            FrenzyUI.ShowStatusInflicted();
            if (frenzyStatusSFX != null)
            {
                AudioSource.PlayClipAtPoint(frenzyStatusSFX, NewMovement.Instance.transform.position, 1.0f);
            }
        }

        private void FrenzyHit()
        {
            frenzyDamageTimer = frenzyDamageTime;
            currentFrenzy += Hydra.HydrasConfig.Madness_TickDelay;
            NewMovement.Instance.GetHurt(((int)(UltraTelephone.Hydra.HydrasConfig.Madness_TickDamage * (currentDiff + 1))), false, 0.0f, false, false);
            displayedFrenzy = currentFrenzy;
            if (frenzyStabSFX != null)
            {
                AudioSource.PlayClipAtPoint(frenzyStabSFX, NewMovement.Instance.transform.position, 1.0f);
            }
        }

        private void OnLevelChanged()
        {
            madMassKilled = false;

            if (BestUtilityEverCreated.InLevel())
            {
                if (MadnessMonster != null)
                {
                    if (MadnessMonster.Alive)
                    {
                        return;
                    }
                }

                if(HydrasConfig.Madness_Enabled)
                {
                    SpawnMadMass();
                }
            }
        }

        private void SpawnMadMass()
        {
            if (madMassKilled || madmassPrefab == null)
            {
                return;
            }

            currentFrenzy = 0.0f;
            displayedFrenzy = 0.0f;
            currentDiff = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty");

            string sceneName = SceneManager.GetActiveScene().name;

            if (TryGetMadMassSpawnPoint(sceneName, out MadMassSpawnPoint spawnPoint))
            {
                maxFrenzy = (spawnPoint.maxFrenzy / (((float)currentDiff + 1.0f) * 0.5f));
                maxFrenzy *= HydrasConfig.Madness_MaxMadnessMultiplier;
                SimpleLogger.Log("Max frenzy: " + maxFrenzy);
                GameObject newGO = GameObject.Instantiate<GameObject>(madmassPrefab, spawnPoint.position, Quaternion.identity);
                MadnessMonster = newGO.GetComponent<MadMass>();
                SimpleLogger.Log("WE spawned a mass! at " + spawnPoint.position);
                HudMessageReceiver.Instance.SendHudMessage("You feel a maddening presence.");

                if (frenzyUIPrefab != null)
                {
                    FrenzyUI = GameObject.Instantiate<GameObject>(frenzyUIPrefab, Vector3.zero, Quaternion.identity).GetComponent<FrenzyMeter>();
                }
            }

        }



        private bool TryGetMadMassSpawnPoint(string levelName, out MadMassSpawnPoint spawnPoint)
        {
            spawnPoint = null;

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i].levelName == levelName)
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
            GameObject.Instantiate<GameObject>(massExplosion, position, Quaternion.identity);
            NewMovement.Instance.LaunchFromPoint(position, 10.0f);
        }

        public void KillMadMass()
        {
            if (MadnessMonster != null)
            {
                if (MadnessMonster.Alive)
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
        new MadMassSpawnPoint(39.8f, 30.0f, 585.7f, "Level 0-1", 120.0f),
        new MadMassSpawnPoint(-59.6f, 0.8f, 254.2f, "Level 0-2"),
        new MadMassSpawnPoint(-19.6f, 85.0f,314.8f, "Level 0-3"),
        new MadMassSpawnPoint(-0.2f, -2.4f, 573.1f, "Level 0-4"),
        new MadMassSpawnPoint(87.9f, -6.1f, 145.3f, "Level 0-S", 15.0f),
        new MadMassSpawnPoint(259.7f, -16.0f, 381.9f, "Level 0-5", 90.0f),
        new MadMassSpawnPoint(81.0f, 12.0f, 168.8f, "Level 1-1", 160.0f),
        new MadMassSpawnPoint(-95.0f, 8.8f, 310f, "Level 1-2", 220.0f),
        new MadMassSpawnPoint(40.5f, -40.8f, 241.5f, "Level 1-3", 260.0f),
        new MadMassSpawnPoint(0.0f,18.0f,494.0f, "Level 1-4", 30.0f),
        new MadMassSpawnPoint(22.0f, -28.2f, 748.0f, "Level 2-1", 170.0f),
        new MadMassSpawnPoint(-124.0f,143.0f,546.0f, "Level 2-2", 140.0f),
        new MadMassSpawnPoint(-75.4f,8.5f,482.2f, "Level 2-3", 320.0f),
        new MadMassSpawnPoint(-96.5f, -127.7f, 452.2f, "Level 3-1"),
        new MadMassSpawnPoint(-4.8f, 110.0f, 345.0f, "Level 3-2"),
        new MadMassSpawnPoint(-290.5f, -35.5f, 452.8f, "Level 4-1"),
        new MadMassSpawnPoint(-503.4f, 348.9f, 66.2f, "Level 4-2"),
        new MadMassSpawnPoint(-65.8f, -5.3f, 675.8f, "Level 4-3"),
        new MadMassSpawnPoint(99.0f, 618.0f, 464.7f, "Level 4-4", 50.0f),
        new MadMassSpawnPoint(-28.8f, -84.6f, 447.5f, "Level 5-1"),
        new MadMassSpawnPoint(-2.7f, 728.6f, 57.1f, "Level 5-2"),
        new MadMassSpawnPoint(-13.5f, -3.2f, 317.5f, "Level 5-3"),
        new MadMassSpawnPoint(32.4f, 116.2f, 1149.7f, "Level 5-4"),
        new MadMassSpawnPoint(167.1f, 194.0f, -430.9f, "Level 6-1"),
        new MadMassSpawnPoint(113.6f, 0.8f, 350.0f, "Level 6-2"),
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
}

