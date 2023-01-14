using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

namespace UltraTelephone.Agent
{
    public static class FoodStandInitializer
    {
        private static bool _initialized = false;

        public static AssetBundle _bundle;
        private static GameObject _foodStand;

        public static void Init()
        {
            Debug.Log("INITIALIZING FOOD STAND");
            if (_initialized) return;
            _bundle = AssetBundle.LoadFromMemory(Properties.Resources.gabrielStand);
            _foodStand = _bundle.LoadAsset<GameObject>("foodstand.prefab");
            _foodStand.transform.Find("icon").gameObject.AddComponent<AlwaysLookAtCamera>();
            _foodStand.AddComponent<FoodStand>().Trigger = _foodStand.transform.Find("trigger").gameObject;

            RenderObject(_foodStand, LayerMask.NameToLayer("Outdoors"));
            _foodStand.transform.Find("icon").gameObject.layer = LayerMask.NameToLayer("AlwaysOnTop");

            BestUtilityEverCreated.OnLevelChanged += PlaceFoodStand;
            _initialized = true;
            Debug.Log("INITIALIZED FOOD STAND");
        }

        public static void PlaceFoodStand(BestUtilityEverCreated.UltrakillLevelType level)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            Debug.Log("PLACING FOOD STAND");
            if (level != BestUtilityEverCreated.UltrakillLevelType.Level) return;
            if (sceneName.Contains("-S") || sceneName.Contains("P-")) return;
            Debug.Log("FOOD STAND OKAY");

            Transform foodStand = GameObject.Instantiate(_foodStand).transform;
            foodStand.parent = null;
            foodStand.position = AgentRegistry.Positions[sceneName];
            foodStand.rotation = Quaternion.Euler(AgentRegistry.Rotations[sceneName]);
            Debug.Log(foodStand);
        }

        public static void RenderObject(GameObject obj, LayerMask layer)
        {
            foreach (var c in obj.GetComponentsInChildren<Renderer>(true))
            {
                c.gameObject.layer = layer;
                c.material.shader = Shader.Find(c.material.shader.name);
            }
        }
    }

    public class FoodStand : MonoBehaviour
    {
        public GameObject Trigger;
        public SpriteRenderer Icon;
        public GameObject Poster;
        public int Cost;
        public string PrePurchaseSub;
        public string PostPurchaseSub;
        AudioSource source;
        private bool posterBool = false;
        private bool bibleBool = false;

        private void Awake()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            Trigger.AddComponent<FoodStandTrigger>().foodStand = this;
            Icon = transform.Find("icon").GetComponent<SpriteRenderer>();
            Icon.flipX = true;
            Cost = AgentRegistry.Costs[sceneName];
            PrePurchaseSub = AgentRegistry.PrePurchaseDialogue[sceneName];
            PostPurchaseSub = AgentRegistry.PostPurchaseDialogue[sceneName];
            Icon.sprite = AgentRegistry.Icons[sceneName];

            source = gameObject.AddComponent<AudioSource>();
            source.volume = 3;
            source.clip = AgentRegistry.eat_clip;

            Poster = Instantiate(FoodStandInitializer._bundle.LoadAsset<GameObject>("poster.prefab"), new Vector3(-14.84f, 2.86f, 258.1f), Quaternion.Euler(0, 90, 0), null);
            Poster.transform.localScale *= 3;
            FoodStandInitializer.RenderObject(Poster, LayerMask.NameToLayer("Outdoors"));
            Poster.SetActive(false);
            CheckPoster();
        }

        public void ButtonPressed()
        {
            if (CoinCollectorManager.Instance.SpendCoins(Cost))
            {
                Purchase();
                return;
            }
            HudMessageReceiver.Instance.SendHudMessage("YOU'RE TOO BROKE");
        }

        public void Purchase()
        {
            Trigger.SetActive(false);
            Icon.gameObject.SetActive(false);
            SubtitleController.Instance.DisplaySubtitle(PostPurchaseSub);
            source.Play();
            AgentRegistry.CompleteLevel(SceneManager.GetActiveScene().name);
            CheckComplete();
        }

        public void CheckComplete()
        {
            string[] act1 = AgentRegistry.incompleteLevels.Where(s => s.Contains("0-") || s.Contains("1-") || s.Contains("2-") || s.Contains("3-")).ToArray();

            if (act1.Length == 0)
            {
                if (!bibleBool)
                    SubtitleController.Instance.DisplaySubtitle("Machine, I want to give you something. It's very important to me.");
                // TODO: CREATE BIBLE
            }
            if (!posterBool && CheckPoster())
            {
                SubtitleController.Instance.DisplaySubtitle("Machine, I can never thank you enough.");
                SubtitleController.Instance.DisplaySubtitle("Please, take this as a token of my gratitude.");
                HudMessageReceiver.Instance.SendHudMessage("Acquired Poster!");
            }
        }

        public bool CheckPoster()
        {
            string[] act1 = AgentRegistry.incompleteLevels.Where(s => s.Contains("0-") || s.Contains("1-") || s.Contains("2-") || s.Contains("3-")).ToArray();
            string[] act2 = AgentRegistry.incompleteLevels.Where(s => s.Contains("4-") || s.Contains("5-") || s.Contains("6-")).ToArray();

            if (act1.Length == 0)
            {
                bibleBool = true;
            }
            if (act2.Length == 0)
            {
                Poster.SetActive(true);
                posterBool = true;
                return true;
            }
            return false;
        }
    }   

    public class FoodStandTrigger : MonoBehaviour
    {
        public FoodStand foodStand;
        HudMessage message;
        private bool active = false;
        private bool first_active = false;

        private void Awake()
        {
            message = gameObject.AddComponent<HudMessage>();
            message.playerPref = "";
            message.deactiveOnTriggerExit = true;
            message.notOneTime = true;
            message.dontActivateOnTriggerEnter = false;
            message.timed = true;
            message.timerTime = 3;

            message.message = "Press ";
            message.input = "Punch";
            message.message2 = " to purchase.";
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            active = true;
            foodStand.Icon.gameObject.SetActive(true);
            if (!first_active)
            {
                SubtitleController.Instance.DisplaySubtitle(foodStand.PrePurchaseSub);
                first_active = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag != "Player") return;
            active = false;
            foodStand.Icon.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!active) return;
            if (InputManager.Instance.InputSource.Punch.WasPerformedThisFrame)
            {
                foodStand.ButtonPressed();
            }
        }

        private void OnDisable()
        {
            message.timerTime = 0;
        }
    }
}
