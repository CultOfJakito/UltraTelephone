using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            SimpleLogger.Log("INITIALIZING FOOD STAND");
            if (_initialized) return;
            _bundle = AssetBundle.LoadFromMemory(Properties.Resources.gabrielStand);
            _foodStand = _bundle.LoadAsset<GameObject>("foodstand.prefab");
            _foodStand.transform.Find("icon").gameObject.AddComponent<AlwaysLookAtCamera>();
            _foodStand.AddComponent<FoodStand>().Trigger = _foodStand.transform.Find("trigger").gameObject;

            RenderObject(_foodStand, LayerMask.NameToLayer("Outdoors"));
            _foodStand.transform.Find("icon").gameObject.layer = LayerMask.NameToLayer("AlwaysOnTop");

            BestUtilityEverCreated.OnLevelChanged += PlaceFoodStand;
            _initialized = true;
            SimpleLogger.Log("INITIALIZED FOOD STAND");
        }

        public static void PlaceFoodStand(BestUtilityEverCreated.UltrakillLevelType level)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (level != BestUtilityEverCreated.UltrakillLevelType.Level) return;
            if (sceneName.Contains("-S") || sceneName.Contains("P-")) return;
            SimpleLogger.Log("PLACING FOOD STAND");
            SimpleLogger.Log("FOOD STAND OKAY");

            Transform foodStand = GameObject.Instantiate(_foodStand).transform;
            foodStand.parent = null;
            foodStand.position = AgentRegistry.Positions[sceneName];
            foodStand.rotation = Quaternion.Euler(AgentRegistry.Rotations[sceneName]);
            SimpleLogger.Log(foodStand);
        }

        public static void RenderObject(GameObject obj, LayerMask layer)
        {
            foreach (var c in obj.GetComponentsInChildren<Renderer>(true))
            {
                c.gameObject.layer = layer;
                c.material.shader = Shader.Find(c.material.shader.name);
            }
        }

        public static GameObject PrefabFind(this AssetBundle bundle, string bundlename, string name)
        {
            if (bundle == null)
            {
                if (File.Exists($@"{Application.productName}_Data\StreamingAssets\{bundlename}"))
                {
                    var data = File.ReadAllBytes($@"{Application.productName}_Data\StreamingAssets\{bundlename}");
                    bundle = LoadFromLoaded(bundle, bundlename) ?? AssetBundle.LoadFromMemory(data);
                }
                else
                {
                    Debug.LogWarning($"Could not find bundle {bundlename} or StreamingAssets file");
                    return new GameObject();
                }
            }
            return bundle.LoadAsset<GameObject>(name) ?? new GameObject();
        }
        public static AssetBundle LoadFromLoaded(this AssetBundle bundle, string name)
        {
            foreach (var bndl in AssetBundle.GetAllLoadedAssetBundles())
            {
                if (bndl.name == name)
                {
                    bundle = bndl;
                }
            }
            return bundle ?? null;
        }
    }

    public class FoodStand : MonoBehaviour
    {
        public GameObject Trigger;
        public SpriteRenderer Icon;
        public GameObject Poster;
        public GameObject Bible;
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

            /*
            source = gameObject.AddComponent<AudioSource>();
            source.volume = 3;
            source.clip = AgentRegistry.eat_clip;
            */
            Poster = Instantiate(FoodStandInitializer._bundle.LoadAsset<GameObject>("poster.prefab"), new Vector3(-14.84f, 2.86f, 258.1f), Quaternion.Euler(0, 90, 0), null);
            Poster.transform.localScale *= 3;
            FoodStandInitializer.RenderObject(Poster, LayerMask.NameToLayer("Outdoors"));
            Poster.SetActive(false);
            CheckPoster();
            Bible = FoodStandInitializer.PrefabFind(null, "common", "Book");
        }

        public void ButtonPressed()
        {
            if (Hydra.CoinCollectorManager.Instance.SpendCoins(Cost))
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
            Hydra.RandomSounds.PlayRandomSoundFromSubdirectory("eat");
            AgentRegistry.CompleteLevel(SceneManager.GetActiveScene().name);
            CheckComplete();
        }

        public void CheckComplete()
        {
            string[] act1 = TelephoneData.Data.standsIncomplete.Where(s => s.Contains("0-") || s.Contains("1-") || s.Contains("2-") || s.Contains("3-")).ToArray();

            if (act1.Length == 0)
            {
                if (!bibleBool)
                    SubtitleController.Instance.DisplaySubtitle("Machine, I want to give you something. It's very important to me.");
                GameObject bible = Instantiate(Bible, Icon.transform.position, Quaternion.identity, null);
                Readable read = bible.GetComponent<Readable>();
                read.SetPrivate("content", "<b>Text Scanned - Unique Passage:</b>\n\n<i>\"The angel Gabriel was sent from God\r\nto a town of Galilee called Nazareth,\r\nto a virgin betrothed to a man named Joseph,\r\nof the house of David,\r\nand the virgin’s name was Mary.\r\nAnd coming to her, he said,\r\n'Hail, full of grace! The Lord is with you.'\r\nBut she was greatly troubled at what was said\r\nand pondered what sort of greeting this might be.\r\nThen the angel said to her,\r\n'Do not be afraid, Mary,\r\nfor you have found favor with God...\"</i>\n\n<b>Remaining text: Irrelevant.</b>");
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
            string[] act1 = TelephoneData.Data.standsIncomplete.Where(s => s.Contains("0-") || s.Contains("1-") || s.Contains("2-") || s.Contains("3-")).ToArray();
            string[] act2 = TelephoneData.Data.standsIncomplete.Where(s => s.Contains("4-") || s.Contains("5-") || s.Contains("6-")).ToArray();

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

    public static class ReflectionExtensions
    {
        public static void SetPrivate(this object obj, string name, object value)
        {
            obj.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(obj, value);
        }

        public static T GetPrivate<T>(this object obj, string name)
        {
            return (T)obj.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj);
        }
    }
}
