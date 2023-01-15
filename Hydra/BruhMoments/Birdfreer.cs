﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using UnityEngine;

public class BirdFreer : MonoBehaviour, IBruhMoment
{
    private static bool initialized = false;
    public static BirdFreer Instance { get; private set; }
    private GameObject freeBird;
    private Transform player;

    private List<GameObject> activeBirds = new List<GameObject>();

    private int maxBirdAmount = 18, minBirdAmount = 1, currentBirdAmount;

    private float timeTillNextBird = 0.0f;

    private bool running = false;

    public static void FreeBird()
    {
        if(!initialized || Instance == null)
        {
            Init();
        }

        if(Instance != null)
        {
            Instance.ReleaseBird();
        }
    }


    public static void Init()
    {
        if(!initialized)
        {
            GameObject newBirdFreer = new GameObject("BirdFreer");
            newBirdFreer.AddComponent<BirdFreer>();
        }
    }

    private void Awake()
    {
        if((Instance == null || !initialized))
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            HydraLoader.prefabRegistry.TryGetValue("FreeBird", out freeBird);
            initialized = true;
            BestUtilityEverCreated.OnLevelChanged += _ => OnLevelChange();

            BruhMoments.RegisterBruhMoment(this);
        }
        else
        {
            Destroy(this);
        }

    }

    private void OnLevelChange()
    {
        if(BestUtilityEverCreated.InLevel())
        {
            player = CameraController.Instance.transform;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            Execute();
        }
    }

    public void End()
    {
        running = false;
        KillAll();
    }

    private void KillAll()
    {
        GameObject[] bords = activeBirds.ToArray();
        int birds = activeBirds.Count;
        for(int i = 0; i < birds; i++)
        {
            if(bords[i] != null)
            {
                activeBirds.Remove(bords[i]);
                Destroy(bords[i]);
            }
        }
    }

    public static void BirdDeath(FreedBird bird)
    {
        if(Instance == null)
        {
            return;
        }else
        {
            if (Instance.activeBirds.Contains(bird.gameObject))
            {
                Instance.activeBirds.Remove(bird.gameObject);
            }
        }
    }

    public void Execute()
    {
        if(!running)
        {
            running = true;
            StartCoroutine(ReleaseTheBirds());
        }
    }

    private IEnumerator ReleaseTheBirds()
    {
        running = true;
        currentBirdAmount = UnityEngine.Random.Range(minBirdAmount, maxBirdAmount);
        while(running && activeBirds.Count < currentBirdAmount)
        {
            if(timeTillNextBird < 0.0f)
            {
                timeTillNextBird = UnityEngine.Random.Range(3.0f, 17.0f);
                ReleaseBird();
            }
            yield return new WaitForEndOfFrame();
            timeTillNextBird -= Time.deltaTime;
        }
        running = false;
    }

    private void ReleaseBird()
    {
        if(freeBird != null && player != null)
        {
            Vector3 spawnPos = player.position;
            Vector3 randomOffset = UnityEngine.Random.insideUnitSphere;
            spawnPos += randomOffset * 2.1f;
            GameObject newbird = GameObject.Instantiate<GameObject>(freeBird, spawnPos, Quaternion.identity);
            activeBirds.Add(newbird);
        }
    }

    public string GetName()
    {
        return "Freeing the bird.";
    }

    public bool IsComplete()
    {
        return currentBirdAmount == activeBirds.Count;
    }

    public bool IsRunning()
    {
        return running;
    }

    private void OnDestroy()
    {
        BruhMoments.RemoveBruhMoment(this);
        BestUtilityEverCreated.OnLevelChanged -= _ => OnLevelChange();
    }
}

public class FreedBird : MonoBehaviour
{

    public float maxSpeed = 150.0f, minSpeed = 2.50f;
    private float speed = 0.0f;

    private Transform player;
    private Rigidbody rb;
    private AudioSource clipPlayer;

    private bool Birth()
    {
        player = CameraController.Instance.transform;
        clipPlayer = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        if (clipPlayer == null || player == null || rb == null)
        {
            return false;
        }

        float stereoPan = UnityEngine.Random.Range(-1.0f,1.0f);
        clipPlayer.panStereo = stereoPan;
        speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        return true;
    }

    private void Start()
    {
        if(!Birth())
        {
            Kill();
        }
    }

    private void FixedUpdate()
    {

        if(!clipPlayer.isPlaying || player == null)
        {
            Kill();
        }
        Vector3 newVelocity = rb.velocity;
        Vector3 newPosition = player.position;
        Vector3 currentPos = transform.position;
        Vector3 forceDirection = (newPosition - currentPos).normalized;

        forceDirection *= speed * Time.deltaTime;
        newVelocity += forceDirection;

        rb.velocity = newVelocity;
    }

    public void Kill()
    {
        BirdFreer.BirdDeath(this);
        Destroy(gameObject);
    }
}
