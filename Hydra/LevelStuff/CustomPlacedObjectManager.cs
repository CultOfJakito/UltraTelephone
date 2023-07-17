using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using UnityEngine;

namespace UltraTelephone.Hydra
{
    public static class CustomPlacedObjectManager
    {
        private static Dictionary<string, List<ICustomPlacedObject>> objectsForScenes = new Dictionary<string, List<ICustomPlacedObject>>();

        public static void Init()
        {
            RegisterAllCustomPlacementObjects();
            BestUtilityEverCreated.OnLevelChanged += (_) => OnLevelChanged();
        }


        private static void RegisterAllCustomPlacementObjects()
        {
            Type intf = typeof(ICustomPlacedObject);
            List<Type> customPlaceables = Assembly.GetExecutingAssembly().GetTypes().Where(p => intf.IsAssignableFrom(p) && !p.IsInterface).ToList();

            foreach (Type customPlaceableType in customPlaceables)
            {
                object newObject = Activator.CreateInstance(customPlaceableType);
                ICustomPlacedObject customPlaceable = newObject as ICustomPlacedObject;
                RegisterPlacable(customPlaceable);
            }
        }

        private static void OnLevelChanged()
        {
            string sceneName = SceneHelper.CurrentScene;

            if (!objectsForScenes.ContainsKey(sceneName))
                return;

            foreach (ICustomPlacedObject customPlacedObject in objectsForScenes[sceneName])
            {
                if (customPlacedObject.Place(sceneName))
                {
                    SimpleLogger.Log($"{customPlacedObject.GetType().Name}: Placed");
                }
                else
                {
                    SimpleLogger.Log($"{customPlacedObject.GetType().Name}: Failed to place");
                }
            }
        }

        public static void RegisterPlacable(ICustomPlacedObject customPlacedObject)
        {
            string[] scenes = customPlacedObject.GetScenePlacementNames();

            foreach (string scene in scenes)
            {
                if (!objectsForScenes.ContainsKey(scene))
                {
                    objectsForScenes.Add(scene, new List<ICustomPlacedObject>());
                }

                if (!objectsForScenes[scene].Contains(customPlacedObject))
                {
                    objectsForScenes[scene].Add(customPlacedObject);
                }
            }
        }

        public static void ApplyTrasnformData(this Transform transform, TransformData data)
        {
            transform.localPosition = data.position;
            transform.localRotation = Quaternion.Euler(data.rotation);
            transform.localScale = data.scale;
        }
    }

    public interface ICustomPlacedObject
    {
        public string[] GetScenePlacementNames();
        public bool Place(string sceneName);
    }

    public struct TransformData
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }


}
