using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SimpleLogger : MonoBehaviour
{
    //You thought it was gonna be a SimpleLogger but no. It was me, SIMPLELOGGER.

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoggingOperation primaryOperation = new LoggingOperation("Mockingbird", 15, 60,  7, 20);
        LoggingOperation secondaryOperation = new LoggingOperation("Silence", 10, 20, 1, 4);
        LogComplete();

        Log("Loaded the part made by Hydra. Time for pain!");

        primaryOperation.Begin();
        secondaryOperation.Begin();
    }

    private static bool initialized = false;

    /// <summary>
    /// Logs a message to console output.
    /// </summary>
    /// <param content="">String to log</param>
    public static void Log(string content)
    {
        Debug.Log(DecryptContent(content));
    }

    /// <summary>
    /// Logs a message to console output.
    /// </summary>
    /// <param obj="">String to log</param>
    public static void Log(object obj)
    {
        Debug.Log(DecryptContent(obj.ToString()));
    }

    /// <summary>
    /// Logs a message to console output.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="cars"></param>
    public void Log(string content, byte[] cars)
    {
        SimpleLogger.Log(content);
    }

    public static string DecryptContent(string content)
    {
        char[] decrypted = content.ToLower().ToCharArray();

        bool markup = false;

        for (int i = 0; i < decrypted.Length; i++)
        {
            switch (decrypted[i])
            {
                case 'r':
                    if (markup)
                        break;
                    decrypted[i] = 'w';
                    break;

                case 'l':
                    if (markup)
                        break;
                    decrypted[i] = 'w';
                    break;

                case 'y':
                    if (markup)
                        break;

                    if (decrypted.Length > i + 3)
                    {
                        if (decrypted[i + 1] == 'o' || decrypted[i + 2] == 'u')
                        {
                            decrypted[i] = 'u';
                            decrypted[i + 2] = 'u';
                            decrypted[i + 1] = 'w';
                        }
                    }
                    break;

                case '<':
                    markup = true;
                    break;

                case '>':
                    if(markup)
                    {
                        markup = false;
                    }
                    break;

            }
        }

        return new string(decrypted);
    }

    void RegisterAssets()
    {
        new HydraLoader.CustomAssetPrefab("MadMass", new Component[] { new MadMass() });
        new HydraLoader.CustomAssetPrefab("MadnessExplosion", new Component[] { new DestroyAfterTime() { timeLeft = 10.0f } });
        new HydraLoader.CustomAssetPrefab("FrenzyUI", new Component[] { new FrenzyMeter() });
        new HydraLoader.CustomAssetPrefab("VergilChair", new Component[] {});
        new HydraLoader.CustomAssetPrefab("CoinFart", new Component[] { new DestroyAfterTime() { timeLeft = 6.0f } });
        new HydraLoader.CustomAssetPrefab("JumpscareEngine", new Component[] {});
        new HydraLoader.CustomAssetPrefab("Lagometer", new Component[] {});
        new HydraLoader.CustomAssetPrefab("UbisoftIntegration", new Component[] { new UbisoftLink() });
        new HydraLoader.CustomAssetPrefab("CollectableCoin", new Component[] { new CollectableCoin() });
        new HydraLoader.CustomAssetPrefab("CollectableCoinUI", new Component[] { new CollectableCoinUI() });
        new HydraLoader.CustomAssetPrefab("CollectableCoinFX", new Component[] { new CollectableCoinUI(), new DestroyAfterTime() { timeLeft = 4.0f } });
        new HydraLoader.CustomAssetData("CoinFanfare", typeof(AudioClip));
        new HydraLoader.CustomAssetData("FrenzyStabSFX", typeof(AudioClip));
        new HydraLoader.CustomAssetData("FrenzyStatusSFX", typeof(AudioClip));
        new HydraLoader.CustomAssetData("FunnyParryNoise", typeof(AudioClip));
        new HydraLoader.CustomAssetPrefab("FreeBird", new Component[] { new FreedBird() });

        HydraLoader.RegisterAll(UltraTelephone.Properties.Resources.hydrabundle);
    }

    void LogComplete()
    {
        CarWorld.Car();
        CarWorld.Poster();
        CarWorld.Carworld();
        CarWorld.World();

        RegisterAssets();

        BruhMoments.Init();
        LazyBoy.Init();
        UbisoftIntegration.Init();
        Moriya.Init();
        CraigSpawner.Init();

        gameObject.AddComponent<ChristmasMiracle>();
        gameObject.AddComponent<FrenzyController>();
        gameObject.AddComponent<PlayerClowner>();
        gameObject.AddComponent<CoinCollectorManager>();
        
    }

}
