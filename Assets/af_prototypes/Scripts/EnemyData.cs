using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public float DetectionFOV;
    public float TurnSpeed = 20;
    [Tooltip("How long this enemy will take to start chasing the player")]
    public float ReactionTime = 1;
    [Tooltip("How long this enemy will stay suspicious before losing interest")]
    public float SuspiciousTime = 6;
    [Tooltip("How long this enemy will stay stunned by a trap")]
    public float StunTime = 6;
    [Tooltip("How far away a slingshot projectile will alert this guy")]
    public float DistractionRadius = 25;
    [Tooltip("How far away a slingshot projectile will alert this guy without the hit point being visible")]
    public float DistractionImmediateDetectionRadius = 6;
}
