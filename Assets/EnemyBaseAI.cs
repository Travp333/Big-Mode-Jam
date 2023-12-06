using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateMachine;

public class EnemyBaseAI : MonoBehaviour
{
    #region AI
    public EnemyStateMachine AI = new EnemyStateMachine();
    public EnemyAnimController AnimationStates;
    public static EnemyIdleState IdleState = new EnemyIdleState();
    public static EnemySuspiciousState SuspiciousState = new EnemySuspiciousState();
    public static EnemyChaseState ChaseState = new EnemyChaseState();
    public static EnemyLostPlayerState LostPlayerState = new EnemyLostPlayerState();
    public static EnemyStunnedState StunnedState = new EnemyStunnedState();
    public static EnemyPlayerSpottedState PlayerSpotted = new EnemyPlayerSpottedState();
    public static EnemySlipState SlipState = new EnemySlipState();
    public static EnemyRiseState RiseState = new EnemyRiseState();
    public static EnemyDamagedState DamagedState = new EnemyDamagedState();
    public static EnemyLookAroundState LookAround = new EnemyLookAroundState();

    public NavMeshAgent Agent;
    #endregion

    public EnemyData EnemyData;
    public Transform EyeTransform; // Where detection raycasts originate
    public LayerMask PlayerDetectionMask;
    public LayerMask EnvironmentDetectionMask;
    public TMPro.TMP_Text DebugCommentText;
    public TMPro.TMP_Text DebugStateText;

    [HideInInspector] public Vector3 PointOfInterest;

    [SerializeField ]Transform PlayerTransform;
    Ray _playerRay;
    RaycastHit _hit;
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
    }
    private void OnDisable()
    {
        ProjectileManager.ProjectileHit -= LookAtProjectile;
    }

    private void Awake()
    {
        if (!PlayerTransform) PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();/*
        Debug.Log(AI);
        Debug.Log(IdleState);*/
        AI.SetState(IdleState, this);
    }
    private void Update()
    {
        AI.Update(this);
    }

    public void GoToPlayer()
    {
        if (Agent.isOnNavMesh) Agent.SetDestination(PlayerTransform.position);
    }
    public void GoToPointOfInterest()
    {
        if (Agent.isOnNavMesh) Agent.SetDestination(PointOfInterest);
    }
    public bool PlayerBehindCover()
    {
        return Physics.Linecast(EyeTransform.position, PlayerPosition, out _hit, PlayerDetectionMask) && _hit.collider.tag != "Player";
    }
    public void LookAtProjectile(object sender, ImpactParams parameters)
    {
        // prevent getting locked in suspicious state
        if (AI.CurrentState == SuspiciousState || AI.CurrentState == ChaseState) return;
        PointOfInterest = parameters.ImpactPoint;
        if (PointOfInterestVisible(EnemyData.DistractionImmediateDetectionRadius, false))
        {
            AI.SetState(SuspiciousState, this);
        } else if (PointOfInterestVisible(EnemyData.DistractionRadius))
        {
            AI.SetState(SuspiciousState, this);
        }
    }
    public void TakeDamage()
    {
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
    public bool PlayerVisible()
    {
        if (!PlayerColorChangeBehavior.Instance)
        {
            Debug.Log("No PlayerColorChangeBehavior is in the scene.");
            return false;
        }
        if (PlayerColorChangeBehavior.Instance.IsChanging) return false; // The player's invisible when changing color
        _playerRay = new Ray(EyeTransform.position, PlayerPosition - EyeTransform.position);
        if (Vector3.Angle(EyeTransform.forward, _playerRay.direction) > EnemyData.DetectionFOV)
        {
            if (DebugCommentText) DebugCommentText.text = "Player out of FOV";
            return false;
        }

        Debug.DrawLine(EyeTransform.position, PlayerPosition, Color.green);
        if (Physics.Linecast(EyeTransform.position, PlayerPosition, out _hit, PlayerDetectionMask) && _hit.collider.tag == "Player")
        {
            if (DebugCommentText) DebugCommentText.text = "Player Visible";
            Debug.DrawRay(EyeTransform.position, _playerRay.direction, Color.red);

            if (Physics.Raycast(EyeTransform.position, _playerRay.direction, out _hit, 100, EnvironmentDetectionMask))
            {
                if (_hit.collider.tag == "Environment")
                {
                    if ( PlayerColorChangeBehavior.Instance.IsBlack == TextureIsBlackAtCoord(_hit.textureCoord.x))
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
        } else
        {
            if (DebugCommentText) DebugCommentText.text = "Player Not Visible";
        }            
        return false;
    }
    public void FaceObjectOfInterest()
    {
        Vector3 objectVector = PointOfInterest- transform.position;
        transform.rotation = Quaternion.Lerp( transform.rotation, Quaternion.LookRotation(Vector3.Scale(objectVector, FLATVECTOR)), Time.deltaTime * EnemyData.TurnSpeed );
    }
    private void OnDrawGizmos()
    {
            Gizmos.DrawWireSphere(PointOfInterest, 2);
    }
    public class EnemyIdleState : EnemyBaseState
    {
        public override string Name() { return "Idle"; }
        public override void Enter(EnemyBaseAI owner) {
        }
        public override void Update(EnemyBaseAI owner) {
            //owner.PlayerVisible();
            if (owner.PlayerVisible()) owner.AI.SetState(SuspiciousState, owner);
        }
        public override void Exit(EnemyBaseAI owner) {
        }
    }
    public class EnemySuspiciousState : EnemyBaseState
    {

        float _timer;
        public override string Name() { return "Suspicious"; }
        public override void Enter(EnemyBaseAI owner)
        {
            //owner.PointOfInterest = owner.PlayerPosition;
            owner.AnimationStates.susDesired = true;
            _timer = owner.EnemyData.ReactionTime;
        }
        public override void Update(EnemyBaseAI owner)
        {
            if (owner.PlayerVisible()) owner.PointOfInterest = owner.PlayerPosition;
            owner.FaceObjectOfInterest();

            // Transition
            if (_timer > 0)
                _timer -= Time.deltaTime;
            else
            {
                if (owner.PlayerVisible())
                {
                    owner.AI.SetState(PlayerSpotted, owner);
                } else
                {
                    owner.AI.SetState(IdleState, owner);
                }
            }
        }
        public override void Exit(EnemyBaseAI owner)
        {
            owner.AnimationStates.susDesired = false;
        }
    }
    public class EnemyPlayerSpottedState : EnemyBaseState
    {
        float _timer;

        public override string Name() { return "PlayerSpotted"; }
        public override void Enter(EnemyBaseAI owner)
        {
            _timer = owner.EnemyData.SurprisedDuration;

            owner.AnimationStates.noticingDesired = true;
        }
        public override void Update(EnemyBaseAI owner)
        {
            //owner.PlayerVisible();
            if (_timer > 0)
                _timer -= Time.deltaTime;
            else
                owner.AI.SetState(ChaseState, owner);
        }
        public override void Exit(EnemyBaseAI owner)
        {
            owner.AnimationStates.noticingDesired = false;
        }
    }
    public class EnemyChaseState : EnemyBaseState
    {
        public override string Name() { return "Chasing"; }
        public override void Enter(EnemyBaseAI owner)
        {
            owner.Agent.speed = owner.EnemyData.RunSpeed;
            owner.AnimationStates.chaseDesired = true;
        }
        public override void Update(EnemyBaseAI owner)
        {
            owner.GoToPlayer();
            if (owner.PlayerBehindCover()) // if the player is behind cover! wow!
            {
                owner.AI.SetState(LostPlayerState, owner);
            }
        }
        public override void Exit(EnemyBaseAI owner)
        {
            owner.Agent.speed = owner.EnemyData.WalkSpeed;
            owner.AnimationStates.chaseDesired = false;
        }
    }
    public class EnemySlipState : EnemyBaseState
    {
        float timer;
        public override string Name() { return "Slipping"; }
        public override void Enter(EnemyBaseAI owner)
        {
            owner.Agent.isStopped = true;
            timer = owner.EnemyData.SlipDuration;
            owner.AnimationStates.slipDesired = true;
        }
        public override void Update(EnemyBaseAI owner)
        {
            if (timer > 0) timer -= Time.deltaTime;
            else  owner.AI.SetState(StunnedState, owner);
        }
        public override void Exit(EnemyBaseAI owner)
        {
            owner.AnimationStates.slipDesired = false;
            owner.AnimationStates.onBackDesired = true;
        }
    }
    public class EnemyStunnedState: EnemyBaseState
    {
        float timer = 0;
        public override string Name() { return "Stunned"; }
        public override void Enter(EnemyBaseAI owner)
        {
            timer = owner.EnemyData.StunDuration;
        }
        public override void Update(EnemyBaseAI owner)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            } else
            {
                owner.AI.SetState(RiseState, owner);             
            }
        }
        public override void Exit(EnemyBaseAI owner)
        {
            owner.AnimationStates.onBackDesired = false;
        }
    }
    public class EnemyRiseState : EnemyBaseState
    {
        float timer = 0;
        public override string Name() { return "Rising"; }
        public override void Enter(EnemyBaseAI owner)
        {
            timer = owner.EnemyData.RiseDuration;
        }
        public override void Update(EnemyBaseAI owner)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                if (owner.PlayerVisible())
                {
                    owner.AI.SetState(ChaseState, owner);
                }
                else
                {
                    owner.AI.SetState(SuspiciousState, owner);
                }
            }
        }
        public override void Exit(EnemyBaseAI owner)
        {
            owner.Agent.isStopped = false;
        }
    }
    public class EnemyDamagedState : EnemyBaseState
    {
        float timer = 0;
        public override string Name() { return "Damaged"; }
        public override void Enter(EnemyBaseAI owner)
        {
            timer = owner.EnemyData.HitFlinchDuration;
            owner.Agent.isStopped = true;
            owner.AnimationStates.damageDesired = true;
        }
        public override void Update(EnemyBaseAI owner)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                if (owner.PlayerBehindCover())
                {
                    owner.AI.SetState(SuspiciousState, owner);
                } else
                {
                    owner.AI.SetState(ChaseState, owner);
                }
            }
        }
        public override void Exit(EnemyBaseAI owner)
        {
            owner.Agent.isStopped = false;
            owner.AnimationStates.damageDesired = false;
        }
    }
    public class EnemyLostPlayerState : EnemyBaseState
    {

        float _agentStopDist;
        public override string Name() { return "Player Lost"; }
        public override void Enter(EnemyBaseAI owner)
        {
            _agentStopDist = owner.Agent.stoppingDistance;
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
            if (dist < 3f)
            {
                owner.AI.SetState(LookAround, owner);
            }

            //Chase the player if they become visible again
            if (!owner.PlayerBehindCover()) owner.AI.SetState(ChaseState, owner);
            //if (owner.PlayerVisible()) owner.AI.SetState(ChaseState, owner);
        }
        public override void Exit(EnemyBaseAI owner)
        {
            owner.Agent.stoppingDistance = _agentStopDist;
        }
    }
    public class EnemyLookAroundState : EnemyBaseState
    {
        float _timer = 0;
        float _lookTimer = 0;
        public override string Name() { return "Lookin' Around"; }
        public override void Enter(EnemyBaseAI owner)
        {
            owner.Agent.isStopped = true;
            owner.AnimationStates.searchingDesired = true;
            _timer = owner.EnemyData.SuspiciousTime;
        }
        public override void Update(EnemyBaseAI owner)
        {
            if (_lookTimer > 0)
            {
                _lookTimer -= Time.deltaTime;
            }
            else
            {
                _lookTimer = Random.Range(1f, 3f);
                float randomAngle = Random.Range(40, 160);
                if (Random.Range(0, 2) == 0) // coin flip
                    randomAngle = -randomAngle;
                owner.PointOfInterest = owner.transform.position + (Quaternion.Euler(0, randomAngle, 0) * owner.transform.forward);
            }

            owner.FaceObjectOfInterest();

            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                owner.AI.SetState(IdleState, owner);
            }
            if (owner.PlayerVisible()) owner.AI.SetState(SuspiciousState, owner);
        }
        public override void Exit(EnemyBaseAI owner)
        {
            owner.AnimationStates.searchingDesired = false;
            owner.Agent.isStopped = false;
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
        public virtual void Exit(EnemyBaseAI owner) { throw new System.NotImplementedException(); }
    }
    public class EnemyStateMachine
    {
        public EnemyBaseState LastState { get; private set; }
        public EnemyBaseState CurrentState { get; private set; }

        public void SetState(EnemyBaseState newState, EnemyBaseAI owner)
        {
            if (CurrentState != null)
            {
                LastState = CurrentState;
                CurrentState?.Exit(owner);
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