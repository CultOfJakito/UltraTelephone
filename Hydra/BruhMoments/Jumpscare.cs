using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UltraTelephone.Hydra
{
    public class Jumpscare : MonoBehaviour, IBruhMoment
    {
        private GameObject jumpscareUIPrefab;

        private Image image;
        private RectTransform canvas;
        private AudioSource audioSrc;
        private AudioClip ogClip;

        private Texture2D currentTexture;

        private void Awake()
        {
            SetInstance();

            if (HydraLoader.prefabRegistry.TryGetValue("JumpscareEngine", out jumpscareUIPrefab))
            {
                GameObject newJumpscareUI = GameObject.Instantiate<GameObject>(jumpscareUIPrefab, Vector3.zero, Quaternion.identity);
                newJumpscareUI.transform.parent = transform;
                canvas = newJumpscareUI.GetComponent<RectTransform>();
                audioSrc = canvas.GetComponent<AudioSource>();
                ogClip = audioSrc.clip;
                if (canvas.GetChild(0).TryGetComponent<Image>(out image))
                {
                    BruhMoments.RegisterBruhMoment(this);
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                Jumpscare.Scare(true);
            }
        }

        private float inTime = 0.05f, stayTime = 0.25f, outTime = 0.4f;
        private bool running = false;

        private IEnumerator FlashImage()
        {
            float timer = inTime;
            while (timer > 0.0f && running)
            {
                image.color = Color.Lerp(Color.white, new Color(1.0f, 1.0f, 1.0f, 0.0f), timer / inTime);
                yield return new WaitForEndOfFrame();
                timer -= Time.deltaTime;
            }
            image.color = Color.white;

            if (currentTexture.name == "sergsrhds" || currentTexture.name == "sergsrhds.PNG")
            {
                if (UltraTelephone.AudioSwapper.TryGetAudioClipByName("BadToThaBone", out AudioClip clip))
                {
                    audioSrc.clip = clip;
                }
                else
                {
                    audioSrc.clip = ogClip;
                }
            }
            else
            {
                if (UnityEngine.Random.value > 0.75f)
                {
                    AudioClip jumpscareClip = UltraTelephone.AudioSwapper.GetRandomAudioClipFromSubdirectory("jumpscare");
                    if (jumpscareClip != null)
                    {
                        audioSrc.clip = jumpscareClip;
                    }
                }
                else
                {
                    audioSrc.clip = ogClip;
                }
            }

            audioSrc.Play();

            timer = stayTime;
            while (timer > 0.0f && running)
            {
                yield return new WaitForEndOfFrame();
                timer -= Time.deltaTime;
            }

            timer = outTime;
            while (timer > 0.0f && running)
            {
                image.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 0.0f), Color.white, timer / outTime);
                yield return new WaitForEndOfFrame();
                timer -= Time.deltaTime;
            }
            image.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

            running = false;
        }

        private bool SetNewTexture()
        {
            Texture2D newTex = BestUtilityEverCreated.TextureLoader.PullRandomTexture();
            if (newTex != null)
            {
                currentTexture = newTex;
                return true;
            }
            return false;
        }

        private void SetNewTexture(Texture2D tex)
        {
            if (tex == null)
            {
                SetNewTexture();
            }
            else
            {
                currentTexture = tex;
            }
        }

        private Sprite TextureToSprite(Texture2D tex)
        {
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }

        private static Jumpscare Instance;

        private void SetInstance()
        {
            if (Jumpscare.Instance == null)
            {
                Jumpscare.Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public static void Scare(bool force = false)
        {
            if (Instance != null)
            {
                Instance.StaticExecute(force);
            }
        }

        private void StaticExecute(bool force = false)
        {
            if (force && running)
            {
                End();
            }

            Execute();
        }

        public void Execute()
        {
            if (!running && HydrasConfig.BruhMoments_Jumpscare)
            {
                running = true;
                if (SetNewTexture() && image != null)
                {
                    image.sprite = TextureToSprite(currentTexture);
                    StartCoroutine(FlashImage());
                }
                else
                {
                    running = false;
                }
            }
        }

        //this is garbage. too bad!
        public static void ScareWithTexture(Texture2D texture, bool force = false)
        {
            if (texture != null)
            {
                if (Instance != null)
                {
                    Instance.SetNewTexture(texture);
                    if (!Instance.running)
                    {
                        Instance.running = true;
                        if (Instance.image != null)
                        {
                            Instance.image.sprite = Instance.TextureToSprite(Instance.currentTexture);
                            Instance.StartCoroutine(Instance.FlashImage());
                        }
                        else
                        {
                            Instance.running = false;
                        }
                    }
                }
            }
        }

        public bool IsRunning()
        {
            return running;
        }

        public bool IsComplete()
        {
            return !running;
        }

        public void End()
        {
            running = false;
        }

        private void OnDestroy()
        {
            BruhMoments.RemoveBruhMoment(this);
        }

        public string GetName()
        {
            return "Jumpscare";
        }
    }
}

