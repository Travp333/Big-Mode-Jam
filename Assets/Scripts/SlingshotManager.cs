using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



public class SlingshotManager : MonoBehaviour
{
    public PlayerStates playerStates; 
    public GameObject ProjectilePrefab;
    public Transform Projectile3rdPersonOrigin;
    public Transform Projectile1stPersonOrigin;

    [SerializeField]
    SFXManager sFX;
    [SerializeField]
    AudioSource PlayerAudio;
    public void Shoot()
    {
        if (sFX != null && PlayerAudio != null) { PlayerAudio.PlayOneShot(sFX.spit); }
        Transform origin = playerStates.FPSorTPS ? Projectile3rdPersonOrigin : Projectile1stPersonOrigin;
        Instantiate(ProjectilePrefab, origin.position, Quaternion.LookRotation(origin.forward));
    }
}
