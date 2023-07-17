using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UltraTelephone.Hydra
{
    public class PlushiePlacer : ICustomPlacedObject
    {
        public Dictionary<string, TransformData> positions = new Dictionary<string, TransformData>()
        {
            { "CreditsMuseum2", new TransformData() { position = new Vector3(-261.0277f, 75.00449f, 708.2495f), scale = Vector3.one } },
            { "uk_construct", new TransformData() { position = new Vector3(-113.72f,-9f,589.62f),rotation = new Vector3(0,270f,0), scale = Vector3.one} }
        };

        public static GameObject Plushie
        {
            get
            {
                if (plushiePrefab == null)
                {
                    plushiePrefab = HydraLoader.LoadAsset<GameObject>("HydraDevPlushie");
                }
                return plushiePrefab;
            }
        }

        private static GameObject plushiePrefab;

        public string[] GetScenePlacementNames()
        {
            return positions.Keys.ToArray();
        }

        public bool Place(string sceneName)
        {
            if (Plushie == null)
                return false;

            if (!HydrasConfig.Plushie_Enabled)
                return false;

            GameObject newObject = GameObject.Instantiate(Plushie);
            newObject.transform.ApplyTrasnformData(positions[sceneName]);
            return true;
        }
    }

}
