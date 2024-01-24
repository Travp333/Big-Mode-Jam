using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using StateMachine;
using UnityEngine.Audio;

public class EnemyBaseAI : MonoBehaviour
{
    public const float DISTANCETHATISCLOSEENOUGH = 8;
    #region AI
    public EnemyStateMachine AI = new EnemyStateMachine();
    public NewEnemyAnimStateController AnimationStates;
    public static EnemyIdleState IdleState = new EnemyIdleState();
    public static EnemySuspiciousState SuspiciousState = new EnemySuspiciousState();
    public static EnemyChaseState ChaseState = new EnemyChaseState();
    public static EnemyLostPlayerState LostPlayerState = new EnemyLostPlayerState();
    public static EnemyStunnedState StunnedState = new EnemyStunnedState();
    public static EnemyPlayerSpottedState PlayerSpotted = new EnemyPlayerSpottedState();
    public static EnemyRiseState RiseState = new EnemyRiseState();
    public static EnemyDamagedState DamagedState = new EnemyDamagedState();
    public static EnemyLookAroundState LookAround = new EnemyLookAroundState();
    public static EnemyPatrolState PatrolState = new EnemyPatrolState();

    public static EnemyRagdollState RagdollState = new EnemyRagdollState();
    public static EnemySlipState SlipState = new EnemySlipState();
    public static EnemySmashedState SmashedState = new EnemySmashedState();
    public static EnemyGluedState gluedState = new EnemyGluedState();
    public static EnemyGrabPlayerState grabState = new EnemyGrabPlayerState();
    public static EnemyChokingPlayerState chokingPlayerState = new EnemyChokingPlayerState();
    public static EnemyLookingState LookingState = new EnemyLookingState();
    public static EnemyDumpPlayerState DumpingState = new EnemyDumpPlayerState();
    public static EnemyFallInHoleState FallInHoleState = new EnemyFallInHoleState();
    //public static EnemySlipState SlipState = new EnemySlipState();




    public NavMeshAgent Agent;
    public RagdollSwap RagdollScript;
    #endregion
    public bool TestPatrol;
    public PatrolData PatrolPoints;


    public EnemyData EnemyData;
    public Transform EyeTransform; // Where detection raycasts originate
    public LayerMask PlayerDetectionMask;
    public LayerMask EnvironmentDetectionMask;
    public TMPro.TMP_Text DebugCommentText;
    public TMPro.TMP_Text DebugStateText;

    [SerializeField]
    SFXManager sfx;
    [SerializeField]
    AudioSource audioSource;

    [HideInInspector] public Vector3 PointOfInterest;
    [HideInInspector] public float Timer;

    [SerializeField] Transform PlayerTransform;
    Ray _playerRay;
    RaycastHit _hit;


    // Used by PickupPlayer
    Movement playerMovement;
    [HideInInspector] public GameObject GrabbedObject;
    public Transform HandTransform;
    Transform playerRoot;
    PlayerStates playerStates;
    GameObject playerDummy;
    OrbitCamera orbCam;
    PlayerColorChangeBehavior colorChange;
    [HideInInspector] public TubeEntry tube;

    Vector3 PlayerPosition {
        get
        {
            return PlayerTransform.position + Vector3.up; // Offsets transform position
        }
    }
    bool TextureIsBlackAtCoord (float TextureUCoord)
    {
        // If TextureUCoord < 0.5, the texture should be black, so should return true
        return (TextureUCoord < 0.5f) ? true : false;
    }
    readonly Vector3 FLATVECTOR = new Vector3(1,0,1);

    private void OnEnable()
    {
        ProjectileManager.ProjectileHit += LookAtProjectile;
        OptionsMenuManager.Unstuck += ReleasePlayer;
    }
    private void OnDisable()
    {
        ProjectileManager.ProjectileHit -= LookAtProjectile;
        OptionsMenuManager.Unstuck -= ReleasePlayer;
    }

    private void Awake()
    {
        playerRoot = GameObject.Find("Player").GetComponent<Transform>();
        if (!playerRoot) Debug.LogError("Could not find player");
        PlayerTransform = GameObject.Find("3rd Person Character").GetComponent<Transform>();

        playerMovement = playerRoot.GetComponentInChildren<Movement>();
        if (!playerMovement) Debug.LogError("Could not find player movement");

        playerStates = playerRoot.GetComponentInChildren<PlayerStates>();
        orbCam = playerRoot.GetComponentInChildren<OrbitCamera>();
        orbCam = playerRoot.GetComponentInChildren<OrbitCamera>();
        playerDummy = GetComponentInChildren<ColorChecker>(true).gameObject;
        colorChange = playerMovement.GetComponent<PlayerColorChangeBehavior>();

        if (TestPatrol)
        {
            AI.SetState(PatrolState, this);
        }
        else AI.SetState(IdleState, this);
    }
    private void Update()
    {
        AI.Update(this);
        //PlayerVisible();
    }

    public void GoToPlayer()
    {
        if (Agent.isOnNavMesh) Agent.SetDestination(PlayerTransform.position);
    }
    public void GoToPointOfInterest()
    {
        if (Agent.isOnNavMesh) Agent.SetDestination(PointOfInterest);
    }
    public float DistanceToPlayer {
        get {
            return Vector3.Distance(transform.position, playerMovement.transform.position);
        }
    }
    public bool PlayerBehindCover()
    {
        return Physics.Linecast(EyeTransform.position, PlayerPosition, out _hit, PlayerDetectionMask) && _hit.collider.tag != "Player";
    }
    public void LookAtProjectile(object sender, ImpactParams parameters)
    {
        if (Vector3.Distance(transform.position, parameters.ImpactPoint) > EnemyData.DistractionRadius) return;
        // prevent getting locked in suspicious state
	    //  if (AI.CurrentState == SuspiciousState || AI.CurrentState == ChaseState) return;
	    // PointOfInterest = parameters.ImpactPoint;
	    //  GoToPointOfInterest();
	    //   if (PointOfInterestVisible(EnemyData.DistractionImmediateDetectionRadius, false))
	    //   {
	    //      AI.SetState(SuspiciousState, this);
	    //    } else if (PointOfInterestVisible(EnemyData.DistractionRadius))
	    //    {
	    //           AI.SetState(SuspiciousState, this);
	    //   }
    }
    public void TakeDamage()
    {
        audioSource.PlayOneShot(sfx.sillyImpact);
        AI.SetState(DamagedState, this);
    }
    public bool PointOfInterestVisible(float radius = -1, bool CheckIfPointVisible = true)
    {
        float distToPOI = Vector3.Distance(EyeTransform.position, PointOfInterest);
        if (radius > 0)
        {
            if (distToPOI > radius) return false;
            // if CheckIfPointVisible is false, alert this enemy if the projectile hit in the radius
            else if (!CheckIfPointVisible) return true;
        }

        // returns true if
        Physics.Linecast(EyeTransform.position, PointOfInterest, out _hit, PlayerDetectionMask);
        return Vector3.Distance(EyeTransform.position, _hit.point) >= distToPOI;
    }
    public bool PlayerWalkingNear()
    {
        if (playerStates.moving)
        {
            if (playerStates.crouching) return false;
            if (playerStates.walking)
            {
                if (Vector3.Distance(PlayerPosition, EyeTransform.position) < EnemyData.WalkingFootstepDetectionRange)
                {
                    PointOfInterest = PlayerPosition;
                    return true;
                }
            } else
            {
                if (Vector3.Distance(PlayerPosition, EyeTransform.position) < EnemyData.RunningFootstepDetectionRange)
                {
                    PointOfInterest = PlayerPosition;
                    return true;
                }
            }
        }
        return false;
    }

    public void PickupPlayer()
	{

        bool gotPlayer = false;
        // Check if the player is still close enough to grab
        foreach (Collider col in Physics.OverlapSphere(HandTransform.position, EnemyData.GrabRadius, PlayerDetectionMask, QueryTriggerInteraction.Ignore))
        {
            if (col.tag == "Player")
            {
                gotPlayer = true;
                audioSource.PlayOneShot(sfx.EnemyAlert);
                break;
            }
        }
        if (!gotPlayer)
        {
            Debug.Log("Missed the player");
            return;
        }

        GrabbedObject = playerRoot.gameObject;
        playerDummy.SetActive(true);

        playerStates.ForceThirdPerson();
        if (playerStates.holding) playerStates.pickup.PutDown();
        playerStates.SetFPSBlock(true);

        orbCam.focus = HandTransform;
        playerMovement.blockMovement();
        playerStates.choked = true;
        playerStates.standingHitbox.SetActive(false);
	    playerStates.crouchingHitbox.SetActive(false);
		//playerStates.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        foreach (SkinnedMeshRenderer m in colorChange.mesh)
        {
            m.enabled = false;
        }
        colorChange.face.enabled = false;
    }
    public void DumpPlayerInPipe()
    {
        GrabbedObject.transform.position = HandTransform.position;
        ReleasePlayer();
        if (tube) tube.DumpPlayer(playerRoot);
        tube = null;
    }
    public void ReleasePlayer()
	{
		//playerStates.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        //Got player root!
        GrabbedObject = null;
        playerDummy.SetActive(false);
        //update camera position
        orbCam.focus = playerMovement.center.transform;
        //disable player movement
        playerMovement.unblockMovement();
        //player.GetComponent<Movement>().enabled = true;
        playerStates.SetFPSBlock(false);
        playerStates.choked = false;
        playerStates.crouching = false;
        playerStates.standingHitbox.SetActive(true);
        playerStates.crouchingHitbox.SetActive(false);

        foreach (SkinnedMeshRenderer m in colorChange.mesh)
        {
            if (m.name != "Sling Mesh" && m.name != "FPSArms" && m.name != "FPSSling" && m.name != "Cylinder" && m.name != "Cylinder.001")
            {
                m.enabled = true;
            }
        }
        colorChange.face.enabled = true;
    }
    public void GotPlayer()
    {
        if (GrabbedObject != null) // If the enemy is holding something
        {
            AI.SetState(chokingPlayerState, this);
        } else
        {
            AI.SetState(ChaseState, this);
        }
    }
    public bool PlayerVisible()
    {
        if (Vector3.Distance(transform.position, PlayerPosition) > 300) return false;
        _playerRay = new Ray(EyeTransform.position, PlayerPosition - EyeTransform.position);
        float angle = Vector3.Angle(EyeTransform.forward, _playerRay.direction);
        if (angle > EnemyData.DetectionFOV)
        {
            if (DebugCommentText) DebugCommentText.text = "Player out of FOV";
            return false;
        }
        if (!PlayerColorChangeBehavior.Instance)
        {
            Debug.Log("No PlayerColorChangeBehavior is in the scene.");
            return false;
        }
        if (PlayerColorChangeBehavior.Instance.IsChanging) return false; // The player's invisible when changing color
        if (ScanPlayer(Vector3.up*2)) return true;
        if (ScanPlayer(Vector3.up*6)) return true;
        return false;

        bool ScanPlayer(Vector3 position)
        {
            _playerRay = new Ray(EyeTransform.position, PlayerPosition - EyeTransform.position + position);
            Debug.DrawLine(EyeTransform.position, PlayerPosition + position, Color.green);
            if (Physics.Linecast(EyeTransform.position, PlayerPosition + position, out _hit, PlayerDetectionMask) && _hit.collider.tag == "Player")
            {
	            if (DebugCommentText) DebugCommentText.text = "Player Visible";

                if (Physics.Raycast(EyeTransform.position, _playerRay.direction, out _hit, 100, EnvironmentDetectionMask))
                {

                    Debug.DrawLine(EyeTransform.position, _hit.point, Color.red);
                    if (_hit.collider.tag == "Environment")
                    {
                    	Debug.Log("LAYER " + _hit.collider.gameObject.layer);
	                    //Debug.Log(_hit.collider.name + " u coord: " + _hit.textureCoord.x);
	                    //UV CHECKING THING HERE
	                    //if (PlayerColorChangeBehavior.Instance.IsBlack == TextureIsBlackAtCoord(_hit.textureCoord.x))
	                    if (PlayerColorChangeBehavior.Instance.IsBlack && (_hit.collider.gameObject.layer == 15))
                        {
                            if (DebugCommentText) DebugCommentText.text = "Materials match!";
	                        return false;
                            
                        }
	                    else if (!PlayerColorChangeBehavior.Instance.IsBlack && (_hit.collider.gameObject.layer == 16))
	                    {
		                    if (DebugCommentText) DebugCommentText.text = "Materials match!";
		                    return false;
                            
	                    }
                        else
                        {
                            if (DebugCommentText) DebugCommentText.text = "Materials DO NOT match";
                            return true;
                        }
                    }
                    if (_hit.collider.tag == "Unblendable")
                    {
                        if (DebugCommentText) DebugCommentText.text = "Player in front of unblendable wall";
                        return true;
                    }
                }
            }
            else
            {
	            if (DebugCommentText) DebugCommentText.text = "Player Not Visible";
            }
            return false;
        }
    }
    public void FaceObjectOfInterest()
    {

        Vector3 objectVector = PointOfInterest- transform.position;
        transform.rotation = Quaternion.Lerp( transform.rotation, Quaternion.LookRotation(Vector3.Scale(objectVector, FLATVECTOR)), Time.deltaTime * EnemyData.TurnSpeed );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(PointOfInterest, 2);
	    //Handles.Label(PointOfInterest + Vector3.up, "Point Of Interest");
    }
public class EnemyIdleState : EnemyBaseState
    {
        public override string Name() { return "Idle"; }
        public override void Enter(EnemyBaseAI owner) {
            //owner.AnimationStates.chaseDesired = false;
            owner.audioSource.PlayOneShot(owner.sfx.EnemyAlert);
            owner.AnimationStates.Anim.CrossFade(owner.AnimationStates.idleHash, 0.1f);
            owner.Agent.isStopped = true;
            if (owner.GrabbedObject) owner.ReleasePlayer();
        }
        public override void Update(EnemyBaseAI owner) {
            //owner.PlayerVisible();
            if (owner.PlayerVisible()) owner.AI.SetState(SuspiciousState, owner, true);
            if (owner.PlayerWalkingNear())
            {
                owner.AI.SetState(SuspiciousState, owner, true);
            }
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState) {
            owner.Agent.isStopped = false;
        }
    }
    public class EnemySuspiciousState : EnemyBaseState
    {
        public override string Name() { return "Suspicious"; }
        public override void Enter(EnemyBaseAI owner)
        {

            owner.Agent.isStopped = true;
            owner.AnimationStates.Anim.CrossFade(owner.AnimationStates.susHash, 0.1f);
            owner.Timer = owner.EnemyData.ReactionTime;
        }
        public override void Update(EnemyBaseAI owner)
        {
            if (owner.PlayerVisible()) owner.PointOfInterest = owner.PlayerPosition;
            owner.FaceObjectOfInterest();

            // Transition
            if (owner.Timer > 0)
                owner.Timer -= Time.deltaTime;
            else
            {
                if (owner.PlayerVisible() || (owner.PlayerWalkingNear() && !owner.PlayerBehindCover()))
                {
                    owner.AI.SetState(PlayerSpotted, owner, true);
                } else if (owner.TestPatrol)
                {
                    owner.AI.SetState(PatrolState, owner, true);
                }
                else
                {
                    owner.AI.SetState(IdleState, owner, true);
                }
            }
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
            owner.Agent.isStopped = false;
        }
    }
    public class EnemyPlayerSpottedState : EnemyBaseState
    {
        public override string Name() { return "PlayerSpotted"; }
        public override void Enter(EnemyBaseAI owner)
        {
            owner.Agent.isStopped = true;
            owner.Timer = owner.EnemyData.SurprisedDuration;

            //owner.AnimationStates.noticingDesired = true;
            owner.AnimationStates.Anim.CrossFade(owner.AnimationStates.startledHash, 0.1f);

        }
        public override void Update(EnemyBaseAI owner)
        {
            //owner.PlayerVisible();
            if (owner.Timer > 0)
                owner.Timer -= Time.deltaTime;
            else
            {
                NavMeshPath navMeshPath = new NavMeshPath();

                if (owner.Agent.CalculatePath(owner.PlayerPosition, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    owner.AI.SetState(ChaseState, owner, true);
                } else
                {
                    owner.AI.SetState(LookingState, owner, true);
                }
            }
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
            owner.Agent.isStopped = false;
        }
    }
    public class EnemyLookingState : EnemyBaseState
    {
        public override string Name() { return "Looking at Player"; }
        public override void Enter(EnemyBaseAI owner)
        {
            owner.Agent.isStopped = true;
            owner.AnimationStates.Anim.CrossFade(owner.AnimationStates.idleHash, 0.1f);
            owner.Timer = 10;
        }
        public override void Update(EnemyBaseAI owner)
        {
            owner.PointOfInterest = owner.PlayerPosition;
            owner.FaceObjectOfInterest();
            if (owner.Timer > 0) owner.Timer -= Time.deltaTime;
            else owner.AI.SetState(IdleState, owner);
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
            owner.Agent.isStopped = false;
        }
    }
    public class EnemyChaseState : EnemyBaseState
    {
        public override string Name() { return "Chasing"; }
        public override void Enter(EnemyBaseAI owner)
        {
            owner.Agent.speed = owner.EnemyData.RunSpeed;
            //owner.AnimationStates.chaseDesired = true;
            owner.AnimationStates.Anim.CrossFade(owner.AnimationStates.chaseHash, 0.1f);

        }
        public override void Update(EnemyBaseAI owner)
        {
            owner.GoToPlayer();
            if (owner.PlayerBehindCover()) // if the player is behind cover! wow!
            {
                owner.AI.SetState(LostPlayerState, owner, true);
            }
            if (owner.DistanceToPlayer < owner.EnemyData.GrabDistance)
            {
                owner.AI.SetState(grabState, owner);
            }
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
            owner.Agent.speed = owner.EnemyData.WalkSpeed;
            //owner.AnimationStates.chaseDesired = false;
        }
    }
    public class EnemySlipState : EnemyBaseState
    {
        public override string Name() { return "Slipping"; }
        public override void Enter(EnemyBaseAI owner)
        {
            owner.Agent.isStopped = true;
            owner.Timer = owner.EnemyData.SlipDuration;
            //owner.AnimationStates.slipDesired = true;
            owner.AnimationStates.Anim.CrossFade(owner.AnimationStates.slipHash, 0.1f);

        }
        public override void Update(EnemyBaseAI owner)
        {
            if (owner.Timer > 0) owner.Timer -= Time.deltaTime;
            else  owner.AI.SetState(StunnedState, owner, true);
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
            //owner.AnimationStates.slipDesired = false;
            //owner.AnimationStates.onBackDesired = true;
            owner.AnimationStates.Anim.CrossFade(owner.AnimationStates.onBackHash, 0.1f);

        }
    }
    public class EnemySmashedState : EnemyBaseState
    {
        public override string Name() { return "Pulverized"; }
        public override void Enter(EnemyBaseAI owner)
        {
            owner.Agent.velocity = Vector3.zero;
            owner.Agent.isStopped = true;
            owner.AnimationStates.Anim.CrossFade(owner.AnimationStates.squashedHash, 0.1f);
        }
        public override void Update(EnemyBaseAI owner)
        {
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
        }
    }
    public class EnemyGluedState : EnemyBaseState
    {
        public override string Name() { return "Glued"; }
        public override void Enter(EnemyBaseAI owner)
        {
            owner.Agent.isStopped = true;
            owner.AnimationStates.Anim.CrossFade(owner.AnimationStates.stuckHash, 0.1f);
        }
        public override void Update(EnemyBaseAI owner)
        {
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
        }
    }
    public class EnemyStunnedState: EnemyBaseState
    {
        public override string Name() { return "Stunned"; }
        public override void Enter(EnemyBaseAI owner)
        {
            owner.Timer = owner.EnemyData.StunDuration;
        }
        public override void Update(EnemyBaseAI owner)
        {
            if (owner.Timer > 0)
            {
                owner.Timer -= Time.deltaTime;
            } else
            {
                owner.AI.SetState(RiseState, owner, true);
            }
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
            //owner.AnimationStates.onBackDesired = false;
        }
    }
    public class EnemyChokingPlayerState : EnemyBaseState
    {
        public override string Name() { return "Choking Player"; }
        public override void Enter(EnemyBaseAI owner)
	    {
		    owner.Agent.speed = owner.EnemyData.GrabSpeed;
		    owner.Timer = 10;
            owner.AnimationStates.Anim.CrossFade(owner.AnimationStates.grabWalkHash, 0.1f);
            float nearest = -1;
            float dist;
            Vector3 pos, nearestPos = Vector3.zero;
            bool foundTrash = false;
            NavMeshPath navMeshPath = new NavMeshPath();
            GameObject[] chutes = GameObject.FindGameObjectsWithTag("Trash");
            foreach (GameObject obj in chutes)
            {
                dist = Vector3.Distance(owner.transform.position, obj.transform.position);
                pos = obj.transform.position;


                if (!foundTrash)
                {
                    if (owner.Agent.CalculatePath(pos, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
                    {
                        foundTrash = true;
                        nearest = dist;
                        nearestPos = pos;
                    }
                }
                else
                {
                    if (owner.Agent.CalculatePath(pos, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
                    {
                        if (dist < nearest)
                        {
                            nearest = dist;
                            nearestPos = pos;

                        }
                    }
                }
                Debug.Log(obj.name + " Distance: " + dist + " nearest thing so far: " + nearest);

            }
            if (foundTrash)
            {
                owner.Agent.SetDestination(nearestPos);
            } else
            {
                owner.Timer = 10;
                Debug.LogError("No trash chute found. Make sure any of them are tagged \"Trash\"");
            }
        }
        public override void Update(EnemyBaseAI owner)
        {
	        if (owner.GrabbedObject) owner.GrabbedObject.GetComponentInChildren<Movement>().gameObject.transform.position = owner.HandTransform.position;

            owner.Timer -= Time.deltaTime;
            if (owner.Timer < 0) owner.AI.SetState(SlipState, owner);
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
            if (newState != DumpingState) owner.ReleasePlayer();
        }
    }

    public class EnemyDumpPlayerState : EnemyBaseState
    {
        public override string Name() { return "Dumping"; }
        public override void Enter(EnemyBaseAI owner)
        {
            owner.Agent.isStopped = true;
            owner.AnimationStates.Anim.CrossFade(owner.AnimationStates.dumpHash, 0.1f);
            owner.Timer = 1.5f;
        }
        public override void Update(EnemyBaseAI owner)
        {
            if (owner.Timer > 0) owner.Timer -= Time.deltaTime;
            else
            {
                if (owner.TestPatrol)
                {
                    owner.AI.SetState(PatrolState, owner, true);
                }
                else owner.AI.SetState(IdleState, owner, true);
            }
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
            owner.Agent.isStopped = false;
        }
    }

    public class EnemyGrabPlayerState : EnemyBaseState
    {
        public override string Name() { return "Grabbing Player"; }
        public override void Enter(EnemyBaseAI owner)
	    {

            owner.Agent.isStopped = true;
            owner.AnimationStates.Anim.CrossFade(owner.AnimationStates.grabHash, 0.1f);
            owner.Timer = 1.5f;
        }
        public override void Update(EnemyBaseAI owner)
        {
            if (owner.Timer > 0) owner.Timer -= Time.deltaTime;
            else owner.AI.SetState(ChaseState, owner);
            //owner.GrabbedObject.transform.position = owner.HandTransform.position;
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
            owner.Agent.isStopped = false;
        }
    }

    public class EnemyPatrolState : EnemyBaseState
    {
        public override string Name() { return "Patrolling"; }
        public override void Enter(EnemyBaseAI owner)
	    {
		    //Debug.Log(owner.PatrolPoints.Points.Length);
            owner.Agent.speed = owner.EnemyData.WalkSpeed;
            if (owner.PatrolPoints != null)
                owner.PointOfInterest = owner.PatrolPoints.TargetPoint;
            else
            {
                Debug.LogError("Must have a reference to " + owner.PatrolPoints.GetType().Name);
                owner.TestPatrol = false;
                owner.AI.SetState(IdleState, owner, true);
            }
            owner.Agent.stoppingDistance = 0;
            owner.GoToPointOfInterest();
            owner.AnimationStates.Anim.CrossFade(owner.AnimationStates.walkHash, 0.1f);
        }
        public override void Update(EnemyBaseAI owner)
        {
            float dist = Vector3.Distance(owner.transform.position, owner.PointOfInterest);
            if (dist < DISTANCETHATISCLOSEENOUGH)
            {
                if (owner.PatrolPoints.Points.Length <= 1)
                {
                    owner.AI.SetState(IdleState, owner);
                }
                owner.PatrolPoints.SetNextPatrolPoint();
                owner.PointOfInterest = owner.PatrolPoints.TargetPoint;
                owner.GoToPointOfInterest();
            }
            if (owner.PlayerVisible()) owner.AI.SetState(SuspiciousState, owner, true);
            if (owner.PlayerWalkingNear()) owner.AI.SetState(SuspiciousState, owner, true);
            owner.DebugCommentText.text = dist.ToString();
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
            //owner.AnimationStates.onBackDesired = false;
            owner.Agent.stoppingDistance = owner.EnemyData.StoppingDistance;
        }
    }

    public class EnemyRiseState : EnemyBaseState
    {
        public override string Name() { return "Rising"; }
        public override void Enter(EnemyBaseAI owner)
        {
            owner.Timer = owner.EnemyData.RiseDuration;
            //owner.GetComponent<Animator>()?.Play("Get Up");
            owner.AnimationStates.Anim.CrossFade(owner.AnimationStates.getUpHash, 0.1f);
        }
        public override void Update(EnemyBaseAI owner)
        {
            if (owner.Timer > 0)
            {
                owner.Timer -= Time.deltaTime;
            }
            else
            {
                if (owner.PlayerVisible())
                {
                    owner.AI.SetState(ChaseState, owner, true);
                }
                else
                {
                    owner.AI.SetState(SuspiciousState, owner, true);
                }
            }
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
            owner.Agent.isStopped = false;
        }
    }
    public class EnemyDamagedState : EnemyBaseState
    {
        public override string Name() { return "Damaged"; }
        public override void Enter(EnemyBaseAI owner)
        {
            owner.Timer = owner.EnemyData.HitFlinchDuration;
            owner.Agent.isStopped = true;
            //owner.AnimationStates.damageDesired = true;
            owner.AnimationStates.Anim.CrossFade(owner.AnimationStates.gettingHitHash, 0.1f);

        }
        public override void Update(EnemyBaseAI owner)
        {
            if (owner.Timer > 0)
            {
                owner.Timer -= Time.deltaTime;
            }
            else
            {
                if (owner.PlayerBehindCover())
                {
                    owner.AI.SetState(SuspiciousState, owner, true);
                } else
                {
                    NavMeshPath navMeshPath = new NavMeshPath();

                    if (owner.Agent.CalculatePath(owner.PlayerPosition, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
                    {
                        owner.AI.SetState(ChaseState, owner, true);
                    } else
                    {
                        owner.AI.SetState(LookingState, owner, true);
                    }
                }
            }
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
            owner.Agent.isStopped = false;
            //owner.AnimationStates.damageDesired = false;
        }
    }
    public class EnemyLostPlayerState : EnemyBaseState
    {
        public override string Name() { return "Player Lost"; }
        public override void Enter(EnemyBaseAI owner)
        {
            owner.Agent.stoppingDistance = 0;
            owner.Agent.speed = owner.EnemyData.RunSpeed;

            // Go to the place the player was last seen at
            owner.PointOfInterest = owner.PlayerTransform.position;
            owner.GoToPointOfInterest();
        }
        public override void Update(EnemyBaseAI owner)
        {
            // Check if near where the player was last seen
            float dist = Vector3.Distance(owner.transform.position, owner.PointOfInterest);
            if (dist < DISTANCETHATISCLOSEENOUGH)
            {
                owner.AI.SetState(LookAround, owner, true);
            }
            //Chase the player if they become visible again
            if (!owner.PlayerBehindCover()) owner.AI.SetState(ChaseState, owner, true);
            //if (owner.PlayerVisible()) owner.AI.SetState(ChaseState, owner);
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
            owner.Agent.stoppingDistance = owner.EnemyData.StoppingDistance;
        }
    }
    public class EnemyLookAroundState : EnemyBaseState
    {
        public override string Name() { return "Lookin' Around"; }
        public override void Enter(EnemyBaseAI owner)
        {
            owner.Agent.isStopped = true;
            //owner.AnimationStates.searchingDesired = true;
            owner.AnimationStates.Anim.CrossFade(owner.AnimationStates.searchingHash, 0.1f);

            owner.Timer = owner.EnemyData.SuspiciousTime;
        }
        public override void Update(EnemyBaseAI owner)
        {
            if (owner.Timer > 0)
            {
                owner.Timer -= Time.deltaTime;
            }
            else
            {
                owner.Timer = Random.Range(1f, 3f);
                float randomAngle = Random.Range(40, 160);
                if (Random.Range(0, 2) == 0) // coin flip
                    randomAngle = -randomAngle;
                owner.PointOfInterest = owner.transform.position + (Quaternion.Euler(0, randomAngle, 0) * owner.transform.forward);
            }

            owner.FaceObjectOfInterest();

            if (owner.Timer > 0)
            {
                owner.Timer -= Time.deltaTime;
            }
            else
            {
                if (owner.TestPatrol)
                {
                    owner.AI.SetState(PatrolState, owner, true);
                }
                else owner.AI.SetState(IdleState, owner, true);
            }
            if (owner.PlayerVisible()) owner.AI.SetState(SuspiciousState, owner, true);
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
            owner.Agent.isStopped = false;
        }
    }
    public class EnemyRagdollState : EnemyBaseState
    {
        public override string Name() { return "PlayerSpotted"; }
        public override void Enter(EnemyBaseAI owner)
        {
            if (owner.GrabbedObject) owner.ReleasePlayer();
            owner.RagdollScript.StartRagdoll();
            owner.Timer = owner.EnemyData.RagdollDuration;
        }
        public override void Update(EnemyBaseAI owner)
        {
            //owner.PlayerVisible();
            if (owner.Timer > 0)
                owner.Timer -= Time.deltaTime;
            else
                owner.AI.SetState(RiseState, owner, true);
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
            //owner.AnimationStates.noticingDesired = false;
            owner.RagdollScript.RevertRagdoll();
        }
    }
    public class EnemyFallInHoleState : EnemyBaseState
    {
        public override string Name() { return "PlayerSpotted"; }
        public override void Enter(EnemyBaseAI owner)
        {
            if (owner.GrabbedObject) owner.ReleasePlayer();
            owner.Agent.velocity = Vector3.zero;
            owner.Agent.isStopped = true;
            owner.AnimationStates.Anim.CrossFade(owner.AnimationStates.fallHash, 0.1f);
        }
        public override void Update(EnemyBaseAI owner)
        {
        }
        public override void Exit(EnemyBaseAI owner, EnemyBaseState newState)
        {
        }
    }
}

namespace StateMachine
{
    public abstract class EnemyBaseState
    {
        public abstract string Name();
        public virtual void Enter(EnemyBaseAI owner) { throw new System.NotImplementedException(); }
        public virtual void Update(EnemyBaseAI owner) { throw new System.NotImplementedException(); }
        public virtual void Exit(EnemyBaseAI owner, EnemyBaseState newState) { throw new System.NotImplementedException(); }
    }
    public class EnemyStateMachine
    {
        public EnemyBaseState LastState { get; private set; }
        public EnemyBaseState CurrentState { get; private set; }

        public void SetState(EnemyBaseState newState, EnemyBaseAI owner, bool calledInternally = false)
        {
            if (CurrentState != null)
            {
                // Prevent these states from being interrupted
                if (!calledInternally)
                {
                    switch (CurrentState)
                    {
                        case EnemyBaseAI.EnemyGluedState:
                        case EnemyBaseAI.EnemyStunnedState:
                        case EnemyBaseAI.EnemyRagdollState:
                        case EnemyBaseAI.EnemyRiseState:
                        case EnemyBaseAI.EnemySlipState:
                        case EnemyBaseAI.EnemySmashedState:
                        case EnemyBaseAI.EnemyFallInHoleState:
                            return;
                    }
                }

                LastState = CurrentState;
                CurrentState?.Exit(owner, newState);
            }
            CurrentState = newState;
            if (owner.DebugStateText) owner.DebugStateText.text = newState.Name();
            CurrentState.Enter(owner);
        }

        public void Update(EnemyBaseAI owner)
        {
            CurrentState.Update(owner);
        }
    }
}
