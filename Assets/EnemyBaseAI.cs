using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class EnemyBaseAI : MonoBehaviour
{
    #region AI
    public EnemyStateMachine AI = new EnemyStateMachine();
    public static EnemyIdleState IdleState = new EnemyIdleState();
    public static EnemySuspiciousState SuspiciousState = new EnemySuspiciousState();
    #endregion

    public EnemyData EnemyData;
    public Transform EyeTransform; // Where detection raycasts originate
    public LayerMask PlayerDetectionMask;
    public LayerMask EnvironmentDetectionMask;
    public TMPro.TMP_Text DebugCommentText;
    public TMPro.TMP_Text DebugStateText;

    [HideInInspector] public Vector3 PointOfInterest;

    Transform PlayerTransform;
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

    private void Awake()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();/*
        Debug.Log(AI);
        Debug.Log(IdleState);*/
        AI.SetState(IdleState, this);
    }
    private void Update()
    {
        AI.Update(this);
    }
    public bool PlayerVisible()
    {
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
                    Debug.Log("Name: " + _hit.collider.name + " Texture coord: " + _hit.textureCoord);
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
    public class EnemyIdleState : EnemyBaseState
    {
        public override string Name() { return "Idle"; }
        public override void Enter(EnemyBaseAI owner) {
        }
        public override void Update(EnemyBaseAI owner) {
            //owner.PlayerVisible();
            if (owner.PlayerVisible()) owner.AI.SetState(EnemyBaseAI.SuspiciousState, owner);
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
            owner.PointOfInterest = owner.PlayerPosition;
            _timer = 0;
        }
        public override void Update(EnemyBaseAI owner)
        {
            if (owner.PlayerVisible()) owner.PointOfInterest = owner.PlayerPosition;
            owner.FaceObjectOfInterest();

            // Transition
            if (_timer < owner.EnemyData.SuspiciousTime)
                _timer += Time.deltaTime;
            else owner.AI.SetState(IdleState,owner);
        }
        public override void Exit(EnemyBaseAI owner)
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