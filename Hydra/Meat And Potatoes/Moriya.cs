using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Moriya
{
    private static Transform levelSectionRoot;

    private static float offset = 8.5f;
    private static int count = 26;

    private static List<GameObject> placedChairs = new List<GameObject>();

    private static bool initialized = false;

    public static void Init()
    {
        if(!initialized)
        {
            initialized = true;
            BestUtilityEverCreated.OnLevelChanged += _ => AddStuff();
        }
    }

    private static void AddStuff()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "Level 1-4" || !BestUtilityEverCreated.InLevel())
        {
            return;
        }

        Transform[] transforms = Resources.FindObjectsOfTypeAll<Transform>();
        for (int i = 0; i < transforms.Length; i++)
        {
            if(transforms[i].gameObject.scene.name == "Level 1-4")
            {
                if (transforms[i].gameObject.name == "1 - Opener")
                {
                    levelSectionRoot = transforms[i].gameObject.transform;
                    BeginPlacementSequence();
                    return;
                }
            }      
        }
    }

    private static void BeginPlacementSequence()
    {
        if (levelSectionRoot == null)
        {
            return;
        }

        Transform[] transforms = levelSectionRoot.GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i].name == SuperDaniel.DanielsOS)
            {
                if(IsThingWeLookinFor(transforms[i]))
                {
                    SimpleLogger.Log("Altered things!");
                    return;
                }
            }
        }
    }

    private static bool IsThingWeLookinFor(Transform transf)
    {
        if (transf.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
        {
            Transform p = transf.parent;
            if (p != null)
            {
                if (p.name == SuperDaniel.DanielsOS)
                {
                    AlterThings(p);
                    return true;
                }
            }
        }
        else
        {
            AlterThings(transf);
            return true;
        }

        SimpleLogger.Log("Found likely thing... However....");
        return false;
    }

    private static void AlterThings(Transform thing)
    {
        GameObject primarch = thing.gameObject;
        Vector3 pos = thing.position;
        Quaternion rot = thing.rotation;

        for(int i=0; i<count; i++)
        {
            GameObject.Instantiate<GameObject>(primarch, (pos + new Vector3(0.0f, 0.0f, ((i + 1) * offset))), rot);
        }

    }
}