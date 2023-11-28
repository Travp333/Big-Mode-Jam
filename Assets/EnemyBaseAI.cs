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
    public TMPro.TMP_Text DebugText;

    private Transform PlayerTransform;
    private Vector3 PlayerPosition {
        get
        {
            return PlayerTransform.position + Vector3.up; // Offsets transform position
        }
    }

    Ray _playerRay;
    RaycastHit _hit;

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
        //if (Vector3.Angle(EyeTransform.forward, _playerRay.direction) > EnemyData.DetectionFOV) return false;

        Debug.DrawLine(EyeTransform.position, PlayerPosition, Color.green);
        if (Physics.Linecast(EyeTransform.position, PlayerPosition, out _hit, PlayerDetectionMask) && _hit.collider.tag == "Player")
        {
            if (DebugText) DebugText.text = "Player Visible";
            Debug.DrawRay(EyeTransform.position, _playerRay.direction, Color.red);

            if (Physics.Raycast(EyeTransform.position, _playerRay.direction, out _hit, 100, EnvironmentDetectionMask))
            {
                if (_hit.collider.tag == "Environment")
                {
                    Debug.Log("Name: " + _hit.collider.name + " Texture coord: " + _hit.textureCoord);
                    //Debug.Log("Wall material: " + _hit.collider.GetComponent<Renderer>().sharedMaterial.name + " Player Material: " + PlayerColorChangeBehavior.Instance.CurrentMaterial.name);
                    if ( !string.Equals(_hit.collider.GetComponent<Renderer>().sharedMaterial.name, PlayerColorChangeBehavior.Instance.CurrentMaterial.name) ) 
                    {
                        if (DebugText) DebugText.text = "Materials DO NOT match";
                        return true;
                    }
                    else
                    {
                        if (DebugText) DebugText.text = "Materials match!";
                        return false;
                    }
                }
            }
        } else
        {
            if (DebugText) DebugText.text = "Player Not Visible";
        }
            
        return false;
    }

    public class EnemyIdleState : EnemyBaseState
    {
        public override string Name() { return "Idle"; }
        public override void Enter(EnemyBaseAI owner) {
        }
        public override void Update(EnemyBaseAI owner) {
            owner.PlayerVisible();
            //if (owner.PlayerVisible) owner.AI.SetState(EnemyBaseAI.SuspiciousState, owner);
        }
        public override void Exit(EnemyBaseAI owner) {
        }
    }
    public class EnemySuspiciousState : EnemyBaseState
    {
        public override string Name() { return "Idle"; }
        public override void Enter(EnemyBaseAI owner)
        {
        }
        public override void Update(EnemyBaseAI owner)
        {
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
            Debug.Log("Entering " + CurrentState.Name() + " state");
            CurrentState.Enter(owner);
        }

        public void Update(EnemyBaseAI owner)
        {
            CurrentState.Update(owner);
        }
    }
}