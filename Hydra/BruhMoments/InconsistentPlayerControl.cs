using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using UnityEngine;

namespace UltraTelephone.Hydra
{
    public class InconsistentPlayerControl : MonoBehaviour, IBruhMoment
    {
        private NewMovement playerController;
        private CameraController cam;

        private void OnLevelChanged()
        {
            if(BestUtilityEverCreated.InLevel())
            {
                playerController = NewMovement.Instance;
                cam = CameraController.Instance;
            }
        }

        private void Awake()
        {
            BestUtilityEverCreated.OnLevelChanged += (_) => OnLevelChanged();
            BruhMoments.RegisterBruhMoment(this);
        }

        public void End()
        {
            //Nothing to end :p
        }

        public void Execute()
        {
            if(!BestUtilityEverCreated.InLevel())
            {
                return;
            }

            if(UnityEngine.Random.Range(0, 2) > 0)
            {
                CameraAnnoy();
            }else
            {
                MovementAnnoy();
            }
        }


        private void CameraAnnoy()
        {
            if(cam == null)
            {
                return;
            }

            DoRandomAction(
               () =>
               {
                   Vector3 lookDir = cam.transform.forward;
                   cam.transform.rotation = Quaternion.LookRotation(-lookDir, Vector3.up);
               },
               () =>
               {
                   cam.transform.rotation = UnityEngine.Random.rotation;
               },
               () =>
               {
                   cam.CameraShake(50.0f);
               }
               );      
        }

        private void MovementAnnoy()
        {
            if(playerController == null)
            {
                return;
            }

            DoRandomAction(
                () =>
                {
                    playerController.rb.velocity = -playerController.rb.velocity;
                },
                () =>
                {
                    playerController.Launch(UnityEngine.Random.insideUnitSphere * 100.0f);
                },
                () =>
                {
                    playerController.Jump();
                }
                );   
        }


        private void DoRandomAction(params Action[] actions)
        {
            if(!(actions.Length > 0))
            {
                return;
            }

            int index = UnityEngine.Random.Range(0, actions.Length);

            if(actions[index] != null)
            {
                actions[index].Invoke();
            }
        }

        public string GetName()
        {
            return "Inconsistent player control";
        }

        public bool IsComplete()
        {
            return true;
        }

        public bool IsRunning()
        {
            return false;
        }
    }
}
