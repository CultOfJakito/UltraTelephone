using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class Weirdener : MonoBehaviour
{
    private Renderer[] renderers;

    public float weirdenTime = 0.30f;
    private float weirdenTimer = 0.0f;

    private float meshRefreshTime = 30.0f;
    private float meshRefreshTimer = 0.0f;

    private Texture lastTexture;

    private void Update()
    {
        if (meshRefreshTimer < 0.0f && BestUtilityEverCreated.InLevel())
        {
            meshRefreshTimer = meshRefreshTime;
            RefreshMeshes();
        }

        if (renderers != null)
        {
            if (weirdenTimer < 0.0f && BestUtilityEverCreated.InLevel())
            {
                weirdenTimer = weirdenTime;
                //Weirden(GetRandomRenderer());
            }
        }

        if (Input.GetKeyDown(KeyCode.Equals))
        {

            RefreshMeshes();
            if (renderers != null)
            {  
                WeirdenAll();
            }
        }

        weirdenTimer -= Time.deltaTime;
        meshRefreshTimer -= Time.deltaTime;
    }

    private void Weirden(Renderer render)
    {
        if(render != null)
        {
            Vector3 currentScale = render.transform.localScale;
            Vector3 currentPos = render.transform.localPosition;
            Vector3 scalar = new Vector3
            {
                x = UnityEngine.Random.Range(0.8f, 1.2f),
                y = UnityEngine.Random.Range(1.01f, 1.2f),
                z = UnityEngine.Random.Range(0.8f, 1.2f)
            };

            Vector3 posScalar = UnityEngine.Random.insideUnitSphere * 0.25f;

            currentPos += posScalar;
            currentScale = Vector3.Scale(currentScale, scalar);
            render.transform.localScale = currentScale;
            //render.transform.localPosition = currentPos;

            Texture cache = render.material.mainTexture;

            if (lastTexture != null && render.gameObject.name != "Quad")
            {
                
                int matCount = render.GetMaterialCount();
                for(int i = 0; i < matCount; i++)
                {
                    render.materials[i].mainTexture = lastTexture;
                }  
            }

            lastTexture = cache;
        }
    }

    private void WeirdenAll()
    {

        for(int i = 0; i< renderers.Length; i++)
        {
            if (renderers[i] != null)
            {
                Weirden(renderers[i]);
            }
        }
        
    }

    private Renderer GetRandomRenderer()
    {
        Renderer rend = null;
        int attempts = 0;
        while(rend == null && attempts < 50)
        {
            int rand = UnityEngine.Random.Range(0, renderers.Length);
            if(renderers[rand] != null)
            {
                rend = renderers[rand];
            }
            ++attempts;
        }
        return rend;
    }

    private void RefreshMeshes()
    {
        if (!BestUtilityEverCreated.InLevel())
        {
            return;
        }

        renderers = FindObjectsOfType<MeshRenderer>();
    }

    private void OnLevelLoaded(BestUtilityEverCreated.UltrakillLevelType Ltype)
    {
        RefreshMeshes();
    }

    private void OnEnable()
    {
        BestUtilityEverCreated.OnLevelChanged += OnLevelLoaded;
    }

    private void OnDisable()
    {
        BestUtilityEverCreated.OnLevelChanged -= OnLevelLoaded;
    }
}

