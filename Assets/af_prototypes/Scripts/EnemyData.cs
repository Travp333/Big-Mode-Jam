using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public float DetectionFOV;
    public float TurnSpeed = 20;
    public float WalkSpeed = 3.5f;
    public float RunSpeed = 8;
    [Tooltip("How long this enemy will take to start chasing the player")]
    public float ReactionTime = 1;
    [Tooltip("How long this enemy will stay suspicious before losing interest")]
    public float SuspiciousTime = 6;
    [Tooltip("How long this enemy will be surprised")]
    public float SurprisedDuration = 0.5f;
    [Tooltip("How long this enemy will take to slip")]
    public float SlipDuration = 1;
    [Tooltip("How long this enemy will stay stunned by a trap")]
    public float StunDuration= 6;
    [Tooltip("How long this enemy will take to get up")]
    public float RiseDuration = 6;
    [Tooltip("How far away a slingshot projectile will alert this guy")]
    public float DistractionRadius = 25;
    [Tooltip("How far away a slingshot projectile will alert this guy without the hit point being visible")]
    public float DistractionImmediateDetectionRadius = 6;
}
