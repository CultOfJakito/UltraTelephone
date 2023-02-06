using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UltraTelephone.Hydra
{

    public class FrenzyMeter : MonoBehaviour
    {
        private Animator statusDisplay;
        private Image frenzyMeter;
        private GameObject frenzyContainer;

        private float lingerTime = 4.0f;
        private float lingerTimer = 0.0f;

        private bool flushing = false;

        private void Awake()
        {
            statusDisplay = GetComponent<Animator>();
            frenzyContainer = transform.Find("FrenzyContainer").gameObject;
            frenzyMeter = transform.Find("FrenzyContainer/FrenzyMeter/Meter/Container/Infill").GetComponent<Image>();
        }

        private void Update()
        {

            if (!flushing)
            {
                float frenzyAmount = FrenzyController.Instance.GetDisplayedFrenzy();

                if (frenzyAmount > 0.0f)
                {
                    frenzyContainer.SetActive(true);
                    frenzyMeter.fillAmount = frenzyAmount;
                }
                else
                {
                    if (lingerTimer <= 0.0f)
                    {
                        frenzyContainer.SetActive(false);
                    }
                    else
                    {
                        frenzyMeter.fillAmount = 0.0f;
                        lingerTimer -= Time.deltaTime;
                    }
                }
            }
        }

        private IEnumerator FlushFrenzy()
        {
            float frenzy = frenzyMeter.fillAmount;
            float flushTimer = 4.0f;
            while (flushTimer > 0.0f)
            {
                float unitInterval = Mathf.InverseLerp(4.0f, 0.0f, flushTimer);
                frenzyMeter.fillAmount = Mathf.Lerp(frenzy, 0.0f, unitInterval);
                yield return new WaitForEndOfFrame();
                flushTimer -= Time.deltaTime;
            }
            lingerTimer = 0.0f;
            flushing = false;
        }

        public void ShowStatusInflicted()
        {
            statusDisplay.Play("ShowStatus");
            lingerTimer = lingerTime;
        }

        public void Flush()
        {
            if (!flushing)
            {
                flushing = true;
                StartCoroutine(FlushFrenzy());
            }
        }
    }
}