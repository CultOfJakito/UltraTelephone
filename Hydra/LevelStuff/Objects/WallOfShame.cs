using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UltraTelephone.Hydra
{
    public class WallOfShame : ICustomPlacedObject
    {

        public Dictionary<string, TransformData> positions = new Dictionary<string, TransformData>()
        {
            { "uk_construct", new TransformData() { position = new Vector3(-164.99f,-8f,393f), rotation = new Vector3(0,90f,0),scale = Vector3.one} }
        };

        private static GameObject wallOfShamePrefab;

        public string[] GetScenePlacementNames()
        {
            return positions.Keys.ToArray();
        }

        public bool Place(string sceneName)
        {
            if (!HydrasConfig.WallOfShame_Enabled)
                return false;

            if (wallOfShamePrefab == null)
                wallOfShamePrefab = HydraLoader.LoadAsset<GameObject>("WallOfShame");

            if (wallOfShamePrefab == null)
                return false;


            GameObject newObject = GameObject.Instantiate(wallOfShamePrefab);
            newObject.transform.ApplyTrasnformData(positions[sceneName]);
            return true;
        }
    }
}
