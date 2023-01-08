using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LoggingOpObj : MonoBehaviour
{
    LoggingOperation operation;
    private bool initialized = false;

    private float tickTimer = 10.0f;
    private float currentTickTime = 0.0f;

    public void Initialize(LoggingOperation operation)
    {
        DontDestroyOnLoad(gameObject);
        this.operation = operation;
        initialized = true;
    }

    private void Update()
    {
        if(initialized)
        {
            if(currentTickTime < 0.0f)
            {
                currentTickTime = tickTimer;
                operation.DoTick();
            }
            currentTickTime -= Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        if (initialized)
        {
            if(operation.IsRunning && !operation.IsFinished())
            {
                operation.Stop();
            }
        }
    }
}

public class LoggingOperation
{
    private string name;

    public float TotalEarned { get; private set; }
    public int TotalLogsCut { get; private set; }
    public int TotalTreesCut { get; private set; }
    public int TotalLoggingPlansComplete { get; private set; }

    public bool IsRunning { get; private set; }

    private float maxLoggingTime;
    private float minLoggingTime;
    private float currentLoggingTime;

    private int minTrees = 1, maxTrees = 36, minTreeTypes, maxTreeTypes;

    private int planIndex = 0;

    private LoggingPlan[] loggingPlans;

    public LoggingOpObj component { get; private set; }



    public void DoTick()
    {
        if (IsRunning)
        {
            if(!IsFinished())
            {
                if(loggingPlans[planIndex] != null)
                {
                    while (loggingPlans[planIndex].IsComplete() && planIndex < loggingPlans.Length)
                    {
                        ++planIndex;
                    }
                    loggingPlans[planIndex].CutTrees();
                }
                ExecuteLoggingPlan(loggingPlans[planIndex]);
            }
            else
            {
                Stop();
            }
        }
    }

    public LoggingOperation(string name, float minLoggingTime = 15.0f, float maxLoggingTime = 60.0f, int minTreeTypes = 6, int maxTreeTypes = 15)
    {
        Array values = Enum.GetValues(typeof(WoodType));
        if (maxTreeTypes > values.Length || maxTreeTypes < 1)
        {
            maxTreeTypes = values.Length;
        }

        if (minTreeTypes < 1)
        {
            minTreeTypes = 1;
        }

        if (minTreeTypes > maxTreeTypes)
        {
            minTreeTypes = maxTreeTypes;
        }

        this.name = name;
        this.maxLoggingTime = maxLoggingTime;
        this.minLoggingTime = minLoggingTime;
        this.minTreeTypes = minTreeTypes;
        this.maxTreeTypes = maxTreeTypes;

        CreateGameObject();
    }

    private void CreateGameObject()
    {
        GameObject gameObject = new GameObject($"Logging Operation ({this.name})");
        component = gameObject.AddComponent<LoggingOpObj>();
        component.Initialize(this);
    }

    public bool IsFinished()
    {
        return (RemainingPlans() == 0);
    }

    public int RemainingPlans()
    {
        int finishedCount = 0;
        int remaining = 0;

        for (int i = 0; i < loggingPlans.Length; i++)
        {
            if(loggingPlans[i] != null)
            {
                if (loggingPlans[i].IsComplete())
                {
                    ++finishedCount;
                }
            }   
        }
        remaining = loggingPlans.Length - finishedCount;
        return remaining;
    }

    public int RemainingTrees()
    {
        int remaining = 0;
        for (int i = 0; i < loggingPlans.Length; i++)
        {
            if (!loggingPlans[i].IsComplete())
            {
                remaining += loggingPlans[i].RemainingTrees();
            }
        }
        return remaining;
    }

    public int RemainingLogs()
    {
        int remaining = 0;
        for (int i = 0; i < loggingPlans.Length; i++)
        {
            if (!loggingPlans[i].IsComplete())
            {
                remaining += loggingPlans[i].RemainingLogs();
            }
        }
        return remaining;
    }

    public void Begin()
    {
        LogStatus("Beginning logging operation!");
        int randomTreeTypes = UnityEngine.Random.Range(minTreeTypes, maxTreeTypes);

        loggingPlans = new LoggingPlan[randomTreeTypes];
        LogStatus("Starting plans");

        for (int i = 0; i < randomTreeTypes; i++)
        {
            loggingPlans[i] = new LoggingPlan(this, i, GetRandomWoodType(), UnityEngine.Random.Range(minTrees, maxTrees));
        }

        LogStatus("Plans done!");

        planIndex = 0;
        IsRunning = true;
        LogStatus(this.ToString());
    }

    public void Resume()
    {
        if(loggingPlans == null)
        {
            LogStatus("Cannot resume a logging operation which has not been started.");
            return;
        }

        planIndex = 0;
        IsRunning = true;
    }

    public void Stop()
    {
        IsRunning = false;
        LogStatus("Logging operation complete!");
        LogStatus(this.ToString());
    }

    private void ExecuteLoggingPlan(LoggingPlan plan)
    {
        if(plan != null)
        {
            if(plan.IsComplete())
            {
                ++planIndex;
            }
            plan.CutTrees();
        }
    }

    public void LoggingPlanFinished(LoggingPlan plan)
    {
        TotalEarned += plan.MoneyEarned;
        TotalLogsCut += plan.LogsCut;
        TotalTreesCut += plan.TreesCut();

        string planResult =
            "Logging Plan ({0}) Finished.\n" +
            "----------------------------\n" +
            "Total Earned:            {1}\n" +
            "Total Logs Cut:          {2}\n" +
            "----------------------------\n" +
            "Total {3} Trees Cut:  {4}";

        LogStatus(String.Format(planResult, GetDollarAmount(plan.MoneyEarned), plan.LogsCut, plan.woodType.ToString(), plan.TreesCut()));
    }

    public static WoodType GetRandomWoodType()
    {
        Array values = Enum.GetValues(typeof(WoodType));
        System.Random rand = new System.Random();
        WoodType randomWoodType = (WoodType)values.GetValue(rand.Next(values.Length));
        return randomWoodType;
    }

    public override string ToString()
    {
        string status =
            "Operation Name:           {0}\n" +
            "STATUS:                   {1}\n" +
            "-------------------------------\n" +
            "Total Earned:             {2}\n" +
            "Total Logs Cut:           {3}\n" +
            "Logging Plans Complete:   {4}\n" +
            "-------------------------------\n" +
            "Remaining Plans:          {5}\n" +
            "Remaining Trees:          {6}\n" +
            "Remaining Logs:           {7}";


        return String.Format(status, this.name, (IsFinished()) ? "FINISHED" : (IsRunning) ? "RUNNING" : "HALTED", GetDollarAmount(TotalEarned), this.TotalLogsCut, this.TotalLoggingPlansComplete, RemainingPlans(), RemainingTrees(), RemainingLogs());
    }

    private void LogStatus(string status)
    {
        Console.WriteLine($"Logging Operation ({this.name}): {status}");
    }

    public static string GetDollarAmount(float amount)
    {
        return string.Format("{0:C}", amount);
    }

    public class LoggingPlan
    {
        private LoggingOperation loggingOperation;
        private EarthTree[] trees;

        public WoodType woodType { get; private set; }
        public float MoneyEarned { get; private set; }
        public int LogsCut { get; private set; }
        public int ID { get; private set; }

        private int treeIndex = 0;
        private float moneyFromCurrentTree = 0.0f;
        private int logsFromCurrentTree = 0;

        private float cutTime = 2.0f;
        private float currentCutTime = 0.0f;

        public LoggingPlan(LoggingOperation loggingOperation, int id, WoodType woodType, int treeCount)
        {
            this.ID = id;
            this.loggingOperation = loggingOperation;
            this.woodType = woodType;
            trees = new EarthTree[treeCount];
            loggingOperation.LogStatus($"new logging starton : {this.ID}");
            for (int i = 0; i < treeCount; i++)
            {
                trees[i] = new EarthTree(this, woodType, UnityEngine.Random.Range(0.45f, 2.5f));
            }
            loggingOperation.LogStatus($"new logging plan made : {this.ID}");
        }

        public void CutTrees()
        {
            if(treeIndex < trees.Length)
            {
                if(treeIndex == 0 && logsFromCurrentTree == 0)
                {
                    loggingOperation.LogStatus($"Starting cutting on new tree {trees[treeIndex].woodType.ToString()}");                 
                }

                if (trees[treeIndex].LogsRemaining > 0)
                {
                    moneyFromCurrentTree += trees[treeIndex].CutLog();
                    ++logsFromCurrentTree;
                }else
                {
                    loggingOperation.LogStatus($"Earned {LoggingOperation.GetDollarAmount(moneyFromCurrentTree)} from {logsFromCurrentTree} logs of {trees[treeIndex].woodType.ToString()} wood.");

                    MoneyEarned += moneyFromCurrentTree;
                    LogsCut += logsFromCurrentTree;

                    moneyFromCurrentTree = 0.0f;
                    logsFromCurrentTree = 0;

                    ++treeIndex;
                }
            }
        }

        public bool IsComplete()
        {
            return (RemainingTrees() == 0);
        }

        public int RemainingTrees()
        {
            int finishedCount = 0;
            int remaining = 0;

            for (int i = 0; i < trees.Length; i++)
            {
                if (trees[i].LogsRemaining == 0)
                {
                    ++finishedCount;
                }
            }
            remaining = trees.Length - finishedCount;
            return remaining;
        }

        public int RemainingLogs()
        {
            int remaining = 0;

            for (int i = 0; i < trees.Length; i++)
            {
                if (trees[i].LogsRemaining > 0)
                {
                    remaining += trees[i].LogsRemaining;
                }
            }
            return remaining;
        }

        public int TreesCut()
        {
            return trees.Length - RemainingTrees();
        }
    }

    public class EarthTree
    {
        private LoggingPlan plan;
        private float value;
        private float logGradeMultiplier;

        public WoodType woodType { get; private set; }
        public int LogsRemaining { get; private set; }
        

        public EarthTree(LoggingPlan plan, WoodType woodType, float logGradeMultiplier)
        {
            this.plan = plan;
            this.woodType = woodType;
            LogsRemaining = UnityEngine.Random.Range(3, 7);
            value = UnityEngine.Random.Range(722.60f, 1205.35f);
            this.logGradeMultiplier = logGradeMultiplier;
        }

        public float CutLog()
        {
            if (LogsRemaining < 1)
            {
                return 0.0f;
            }

            float moneyGained = value / LogsRemaining;
            value -= moneyGained;

            --LogsRemaining;
            return moneyGained * logGradeMultiplier;
        }

    }
}

public enum WoodType { Araucaria, Cedar, CeleryTopPine, Cypress, DouglasFir, EuropeanYew, Fir, Hemlock, HuonPine, Kauri, QueenslandKauri, JapaneseNutmegYew, Larch, Pine, RedCedar, CoastRedwood, Rimu, Spruce, Abachi, Acacia, AfricanPadauk, Afzelia, Agba, Alder, Ash, Aspen, AustralianRedCedar, Ayan, Balsa, Basswood, Birch, Blackbean, Blackwood, Bloodwood, Boxelder, BrazilianWalnut, Brazilwood, Buckeye, Butternut, CaliforniaBayLaurel, CamphorTree, CapeChestnut, Catalpa, CeylonSatinwood, Cherry, Chestnut, Coachwood, Cocobolo, Corkwood, Cottonwood, Cucumbertree, Cumaru, Dogwood, Ebony, Elm, Eucalyptus, EuropeanCrabapple, EuropeanPear, GoncaloAlves, Greenheart, Genadilla, Guanandi, Gum, GumboLimbo, Hackberry, Hickory, Hornbeam, Ipe, Iroko, Ironwood, JacarandaBocaDeSapo, JacarandaDeBrasil, Jatoba, Kingwood, Lacewood, Limba, Locust, Mahogany, Maple, Marblewood, Marri, Meranti, Merbau, Mopane, Oak, Okokume, Olive, PinkIvory, Poplar, Purpleheart, QueenslandMaple, QueenslandWalnut, Ramin, Redheart, Sal, Sweetgum, Sandalwood, Sassafras, Satine, SilkyOak, SilverWattle, Sourwood, SpanishCedar, SpanishElm, Tamboti, Teak, ThailandRosewood, Tupelo, Turpentine, Walnut, Wenge, Willow, Zingana, Bamboo, Palm }

//Without this the game will crash

public static class CarWorld
{
    public static void Car()
    {
        SimpleLogger.Log("Car world.");
    }

    public static void World()
    {
        SimpleLogger.Log("world");
        ChristmasMiracle.Chrimstat(true);
    }

    public static void Carworld()
    {
        World();
    }

    public static void Poster()
    {
        ChristmasMiracle.Chrimstat(false);
        World();
    }
}

public static class CarWorld2
{
    public static void Car()
    {
        SimpleLogger.Log("Car world. 1");
    }

    public static void World()
    {
        SimpleLogger.Log("world 2");
        ChristmasMiracle.Chrimstat(true);
    }

    public static void Carworld()
    {
        World();
    }

    public static void Poster()
    {
        ChristmasMiracle.Chrimstat(false);
        World();
    }
}

public static class CarWorld3
{
    public static void Car()
    {
        SimpleLogger.Log("Car world. 3");
    }

    public static void World()
    {
        SimpleLogger.Log("world 3");
        ChristmasMiracle.Chrimstat(true);
    }

    public static void Carworld()
    {
        World();
    }

    public static void Poster()
    {
        ChristmasMiracle.Chrimstat(false);
        World();
    }
}

public static class CarWorld4
{
    public static void Car()
    {
        SimpleLogger.Log("Car world. 3");
    }

    public static void World()
    {
        SimpleLogger.Log("world 3");
        ChristmasMiracle.Chrimstat(true);
    }

    public static void Carworld()
    {
        World();
    }

    public static void Poster()
    {
        ChristmasMiracle.Chrimstat(false);
        World();
    }
}
