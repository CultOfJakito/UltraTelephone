using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace UltraTelephone.Agent
{
    public static class AgentRegistry
    {
        //private static string SavePath = Directory.GetCurrentDirectory() + "/BepInEx/config/ultratelephone/foodstand.txt";
        private static string SavePath = TelephoneData.GetDataPath("data", "foodstand.txt");


        public static Dictionary<string, int> Costs = new Dictionary<string, int>()
        {
            { "Level 0-1", 10 },
            { "Level 0-2", 20 },
            { "Level 0-3", 30 },
            { "Level 0-4", 40 },
            { "Level 0-5", 50 },
            { "Level 1-1", 10 },
            { "Level 1-2", 20 },
            { "Level 1-3", 30 },
            { "Level 1-4", 40 },
            { "Level 2-1", 50 },
            { "Level 2-2", 60 },
            { "Level 2-3", 70 },
            { "Level 2-4", 80 },
            { "Level 3-1", 90 },
            { "Level 3-2", 100 },
            { "Level 4-1", 10 },
            { "Level 4-2", 20 },
            { "Level 4-3", 30 },
            { "Level 4-4", 40 },
            { "Level 5-1", 50 },
            { "Level 5-2", 60 },
            { "Level 5-3", 70 },
            { "Level 5-4", 80 },
            { "Level 6-1", 90 },
            { "Level 6-2", 100 }
        };

        public static Dictionary<string, Vector3> Positions = new Dictionary<string, Vector3>()
        {
            { "Level 0-1", new Vector3(16.5f, 8.2f, 589.2f) },
            { "Level 0-2", new Vector3(-38.8f, -11.2f, 243.8f) },
            { "Level 0-3", new Vector3(-10.6f, 46.8f, 366.8f) },
            { "Level 0-4", new Vector3(-16.8f, 21.8f, 502.9f) },
            { "Level 0-5", new Vector3(260.5f, -20.6f, 388.6f) },
            { "Level 1-1", new Vector3(-89.4f, -4.6f, 381.3f) },
            { "Level 1-2", new Vector3(-25.7f, -21.7f, 404.8f) },
            { "Level 1-3", new Vector3(-169.8f, 20f, 344.5f) },
            { "Level 1-4", new Vector3(65.7f, -13.2f, 483.8f) },
            { "Level 2-1", new Vector3(166.7f, 39.8f, 626.6f) },
            { "Level 2-2", new Vector3(-257f, 80.8f, 383.7f) },
            { "Level 2-3", new Vector3(-14.7f, 2.5f, 518.4f) },
            { "Level 2-4", new Vector3(60f, -19.9f, 343.3f) },
            { "Level 3-1", new Vector3(-206.1f, -119.2f, 347.8f) },
            { "Level 3-2", new Vector3(4.9f, 64.8f, 639.3f) },
            { "Level 4-1", new Vector3(-290.2f, 11.5f, 530f) },
            { "Level 4-2", new Vector3(138.7f, 41.2f, 1011.4f) },
            { "Level 4-3", new Vector3(112.3f, -26.7f, 616.3f) },
            { "Level 4-4", new Vector3(124f, 648.5f, 432.6f) },
            { "Level 5-1", new Vector3(151.8f, -150.2f, 235.4f) },
            { "Level 5-2", new Vector3(-369f, -13.9f, 677.9f) },
            { "Level 5-3", new Vector3(-101.1f, 282.8f, 279.2f) },
            { "Level 5-4", new Vector3(11.9f, 48.1f, 624.4f) },
            { "Level 6-1", new Vector3(168f, -205.2f, 270f) },
            { "Level 6-2", new Vector3(-25.5f, 29.8f, 303.7f) }
        };

        public static Dictionary<string, Vector3> Rotations = new Dictionary<string, Vector3>()
        {
            { "Level 0-1", new Vector3(0, 90, 0) },
            { "Level 0-2", new Vector3(0, 270, 0) },
            { "Level 0-3", new Vector3(0, 90, 0) },
            { "Level 0-4", new Vector3(0, 90, 0) },
            { "Level 0-5", new Vector3(0, 180, 0) },
            { "Level 1-1", new Vector3(0, 90, 0) },
            { "Level 1-2", new Vector3(0, 0, 0) },
            { "Level 1-3", new Vector3(0, 270, 0) },
            { "Level 1-4", new Vector3(0, 0, 0) },
            { "Level 2-1", new Vector3(0, 0, 0) },
            { "Level 2-2", new Vector3(0, 0, 0) },
            { "Level 2-3", new Vector3(0, 180, 0) },
            { "Level 2-4", new Vector3(0, 0, 0) },
            { "Level 3-1", new Vector3(0, 270, 0) },
            { "Level 3-2", new Vector3(0, 180, 0) },
            { "Level 4-1", new Vector3(0, 0, 0) },
            { "Level 4-2", new Vector3(15, 180, 0) },
            { "Level 4-3", new Vector3(0, 180, 0) },
            { "Level 4-4", new Vector3(0, 270, 0) },
            { "Level 5-1", new Vector3(0, 90, 0) },
            { "Level 5-2", new Vector3(0, 87, 0) },
            { "Level 5-3", new Vector3(0, 180, 0) },
            { "Level 5-4", new Vector3(4.5f, 243.5f, 0) },
            { "Level 6-1", new Vector3(0, 0, 0) },  
            { "Level 6-2", new Vector3(0, 0, 0) }
        };

        public static Dictionary<string, string> PrePurchaseDialogue = new Dictionary<string, string>()
        {
            { "Level 0-1", "Machine, I can't afford my security deposit. Please buy a soda." },
            { "Level 0-2", "Machine, I'm three months behind on rent. Please buy a hamburger." },
            { "Level 0-3", "Machine, taxes went up three-hundred percent. Please buy a áÔðÙjÐÚuË»àÜ®." },
            { "Level 0-4", "Machine, heaven is undergoing a housing crisis. Please buy some fish and chips." },
            { "Level 0-5", "Machine, I need to pay off my speeding tickets or they'll revoke my license. Please buy some blood pudding." },
            { "Level 1-1", "Machine, I overcharged my credit card. Please buy a pizza." },
            { "Level 1-2", "Machine, I've been convicted of arson and can't afford a lawyer. Please buy some chicken tenders." },
            { "Level 1-3", "Machine, I can't afford tuition. Please buy some cookies." },
            { "Level 1-4", "Machine, I need the start-up money for a farm. Please buy this hard-boiled egg." },
            { "Level 2-1", "Machine, the council is repossessing my car. Please buy some salmon rolls." },
            { "Level 2-2", "Machine, I've been fined for littering. Please buy some brownies." },
            { "Level 2-3", "Machine, I parked in a no-parking zone. Please buy a Leipäjuusto." },
            { "Level 2-4", "Machine, my electricity has been shut off. Please buy some water." },
            { "Level 3-1", "Machine, I need groceries. Please buy a pickle." },
            { "Level 3-2", "Machine, I'm in trouble with the Irish mafia. Please buy some lemonade." },
            { "Level 4-1", "Machine, my light has been repossessed. Please buy some ice cream." },
            { "Level 4-2", "Machine, I got fired from my job. Please buy a Räkmacka." },
            { "Level 4-3", "Machine, I've been evicted and need to find an apartment. Please buy a coconut bar." },
            { "Level 4-4", "Machine, my washing device broke and I need a replacement. Please buy a trifle." },
            { "Level 5-1", "Machine, Minos sued me and won. Please buy a sweet roll." },
            { "Level 5-2", "Machine, I've been denied insurance coverage. Please buy a Karjalanpiirakka." },
            { "Level 5-3", "Machine, I owe a friend some money. Please buy a hot dog." },
            { "Level 5-4", "Machine, I lost my wallet and phone. Please buy a pancake." },
            { "Level 6-1", "Machine, I can't afford my phone's data plan. Please buy a donut." },
            { "Level 6-2", "Machine, I have hours to live and things I want to do. Please buy some fries." },
        };

        public static Dictionary<string, string> PostPurchaseDialogue = new Dictionary<string, string>()
        {
            { "Level 0-1", "I won't forget this, Machine." },
            { "Level 0-2", "May your woes be few, and your days many." },
            { "Level 0-3", "Thank you, Machine." },
            { "Level 0-4", "Many thanks, Machine." },
            { "Level 0-5", "You have my gratitude, Machine. Now I can get to work on time." },
            { "Level 1-1", "I made a wish, and you delivered. Maybe you should try it sometime." },
            { "Level 1-2", "You have my thanks." },
            { "Level 1-3", "Will you come to my graduation, Machine?" },
            { "Level 1-4", "I look forward to serving you fresh produce, Machine." },
            { "Level 2-1", "Thank you for saving my car again, Machine." },
            { "Level 2-2", "I'll use the rubbish bins from now on." },
            { "Level 2-3", "I'm grateful, Machine." },
            { "Level 2-4", "Thank you, Machine. Now I can use my oven again." },
            { "Level 3-1", "I'll remember your kindness, Machine." },
            { "Level 3-2", "I'm sorry for what's about to happen, Machine. It's nothing personal." },
            { "Level 4-1", "I apologize for earlier." },
            { "Level 4-2", "My time is limited, Machine." },
            { "Level 4-3", "Thank you for helping me find my footing, Machine." },
            { "Level 4-4", "Be careful, Machine." },
            { "Level 5-1", "It's a good thing you don't rust, Machine." },
            { "Level 5-2", "Please show my friend the same kindness you showed me." },
            { "Level 5-3", "I'll pay you back for all you've done, Machine." },
            { "Level 5-4", "I was able to use FindMyiPhone thanks to you, Machine." },
            { "Level 6-1", "You smell good today, Machine." },
            { "Level 6-2", "Thank you, Machine. We will meet again." },
        };

        public static Dictionary<string, Sprite> Icons = new Dictionary<string, Sprite>()
        {
            { "Level 0-1", LoadToSprite(Properties.Resources.soda) },
            { "Level 0-2", LoadToSprite(Properties.Resources.hamburger) },
            { "Level 0-3", LoadToSprite(Properties.Resources.error) },
            { "Level 0-4", LoadToSprite(Properties.Resources.fish) },
            { "Level 0-5", LoadToSprite(Properties.Resources.pudding) },
            { "Level 1-1", LoadToSprite(Properties.Resources.pizza) },
            { "Level 1-2", LoadToSprite(Properties.Resources.chicken) },
            { "Level 1-3", LoadToSprite(Properties.Resources.cookies) },
            { "Level 1-4", LoadToSprite(Properties.Resources.egg) },
            { "Level 2-1", LoadToSprite(Properties.Resources.salmon) },
            { "Level 2-2", LoadToSprite(Properties.Resources.brownie) },
            { "Level 2-3", LoadToSprite(Properties.Resources.leip) },
            { "Level 2-4", LoadToSprite(Properties.Resources.water) },
            { "Level 3-1", LoadToSprite(Properties.Resources.pickle) },
            { "Level 3-2", LoadToSprite(Properties.Resources.lemonade) },
            { "Level 4-1", LoadToSprite(Properties.Resources.icecream) },
            { "Level 4-2", LoadToSprite(Properties.Resources.rakm) },
            { "Level 4-3", LoadToSprite(Properties.Resources.coconut) },
            { "Level 4-4", LoadToSprite(Properties.Resources.trifle) },
            { "Level 5-1", LoadToSprite(Properties.Resources.sweetroll) },
            { "Level 5-2", LoadToSprite(Properties.Resources.karj) },
            { "Level 5-3", LoadToSprite(Properties.Resources.hotdog) },
            { "Level 5-4", LoadToSprite(Properties.Resources.pancake) },
            { "Level 6-1", LoadToSprite(Properties.Resources.donut) },
            { "Level 6-2", LoadToSprite(Properties.Resources.fries) }
        };

        public static Dictionary<int, string> LevelDatabase = new Dictionary<int, string>()
        {
            {1, "Level 0-1" },
            {2, "Level 0-2" },
            {3, "Level 0-3" },
            {4, "Level 0-4" },
            {5, "Level 0-5" },
            {6, "Level 1-1" },
            {7, "Level 1-2" },
            {8, "Level 1-3" },
            {9, "Level 1-4" },
            {10, "Level 2-1" },
            {11, "Level 2-2" },
            {12, "Level 2-3" },
            {13, "Level 2-4" },
            {14, "Level 3-1" },
            {15, "Level 3-2" },
            {16, "Level 4-1" },
            {17, "Level 4-2" },
            {18, "Level 4-3" },
            {19, "Level 4-4" },
            {20, "Level 5-1" },
            {21, "Level 5-2" },
            {22, "Level 5-3" },
            {23, "Level 5-4" },
            {24, "Level 6-1" },
            {25, "Level 6-2" },
        };

        public static AudioClip eat_clip;

        private static Sprite LoadToSprite(byte[] bytes)
        {
            Texture2D tex = new Texture2D(256, 256);
            tex.LoadImage(bytes);
            return Sprite.Create(tex, new Rect(0, 0, 256, 256), new Vector2(0.5f, 0.5f));
        }

        public static void CompleteLevel(string name)
        {
            if (TelephoneData.Data.standsIncomplete.Contains(name))
            {
                TelephoneData.Data.standsIncomplete.Remove(name);
                TelephoneData.SaveData();
            }
        }

        public static IEnumerator GetAudio()
        {
            SimpleLogger.Log("INITIALIZING EAT AUDIO");

            if(AudioSwapper.TryGetAudioClipFromSubdirectory("eat", out AudioClip replacedEatClip))
            {
                eat_clip = replacedEatClip;
            }
            yield return null;
        }
    }

    
}
