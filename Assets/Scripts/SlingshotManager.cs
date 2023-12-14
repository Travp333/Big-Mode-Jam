using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotManager : MonoBehaviour
{
    public PlayerStates playerStates; 
    public GameObject ProjectilePrefab;
    public Transform Projectile3rdPersonOrigin;
    public Transform Projectile1stPersonOrigin;

    public void Shoot()
    {
        Transform origin = playerStates.FPSorTPS ? Projectile3rdPersonOrigin : Projectile1stPersonOrigin;
        Instantiate(ProjectilePrefab, origin.position, Quaternion.LookRotation(origin.forward));
    }
}
