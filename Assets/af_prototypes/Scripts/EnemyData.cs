using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public float DetectionFOV;
    public float TurnSpeed = 20;
    [Tooltip("How long this enemy will stay suspicious before losing interest")]
    public float SuspiciousTime = 5;
}
