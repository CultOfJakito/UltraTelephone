using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace UltraTelephone.Hydra
{
    public class Herobrine : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform head;
        [SerializeField] private Transform centerMass;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private GameObject allVisuals;
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

        [SerializeField] private AudioSource audibleSource;
        [SerializeField] private AudioSource positionSource;

        public float lookSpeed = 40f;

        private Transform target;
        private Material material;

        private HerobrineState currentState;
        private HerobrineState stateBuffer;
        private HerobrineState defaultState => idle;

        private Idle idle = new Idle();
        private Lurk lurk = new Lurk();
        private Stalk stalk = new Stalk();
        private FloatAbove floatAbove = new FloatAbove();
        private FlyingFlee flyingFlee = new FlyingFlee();
        private FadeAway fadeAway = new FadeAway();
        private Teabag teabag = new Teabag();
        private FollowPlayer followPlayer = new FollowPlayer();
        private Exterminate exterminate = new Exterminate();
        private Flee flee = new Flee();

        public string CurrentStateDebugText;


        private void Start()
        {
            SwitchState(defaultState);
        }

        private void Update()
        {
            if (currentState != null)
                currentState.UpdateState(this);
        }

        private void LateUpdate()
        {
            if(currentState != null)
                currentState.UpdateAnimation(this);
        }

        public void SwitchState(HerobrineState newState)
        {
            if (currentState != null)
                currentState?.ExitState(this);

            if(newState == null)
                newState = defaultState;

            currentState = newState;
            CurrentStateDebugText = currentState.GetType().Name;
            currentState?.EnterState(this);
        }

        public void NextState()
        {
            if (stateBuffer != null)
            {
                HerobrineState newState = stateBuffer;
                stateBuffer = null;
                SwitchState(stateBuffer);
                return;
            }

            SwitchState(defaultState);
        }

        private void SetAlpha(float interval)
        {
            if (material == null)
            {
                material = skinnedMeshRenderer.material;
            }

            interval = Mathf.Clamp01(interval);

            if (material != null)
            {
                material.SetFloat("_OpacScale", interval);
            }
        }

        public bool TargetHasLineOfSight()
        {
            if (target == null)
                return false;

            Vector3 toHead = head.position - target.position;
            Vector3 targetLookDir = target.forward;

            if (Vector3.Dot(toHead.normalized, targetLookDir.normalized) <= 0f)
                return false;

            float expectedDist = toHead.magnitude;

            if (!Physics.Raycast(target.position, toHead, out RaycastHit hit, expectedDist, LayerMaskDefaults.Get(LMD.Environment)))
                return true;

            float errorMargin = 1f;

            return (expectedDist - errorMargin) > hit.distance;
        }

        public float GetTargetVisionDot()
        {
            if (target == null)
                return 0f;

            Vector3 toHerobrine = head.position - target.position;
            Vector3 lookDirection = target.forward;

            return Vector3.Dot(lookDirection, toHerobrine.normalized);
        }

        public float GetTargetDistance(bool XZ = false)
        {
            if (target == null)
                return 0f;

            Vector3 toTarget = target.position - head.position;

            if(XZ)
                toTarget.y = 0f;

            return toTarget.magnitude;
        }

        public void UpdateBodyRotation()
        {
            Quaternion headRot = head.rotation;
            Vector3 lookDir = headRot * Vector3.forward;
            lookDir.y = 0f;

            Quaternion bodyRotation = transform.rotation;
            Vector3 bodyDir = bodyRotation * Vector3.forward;
            bodyDir.y = 0f;

            float signedAngle = Vector3.SignedAngle(bodyDir.normalized, lookDir.normalized, Vector3.up);

            float YAngle = bodyRotation.eulerAngles.y;

            if(Mathf.Abs(signedAngle) > 60f)
            {
                float direction = Mathf.Sign(signedAngle);
                float targetDelta = 60f - Mathf.Abs(signedAngle);

                targetDelta *= direction;

                YAngle += targetDelta;
            }

            Quaternion newBodyRotation = Quaternion.Euler(0, YAngle, 0);
            transform.rotation = newBodyRotation;
            head.rotation = headRot;

        }

        public void SmoothRotateHead(Quaternion targetRotation, float speed = -1f)
        {
            speed = (speed < 0) ? lookSpeed: speed;

            Quaternion currentHeadRotation = head.rotation;

            Quaternion newHeadRotation = Quaternion.RotateTowards(currentHeadRotation, targetRotation, speed * Time.deltaTime);

            head.rotation = newHeadRotation;
            UpdateBodyRotation();
        }

        public void SmoothRotateHeadToFace(Vector3 point, float speed = -1f)
        {
            SmoothRotateHeadInDirection((point - head.position), speed);
        }

        public void SmoothRotateHeadInDirection(Vector3 direction, float speed = -1f)
        {
            Quaternion lookDir = Quaternion.LookRotation(direction.normalized);
            SmoothRotateHead(lookDir, speed);
        }

        public void LookAt(Vector3 pos)
        {
            LookInDirection((pos - head.position).normalized);
        }

        public void LookInDirection(Vector3 direction)
        {
            head.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            UpdateBodyRotation();
        }

        public bool TryFindSpotNearPlayer(out Vector3 point, float range, int attempts, bool xz = true)
        {
            point = Vector3.zero;

            if (target == null)
                return false;

            for (int i = 0; i < attempts; i++)
            {
                Vector3 targetPos = target.position;

                Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * range;

                Vector3 randomizedPostion = targetPos;
                randomizedPostion.x += randomOffset.x;
                randomizedPostion.z += randomOffset.z;

                if (!xz)
                    randomizedPostion.y += randomOffset.y;

                if (NavMesh.SamplePosition(randomizedPostion, out NavMeshHit hit, agent.height * 1.5f, NavMesh.AllAreas))
                {
                    point = hit.position;
                    return true;
                }
            }

            return false;
        }

        private void OnGUI()
        {
            //debug
            return;
            GUI.skin.box.fontSize = 35;
            GUI.skin.box.normal.textColor = Color.white;
            GUI.skin.box.alignment = TextAnchor.UpperLeft;
            GUILayout.Box(CurrentStateDebugText);
        }

        public class HerobrineState
        {
            public virtual void EnterState(Herobrine herobrine) { }
            public virtual void ExitState(Herobrine herobrine) { }
            public virtual void UpdateState(Herobrine herobrine) { }
            public virtual void UpdateAnimation(Herobrine herobrine) { }

            public virtual bool GetStateConditional(Herobrine herobrine) { return true; }
        }

        public class Idle : HerobrineState
        {
            float checkDelay = 2f;
            float timeLeft = 0;
            Herobrine herobrine;

            public override void EnterState(Herobrine herobrine)
            {
                this.herobrine = herobrine;
                herobrine.allVisuals.SetActive(false);
                herobrine.agent.enabled = false;
            }

            public override void UpdateState(Herobrine herobrine)
            {
                if(timeLeft > 0.0f)
                {
                    timeLeft -= Time.deltaTime;
                    return;
                }

                timeLeft = checkDelay;
                PerformCheck();
            }

            private void PerformCheck()
            {
                if (!BestUtilityEverCreated.InLevel())
                    return;

                if(herobrine.target != null)
                {
                    herobrine.SwitchState(herobrine.lurk);
                    return;
                }

                herobrine.target = CameraController.Instance.transform;
            }
        }

        public class Lurk : HerobrineState
        {
            Transform player;
            Herobrine herobrine;

            Vector2 lurkDelayRange;
            float timer;


            HerobrineState[] torments;

            public override void EnterState(Herobrine herobrine)
            {
                lurkDelayRange = new Vector2(5.0f, HydrasConfig.Herobrine_Frequency);
                torments = new HerobrineState[]
                {
                    herobrine.stalk,
                    herobrine.floatAbove,
                    herobrine.followPlayer,
                    herobrine.exterminate
                };

                timer = UnityEngine.Random.Range(lurkDelayRange.x, lurkDelayRange.y);
                this.herobrine = herobrine;
                this.player = herobrine.target;
                this.herobrine.agent.enabled = false;
                this.herobrine.allVisuals.SetActive(false);
                this.herobrine.SetAlpha(1f);
            }

            public override void UpdateState(Herobrine herobrine)
            {
                if(timer >0.0f)
                {
                    timer -= Time.deltaTime;
                    return;
                }

                herobrine.SwitchState(PickNextTorment());
            }

            private HerobrineState PickNextTorment()
            {
                List<HerobrineState> availableStates = torments.ToList();

                while(availableStates.Count > 0)
                {
                    HerobrineState randomState = availableStates[UnityEngine.Random.Range(0, availableStates.Count)];
                    
                    if(randomState.GetStateConditional(herobrine))
                    {
                        return randomState;
                    }

                    availableStates.Remove(randomState);
                }

                return this;
            }
        }

        public class FadeAway : HerobrineState
        {
            float fadeLength = 1.8f;
            float fadeTimer = 0f;

            public override void EnterState(Herobrine herobrine)
            {
                fadeTimer = fadeLength;
            }

            public override void UpdateAnimation(Herobrine herobrine)
            {
                fadeTimer = Mathf.Max(0, fadeTimer);
                herobrine.SetAlpha(fadeTimer / fadeLength);

                if(fadeTimer == 0)
                {
                    herobrine.NextState();
                }

                fadeTimer -= Time.deltaTime;
            }
        }

        public class Stalk : HerobrineState
        {
            float delay = 30f;
            float timer;

            public override void EnterState(Herobrine herobrine)
            {
                timer = delay;
                herobrine.allVisuals.SetActive(true);
                herobrine.agent.enabled = true;
                herobrine.SetAlpha(0f);

                if (herobrine.target == null)
                {
                    herobrine.NextState();
                    return;
                }
            }

            public override void UpdateState(Herobrine herobrine)
            {
                timer -= Time.deltaTime;

                bool onNavMesh = herobrine.agent.isOnNavMesh;
                herobrine.SetAlpha((onNavMesh) ? 1f : 0f);

                if (herobrine.target == null || timer < 0.0f)
                {
                    herobrine.NextState();
                    return;
                }

                if (!onNavMesh)
                {
                    if(herobrine.TryFindSpotNearPlayer(out Vector3 pos, 70f, 30))
                    {
                        herobrine.agent.Warp(pos);
                    }

                    return;
                }

                float lateralDist = herobrine.GetTargetDistance(true);
                bool lineOfSight = herobrine.TargetHasLineOfSight();
                Vector3 playerPos = herobrine.target.position;

                if (herobrine.GetTargetVisionDot() > 0.9f && lateralDist > 20f && lineOfSight)
                {
                    if(UnityEngine.Random.value > 0.90f)
                    {
                        herobrine.stateBuffer = herobrine.fadeAway;
                        herobrine.SwitchState(herobrine.teabag);
                        return;
                    }

                    
                }


                if (lateralDist < 8f && lineOfSight)
                {
                    if (UnityEngine.Random.value > 0.6f)
                    {
                        herobrine.SwitchState(herobrine.flee);
                        return;
                    }

                    //PlayLaugh
                    herobrine.stateBuffer = herobrine.lurk;
                    herobrine.SwitchState(herobrine.fadeAway);
                    return;

                }else if(lateralDist > 60f)
                {
                    herobrine.SwitchState(herobrine.followPlayer);
                }

            }

            public override void UpdateAnimation(Herobrine herobrine)
            {
                Vector3 playerPos = herobrine.target.position;
                herobrine.LookAt(playerPos);

                herobrine.animator.SetFloat("Floating", 0);
                herobrine.animator.SetFloat("Speed", 0);
                herobrine.animator.SetFloat("Crouching", 0);
            }
        }

        public class FloatAbove : HerobrineState
        {

            float verticalOffset = 13f;
            float horizontalOffset = 4f;

            float playSoundDelay = 3f;

            float playSoundTimer = 0f;

            bool playedSound = false;

            float currentAlpha;
            float alphaChangeDelta = 3f;

            float floatAroundSpeed = 25f;

            public override void EnterState(Herobrine herobrine)
            {
                playedSound = false;
                playSoundTimer = playSoundDelay;

                herobrine.agent.enabled = false;
                herobrine.allVisuals.SetActive(true);
                currentAlpha = 0f;

                Vector3 ppos = herobrine.target.position;
                Vector3 pLook = herobrine.target.forward;

                Vector3 relativeSpawnDirection = -pLook;
                relativeSpawnDirection.y = 0;

                Vector3 spawnPosition = ppos + (Vector3.up * verticalOffset);
                spawnPosition += relativeSpawnDirection.normalized * horizontalOffset;
                herobrine.transform.position = spawnPosition;
                herobrine.LookAt(ppos);
            }

            public override void UpdateState(Herobrine herobrine)
            {
                if(playSoundTimer > 0.0f)
                {
                    playSoundTimer -= Time.deltaTime;
                }else if(!playedSound)
                {
                    playedSound = true;
                    Debug.Log("HB Laugh");
                    //herobrine.MakeNoise(herobrine.transform.position);
                }

                currentAlpha = Mathf.MoveTowards(currentAlpha,1,alphaChangeDelta*Time.deltaTime);
                herobrine.SetAlpha(currentAlpha);

                if(herobrine.GetTargetVisionDot() > 0.6f)
                {
                    herobrine.SwitchState(herobrine.flyingFlee);
                    return;
                }

                Vector3 ppos = herobrine.target.position;
                Vector3 pLook = herobrine.target.forward;

                Vector3 relativeSpawnDirection = -pLook;
                relativeSpawnDirection.y = 0;

                Vector3 targetPos = ppos + (Vector3.up * verticalOffset);
                targetPos += relativeSpawnDirection.normalized * horizontalOffset;

                Vector3 currentPos = herobrine.transform.position;
                Vector3 newPosition = Vector3.Lerp(currentPos,targetPos,Time.deltaTime*floatAroundSpeed);

                Vector3 moveDelta = newPosition - currentPos;

                movingSpeed = moveDelta.magnitude;

                herobrine.transform.position += moveDelta;

                herobrine.SmoothRotateHeadToFace(ppos);

            }

            float movingSpeed = 0f;

            public override void UpdateAnimation(Herobrine herobrine)
            {
                herobrine.animator.SetFloat("Floating", 1);
                float speedCalc = Mathf.Clamp(movingSpeed,0f,15f)/15f;
                herobrine.animator.SetFloat("Speed", speedCalc);
                herobrine.animator.SetFloat("Crouching", 0);
            }

            public override bool GetStateConditional(Herobrine herobrine)
            {
                if (herobrine.target == null)
                    return false;

                Vector3 pLook = herobrine.target.forward;

                if (Vector3.Dot(pLook, Vector3.up) > 0.6f) //target not looking directly up
                    return false;

                Vector3 ppos = herobrine.target.position;


                Vector3 relativeSpawnDirection = -pLook;
                relativeSpawnDirection.y = 0;

                Vector3 spawnPosition = ppos + (Vector3.up * verticalOffset);
                spawnPosition += relativeSpawnDirection.normalized * horizontalOffset;

                Vector3 toSpawnPos = spawnPosition - ppos;


                if (Physics.Raycast(herobrine.target.position, toSpawnPos, toSpawnPos.magnitude+2f, LayerMaskDefaults.Get(LMD.Environment))) //we have space
                    return false;


                return true;
            }
        }

        public class FlyingFlee : HerobrineState
        {
            float alphaOverTimeDelay = 1.8f;
            float alphaTimer = 0f;

            float delayBeforeFadeOut = 2f;
            float fadeOutDelayTimer = 0f;

            float currentSpeed = 0f;
            float moveAcceleration = 2f;
            float moveAwayMaxSpeed = 20f;

            public override void EnterState(Herobrine herobrine)
            {
                herobrine.agent.enabled = false;
                herobrine.allVisuals.SetActive(true);
                currentSpeed = 0f;

                fadeOutDelayTimer = delayBeforeFadeOut;
                alphaTimer = alphaOverTimeDelay;
            }

            public override void UpdateState(Herobrine herobrine)
            {

                Vector3 playerPos = herobrine.target.position;
                Vector3 brinePos = herobrine.transform.position;

                Vector3 awayDir = brinePos- playerPos;
                awayDir.y = 0f;
                awayDir = (awayDir.normalized + Vector3.up * 2f).normalized;

                currentSpeed += Time.deltaTime * moveAcceleration;
                currentSpeed = Mathf.Min(currentSpeed, moveAwayMaxSpeed);

                Vector3 flightDelta = awayDir * currentSpeed;

                herobrine.transform.position += flightDelta;

                herobrine.SmoothRotateHeadInDirection(flightDelta);
            }

            public override void UpdateAnimation(Herobrine herobrine)
            {
                herobrine.animator.SetFloat("Floating", 1f);
                herobrine.animator.SetFloat("Speed", 1f);
                herobrine.animator.SetFloat("Crouching", 0);


                if (fadeOutDelayTimer > 0.0f)
                {
                    fadeOutDelayTimer -= Time.deltaTime;
                }
                else if (alphaTimer > 0.0f)
                {
                    alphaTimer -= Time.deltaTime;
                    alphaTimer = Mathf.Max(0, alphaTimer);
                }else
                {
                    herobrine.SwitchState(herobrine.lurk);
                }

                herobrine.SetAlpha(alphaTimer/alphaOverTimeDelay);
            }
        }

        public class Teabag : HerobrineState
        {
            bool down;
            float flipDelay = 0.7f;
            float flipTimer = 0f;

            float timerLength = 9f;
            float timer = 0f;

            public override void EnterState(Herobrine herobrine)
            {
                down = false;
                timer = timerLength;
            }

            public override void UpdateState(Herobrine herobrine)
            {
                if (herobrine.target == null || timer < 0.0f)
                {
                    herobrine.NextState();
                    return;
                }


                if (herobrine.GetTargetDistance() < 7f)
                {
                    herobrine.NextState();
                    return;
                }

                if (flipTimer < 0.0f)
                {
                    down = !down;
                    flipTimer = flipDelay;
                }else
                {
                    flipTimer -= Time.deltaTime;
                }

                timer -= Time.deltaTime;
            }


            public override void UpdateAnimation(Herobrine herobrine)
            {
                Vector3 ppos = herobrine.target.position;

                herobrine.SmoothRotateHeadToFace(ppos);

                herobrine.animator.SetFloat("Crouching", (down) ? 1 : 0);
            }
        }

        public class FollowPlayer : HerobrineState
        {
            float followDistance = 30f;

            float delay = 25f;
            float timer = 0f;

            public override void EnterState(Herobrine herobrine)
            {
                timer = delay;
                herobrine.agent.enabled = true;
                herobrine.allVisuals.SetActive(true);
                herobrine.SetAlpha(1f);
            }

            float movement;

            public override void UpdateState(Herobrine herobrine)
            {
                timer -= Time.deltaTime;

                bool onNavMesh = herobrine.agent.isOnNavMesh;
                herobrine.SetAlpha((onNavMesh) ? 1f : 0f);

                if (herobrine.target == null || timer < 0.0f)
                {
                    herobrine.NextState();
                    return;
                }

                if (!onNavMesh)
                {
                    if (herobrine.TryFindSpotNearPlayer(out Vector3 pos, 70f, 30))
                    {
                        herobrine.agent.Warp(pos);
                    }

                    return;
                }

                float lateralDist = herobrine.GetTargetDistance(true);
                bool lineOfSight = herobrine.TargetHasLineOfSight();
                Vector3 playerPos = herobrine.target.position;

                if (herobrine.GetTargetVisionDot() > 0.75f && lineOfSight)
                {
                    if (UnityEngine.Random.value > 0.60f)
                    {
                        herobrine.SwitchState(herobrine.fadeAway);
                        return;
                    }
                }


                if (lateralDist < followDistance && lineOfSight)
                {
                    //PlayLaugh
                    movement = Mathf.MoveTowards(movement, 0f, Time.deltaTime * 3f);
                    herobrine.agent.SetDestination(herobrine.transform.position);
                    herobrine.stateBuffer = herobrine.lurk;
                    herobrine.SwitchState(herobrine.fadeAway);
                    return;
                }
                else
                {
                    movement = Mathf.MoveTowards(movement, 1f, Time.deltaTime*3f);
                    herobrine.agent.SetDestination(playerPos);
                }
            }

            public override void UpdateAnimation(Herobrine herobrine)
            {
                herobrine.SetAlpha(1f);
                herobrine.animator.SetFloat("Floating", 0f);
                herobrine.animator.SetFloat("Crouching", 0f);
                herobrine.animator.SetFloat("Speed", movement);
            }
        }

        public class Exterminate : HerobrineState
        {
            float length = 1.5f;
            float timer;

            float startDistance = 10f;
            Vector3 headOffset;

            public override void EnterState(Herobrine herobrine)
            {
                herobrine.agent.enabled = false;
                herobrine.allVisuals.SetActive(true);
                herobrine.SetAlpha(0f);
                RandomSounds.PlayRandomSound();
                timer = length;
                headOffset = herobrine.transform.position - herobrine.head.position;
            }

            public override void UpdateState(Herobrine herobrine)
            {
                timer = Mathf.Max(0, timer);
                interval = 1-(timer / length);

                if (timer == 0)
                {
                    SimpleLogger.Log("Killed by herobrine.");
                    Application.Quit();
                }

                timer -= Time.deltaTime;
            }

            float interval = 0f;

            public override void UpdateAnimation(Herobrine herobrine)
            {
                herobrine.SetAlpha(interval);

                Vector3 playerPos = herobrine.target.position;
                Vector3 lookDir = herobrine.target.forward;

                lookDir.y = 0f;

                Vector3 targetPosition = playerPos + lookDir.normalized * (startDistance*(1-interval));
                targetPosition += headOffset;

                herobrine.transform.position = targetPosition;
                herobrine.transform.rotation = Quaternion.LookRotation(-lookDir, Vector3.up);
                herobrine.LookInDirection(-lookDir);

                herobrine.animator.SetFloat("Floating", 0f);
                herobrine.animator.SetFloat("Speed", 1f);
                herobrine.animator.SetFloat("Crouching", 0f);
            }

            public override bool GetStateConditional(Herobrine herobrine)
            {
                return UnityEngine.Random.value > 0.95f;
            }

        }

        public class Flee : HerobrineState
        {
            Vector3 startPos;
            Vector3 runDirection;
            bool groundFlee;
            float runSpeed;

            float downSpeed;

            float fleeDelay = 5f;
            float fleeTimer;

            public override void EnterState(Herobrine herobrine)
            {
                startPos = herobrine.transform.position;
                herobrine.allVisuals.SetActive(true);
                alpha = 1f;
                herobrine.agent.enabled = false;

                fleeTimer = fleeDelay;

                groundFlee = UnityEngine.Random.value > 0.499f;


                if(herobrine.target == null)
                {
                    herobrine.NextState();
                    return;
                }

                Vector3 playerPosition = herobrine.target.position;

                Vector3 pos = herobrine.head.position;

                runDirection = pos - playerPosition;
                runDirection.y = 0f;
                runDirection.Normalize();
            }

            public override void UpdateState(Herobrine herobrine)
            {
                if (groundFlee)
                    FleeGround(herobrine);
                else
                    FleeWall(herobrine);

                fleeTimer -= Time.deltaTime;

                if(fleeTimer < 0.0f && !groundFlee)
                {
                    herobrine.stateBuffer = herobrine.lurk;
                    herobrine.SwitchState(herobrine.fadeAway);
                }
            }

            private void FleeGround(Herobrine herobrine)
            {

                Vector3 ppos = Vector3.zero;

                if(herobrine.target != null)
                {
                    ppos = herobrine.target.position;
                }

                herobrine.SmoothRotateHeadToFace(ppos);

                Vector3 moveDelta = Vector3.down * downSpeed * Time.deltaTime;
                herobrine.transform.position += moveDelta;

                Vector3 currentPos = herobrine.transform.position;

                float verticalMoveDistance = currentPos.y - startPos.y;
                float distanceFromFeetToHead = (herobrine.head.position - currentPos).magnitude;

                bool nextState = verticalMoveDistance > distanceFromFeetToHead;

                verticalMoveDistance = Mathf.Clamp(verticalMoveDistance, 0, distanceFromFeetToHead);

                alpha = 1-(verticalMoveDistance/distanceFromFeetToHead);

                if(nextState)
                {
                    herobrine.NextState();
                }
            }

            private void FleeWall(Herobrine herobrine)
            {
                herobrine.LookInDirection(runDirection);
                herobrine.transform.rotation = Quaternion.LookRotation(runDirection);

                Vector3 moveDelta = herobrine.transform.forward * runSpeed * Time.deltaTime;

                if(Physics.Raycast(herobrine.head.position, herobrine.head.forward, out RaycastHit hit, 20f, LayerMaskDefaults.Get(LMD.Environment)))
                {
                    walkingTowardsWall = true;
                    distanceToWall = hit.distance;

                    distanceToWall = Mathf.Min(distanceToWallMax, distanceToWall);
                    alpha = distanceToWall / distanceToWallMax;

                    if (hit.distance-moveDelta.magnitude < 0)
                    {
                        herobrine.transform.position += moveDelta.normalized*hit.distance;
                        herobrine.NextState();
                        return;
                    }

                }

                herobrine.transform.position += moveDelta;
            }

            float distanceToWall;
            float distanceToWallMax = 5f;

            bool walkingTowardsWall;

            float alpha =1f;

            public override void UpdateAnimation(Herobrine herobrine)
            {
                herobrine.SetAlpha(alpha);

                herobrine.animator.SetFloat("Floating", 0f);
                herobrine.animator.SetFloat("Speed", (groundFlee) ? 0 : 1);
                herobrine.animator.SetFloat("Crouching", 0f);
            }
        }
    }
}
