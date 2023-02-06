using System;
using System.Collections.Generic;
using UnityEngine;

namespace UltraTelephone.Hydra
{
    public class CollectableCoin : MonoBehaviour
    {
        private CoinCollectorManager manager;

        public bool IsCollected { get; private set; } = false;

        public void SetManager(CoinCollectorManager manager)
        {
            if (this.manager == null)
            {
                this.manager = manager;
            }
        }

        private void Collected()
        {
            IsCollected = true;

            if (manager != null)
            {
                manager.CoinCollected(this);
            }

            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == NewMovement.Instance.gameObject)
            {
                Collected();
            }
        }
    }
}

