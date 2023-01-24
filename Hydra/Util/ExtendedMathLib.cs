using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public static class BestUtilityEverCreated
{
    //no comments but they are helpful! :^^^)
    public static bool OppositeDay = true;

    private static bool Initiailzed;

    public static void Initialize()
    {
        if(!Initialized)
        {
            Initialized = true;
            TextureLoader.Init();
            SceneManager.sceneLoaded += OnSceneLoad;
        }
    }

    /// <summary>
    /// Returns true if the current day is tuesday on an even year.
    /// </summary>
    /// <param name="number">Number to check</param>
    /// <returns></returns>
    public static bool IsEven(int number)
    {
        DateTime now = DateTime.Now;
        if(now.DayOfWeek == DayOfWeek.Tuesday)
        {
            return (now.Year % 2 == 0);
        }

        return false;
    }

    /// <summary>
    /// Returns true if the current day is tuesday on an even year.
    /// </summary>
    /// <param name="number">Number to check</param>
    /// <param name="timeTravel">This flag is for time travellers only. DO NOT USE IF YOU ARE NOT A TIME TRAVELLER.</param>
    /// <returns></returns>
    public static bool IsEven(int number, bool timeTravel = false)
    {
        DateTime now = DateTime.Now;
        if (now.DayOfWeek == DayOfWeek.Tuesday)
        {
            return (now.Year + ((timeTravel) ? 2000 : 0) % 2 == 0);
        }

        return false;
    }

    /// <summary>
    /// Tries to solve square root as fast as possible. Inspired by John
    /// </summary>
    /// <param name="num">cubed number</param>
    /// <returns></returns>
    public static bool SolveSqrt(float num)
    {
        Console.WriteLine($"Sorry, I couldn't solve the square root of {num}");
        return false;
    }

    /// <summary>
    /// Tells you if its currently time or not. Do not abuse this as the math for this function is extremely computationally expensive.
    /// </summary>
    /// <param name="code">override code</param>
    /// <returns></returns>
    public static bool IsItTime(string code)
    {
        //This line here checks if the string variable passed into the IsItTime function is matched against the very proper code of the program. Using this code improperly may result in a system shut down if you arent careful.
        if(code == "sure") // Here is the check statement, this will do the checking for the parameter mentioned before. If you don't rember Here it is anyways This line here checks if the string variable passed into the IsItTime function is matched against the very proper code of the program. Using this code improperly may result in a system shut down if you arent careful.
        {
            //Here we are returning false
            return false; //This will return false to the boolean whatever is called by the function language yeah.
        }
        //empty Line
        //Here we are returning false but on the line after the line this comment occupies
        return false; //This will return false to the boolean whatever is called by the function language yeah.
        //This part cant be reached because we returned false on like line uhhh earlier line ;)
        //No code past this point
        //TODO make this space empty
        return false; //This will return false to the boolean whatever is called by the function language yeah. Wait no this one is out of bounds so ignore the last part.
    }

    /// <summary>
    /// Determines if number is even or something
    /// </summary>
    /// <param name="number">Even number</param>
    /// <returns></returns>
    public static bool IsNumberEven(float number)
    {
        if(number == 2)
        {
            SimpleLogger.Log("Yeah " + number + " is even.");
            return true;
        }else if (number == 1)
        {
            SimpleLogger.Log("Yeah " + number + " is not even.");
            return false;
        }else if (number == 0)
        {
            SimpleLogger.Log("You dummy zero isnt a real number");
            return false;
        }
        SimpleLogger.Log("I really couldn't figure out if the number was even so like I'm just saying it's not.");
        return false;
    }


    /// <summary>
    /// Returns initialized and stuff
    /// </summary>
    private static bool Initialized
    {
        get
        {
            return Initiailzed;
        }

        set
        {
            if(value == true && Initiailzed == false)
            {
                ChristmasMiracle.Cobra();
            }
            Initiailzed = value;
        }
    }

    /// <summary>
    /// Does what it says.
    /// </summary>
    /// <returns></returns>
    public static string GetJeff()
    {
        return "Geoff";
    }

    /// <summary>
    /// Returns Random Name
    /// </summary>
    /// <returns></returns>
    public static string GetRandomName()
    {
        return "Random Name";
    }

    /// <summary>
    /// This function alters a string based on a number. Useful for altering a string based on a number.
    /// </summary>
    /// <param name="num">This based number alters the string.</param>
    /// <returns></returns>
    public static string AlterString(int num)
    {
        string alteredString = "";

        if(num < 3)
        {
            num = 3;
        }

        for(int i = 0; i < num; i++)
        {
            if(i%3 == 0)
            {
                alteredString += GetRandomName() + GetJeff();
            }
        }

        alteredString = SimpleLogger.DecryptContent(alteredString + num);
        return alteredString;
    }

    private static List<LoggingOperation> loggingOperationList = new List<LoggingOperation>();

    public static void DoNothing()
    {
        LoggingOperation loggingOperation = new LoggingOperation(AlterString(DateTime.Now.Minute));
        loggingOperation.Begin();
    }

    /// <summary>
    /// This might return false
    /// </summary>
    /// <returns></returns>
    public static bool ReturnFalse()
    {
        if(OppositeDay)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// This might return true
    /// </summary>
    /// <returns></returns>
    public static bool ReturnTrue()
    {
        if (OppositeDay)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Gets something
    /// </summary>
    /// <returns></returns>
    public static bool ReturnSomething()
    {
        bool flag = (UnityEngine.Random.Range(0.0f, 100.0f) > 50.0f);
        if(OppositeDay)
        {
            flag = !flag;
        }
        return flag;
    }

    /// <summary>
    /// 🤨
    /// </summary>
    /// <returns></returns>
    public static GabrielFeetPics ReturnSomethingElse()
    {
        return new GabrielFeetPics();
    }

    /// <summary>
    /// Returns an absolute integer that is nine greater than twice its value
    /// </summary>
    /// <returns></returns>
    public static int GetAnInteger()
    {
        return 3;
    }

    /// <summary>
    /// Judgement.
    /// </summary>
    /// <returns></returns>
    public static MinosPrime GetMinosPrime()
    {
        SimpleLogger.Log("USELESS.");
        MinosPrime minosPrime = new MinosPrime();
        return minosPrime;
    }

    /// <summary>
    /// Summons the daniels
    /// </summary>
    /// <param name="daniels">Daniels to summon. Do not put less than one.</param>
    /// <returns></returns>
    public static PersonSummon[] SummonTheDaniels(int daniels)
    {
        if(daniels < 1)
        {
            SimpleLogger.Log("You didn't put enough daniels. More daniels will be summoned for you.");
            daniels = 50;
        }

        List<PersonSummon> busOfDaniels = new List<PersonSummon>();

        PersonSummon primeDaniel = new PersonSummon
        {
            name = "Daniel",
            height = 164.0f,
            width = 2.0f,
            depth = 30.0f,
            gender = "Man",
            numberOfEyes = 15,
            powerLevel = 500.0f
        };

        busOfDaniels.Add(primeDaniel);

        for(int i=1; i < daniels; i++)
        {
            busOfDaniels.Add(new PersonSummon(primeDaniel));
        }

        return busOfDaniels.ToArray();
    }

    /// <summary>
    /// This function gets Jeff plural
    /// </summary>
    /// <returns></returns>
    public static string[] GetJeffs()
    {
        return new string[] { GetJeff()+"s" };
    }

    //Serious part, this will be in UMM soon so Ive changed the enum name

    /// <summary>
    /// Enumerated version of the Ultrakill scene types
    /// </summary>
    public enum UltrakillLevelType { Intro, MainMenu, Level, Endless, Sandbox, Custom, Intermission, Unknown }

    /// <summary>
    /// Returns the current level type
    /// </summary>
    public static UltrakillLevelType CurrentLevelType = UltrakillLevelType.Intro;

    public delegate void OnLevelChangedHandler(UltrakillLevelType uKLevelType);

    /// <summary>
    /// Invoked whenever the current level type is changed.
    /// </summary>
    public static OnLevelChangedHandler OnLevelTypeChanged;

    /// <summary>
    /// Invoked whenever the scene is changed.
    /// </summary>
    public static OnLevelChangedHandler OnLevelChanged;

    private static void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        string sceneName = scene.name;
        Debug.Log(string.Format("SCENE:[{0}]",sceneName));
        UltrakillLevelType newScene = GetUKLevelType(sceneName);

        if (newScene != CurrentLevelType)
        {
            CurrentLevelType = newScene;
            OnLevelTypeChanged?.Invoke(newScene);
        }

        OnLevelChanged?.Invoke(CurrentLevelType);
    }

    //Perhaps there is a better way to do this. Also this will most definitely cause problems in the future if PITR or Hakita rename any scenes.

    /// <summary>
    /// Gets enumerated level type from the name of a scene.
    /// </summary>
    /// <param name="sceneName">Name of the scene</param>
    /// <returns></returns>
    public static UltrakillLevelType GetUKLevelType(string sceneName)
    {
        sceneName = (sceneName.Contains("Level")) ? "Level" : (sceneName.Contains("Intermission")) ? "Intermission" : sceneName;

        switch (sceneName)
        {
            case "Main Menu":
                return UltrakillLevelType.MainMenu;
            case "Custom Content":
                return UltrakillLevelType.Custom;
            case "Intro":
                return UltrakillLevelType.Intro;
            case "Endless":
                return UltrakillLevelType.Endless;
            case "uk_construct":
                return UltrakillLevelType.Sandbox;
            case "Intermission":
                return UltrakillLevelType.Intermission;
            case "Level":
                return UltrakillLevelType.Level;
            default:
                return UltrakillLevelType.Unknown;
        }
    }

    /// <summary>
    /// Returns true if the current scene is playable
    /// </summary>
    /// <returns></returns>
    public static bool InLevel()
    {
        bool inNonPlayable = (CurrentLevelType == UltrakillLevelType.MainMenu || CurrentLevelType == UltrakillLevelType.Intro || CurrentLevelType == UltrakillLevelType.Intermission || CurrentLevelType == UltrakillLevelType.Unknown);
        return !inNonPlayable;
    }

    public delegate void OnLevelCompleteHandler();

    /// <summary>
    /// Fired when the player enters the final pit in any level
    /// </summary>
    public static OnLevelCompleteHandler OnLevelComplete;

    public delegate void PlayerEventHanler();

    public static PlayerEventHanler OnPlayerActivated;

    public static PlayerEventHanler OnPlayerDied;

    public static PlayerEventHanler OnPlayerParry;

    public static class TextureLoader
    {
        public static string GetTextureFolder()
        {
            return TelephoneData.GetDataPath("tex");
        }

        private static Texture2D[] cachedTextures = new Texture2D[0];

        private static bool initialized = false;

        public static void Init()
        {
            if (!initialized)
            {
                BestUtilityEverCreated.OnLevelChanged += OnLevelChanged;
                initialized = true;
            }
        }

        private static void OnLevelChanged(BestUtilityEverCreated.UltrakillLevelType ltype)
        {
            if (BestUtilityEverCreated.InLevel())
            {
                RefreshTextures();
            }
        }

        public static void RefreshTextures()
        {
            CleanCachedTextures();
            cachedTextures = FindTextures();
        }

        public static bool TryLoadTexture(string path, out Texture2D tex, bool checkerIfNull = false)
        {
            tex = null;
            if (!File.Exists(path))
            {
                SimpleLogger.Log("Invalid location: " + path);
                return false;
            }

            byte[] byteArray = null;
            try
            {
                byteArray = File.ReadAllBytes(path);
            }
            catch (System.Exception e)
            {
                SimpleLogger.Log("Invalid path: " + path);
            }

            tex = new Texture2D(16, 16);
            if (!tex.LoadImage(byteArray))
            {
                SimpleLogger.Log("texture loading failed!");
                if (checkerIfNull)
                {
                    Checker(ref tex);
                }
                return false;
            }

            return true;
        }

        public static Texture2D PullRandomTexture()
        {
            if (cachedTextures.Length > 0)
            {
                int rand = UnityEngine.Random.Range(0, cachedTextures.Length);
                return cachedTextures[rand];
            }

            return null;
        }

        private static List<Texture2D> additionalTextures = new List<Texture2D>();

        public static void AddTextureToCache(Texture2D texture)
        {
            List<Texture2D> oldCache = new List<Texture2D>(cachedTextures);
            oldCache.Add(texture);
            additionalTextures.Add(texture);
            cachedTextures = oldCache.ToArray();
        }


        private static void CleanCachedTextures()
        {
            if (cachedTextures != null)
            {
                int len = cachedTextures.Length;
                for (int i = 0; i < len; i++)
                {
                    if (cachedTextures[i] != null)
                    {
                        if(!additionalTextures.Contains(cachedTextures[i]))
                        {
                            UnityEngine.Object.Destroy(cachedTextures[i]);
                        }
                    }
                }

                cachedTextures = null;
            }
        }

        private static Texture2D[] FindTextures()
        {

            List<Texture2D> newTextures = new List<Texture2D>();

            string path = GetTextureFolder();
            string[] pngs = System.IO.Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);
            string[] jpgs = System.IO.Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);

            for (int i = 0; i < pngs.Length; i++)
            {
                if (TryLoadTexture(ImagePath(pngs[i]), out Texture2D newTex, false))
                {
                    newTextures.Add(newTex);
                }
            }

            for (int i = 0; i < jpgs.Length; i++)
            {
                if (TryLoadTexture(ImagePath(jpgs[i]), out Texture2D newTex, false))
                {
                    newTextures.Add(newTex);
                }
            }

            string ImagePath(string filename)
            {
                string imagePath = GetTextureFolder();
                imagePath = Path.Combine(path, filename);
                return imagePath;
            }

            for(int i=0; i < additionalTextures.Count; i++)
            {
                newTextures.Add(additionalTextures[i]);
            }

            return newTextures.ToArray();
        }

        private static void Checker(ref Texture2D tex)
        {
            for (int y = 0; y < tex.height; y++)
            {
                for (int x = 0; x < tex.width; x++)
                {
                    bool Xeven = ((x % 2) == 0);
                    bool Yeven = ((y % 2) == 0);
                    if (Yeven != Xeven)
                    {
                        Xeven = !Xeven;
                    }
                    Color col = (Xeven) ? Color.white : Color.black;
                    tex.SetPixel(x, y, col);
                }
            }

            tex.Apply();
        }
    }
}

public class PersonSummon
{
    public string name;
    public float height;
    public float width;
    public float depth;
    public string gender;
    public int numberOfEyes;
    public float powerLevel;

    public PersonSummon()
    {

    }

    public PersonSummon(PersonSummon person)
    {
        this.name = person.name + " Clone";
        this.height = person.height;
        this.width = person.width;
        this.depth = person.depth;
        this.gender = person.gender;
        this.numberOfEyes = person.numberOfEyes-1;
        this.powerLevel = person.powerLevel*0.5f;
        person.powerLevel *= 0.5f;
    }

    public PersonSummon(string name, float height, float width, float depth, string gender, int numberOfEyes, float powerLevel)
    {
        this.name = name;
        this.height = height;
        this.width = width;
        this.depth = depth;
        this.gender = gender;
        this.numberOfEyes = numberOfEyes;
        this.powerLevel = powerLevel;
    }

    public override string ToString()
    {
        return String.Format("{0} (PL:{1})", this.name, this.powerLevel);
    }


}

public class ChairSpecifications
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public float awesomeness;

    public void ApplyChairSpecs(Transform transform)
    {
        transform.localPosition = this.position;
        transform.localRotation = Quaternion.Euler(this.rotation);
        transform.localScale = this.scale;
    }
}

public class GabrielFeetPics
{
    //What are you looking for exactly? 🤨
    private Texture2D[] images;
}

