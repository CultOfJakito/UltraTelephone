using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UltraTelephone.Hydra.Patches
{
    public class DebugPointerTHing : MonoBehaviour
    {

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Keypad2))
            {
                if(Physics.Raycast(transform.position,transform.forward, out RaycastHit hit, Mathf.Infinity, LayerMaskDefaults.Get(LMD.Environment)))
                {
                    Vector3 hitpos = hit.point;
                    Debug.LogWarning($"{hitpos.x}, {hitpos.y}, {hitpos.z}");
                }
            }
        }

    }
}
