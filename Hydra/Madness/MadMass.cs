using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MadMass : MonoBehaviour
{
    private AudioSource madeningLoop;

    public Vector2 pitchRange = new Vector2(0.75f,1.1f);
    public float pitchModulationSpeed = 0.25f;
    private bool pitchModulation = false;

    public bool Alive { get; private set; } = true;

    private void Awake()
    {
        madeningLoop = GetComponentInChildren<AudioSource>();
    }

    public void Die()
    {
        pitchModulation = false;
        Alive = false;
        madeningLoop.Stop();
        FrenzyController.Instance.MassKilled(transform.position);
    }

    private IEnumerator PitchModulator()
    {
        pitchModulation = true;
        

        while(pitchModulation)
        {
            if(madeningLoop != null)
            {
                float pitchTime = (Mathf.Sin(Time.time * pitchModulationSpeed)*0.5f)+0.5f;
                madeningLoop.pitch = Mathf.Lerp(pitchRange.x, pitchRange.y, pitchTime);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnEnable()
    {
        StartCoroutine(PitchModulator());
        Alive = true;
    }

    private void OnDisable()
    {
        pitchModulation = false;
    }
}
