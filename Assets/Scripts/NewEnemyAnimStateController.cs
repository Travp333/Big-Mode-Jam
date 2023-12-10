using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyAnimStateController : MonoBehaviour
{
    public Animator Anim;
    public int walkHash { get; private set; }
    public int idleHash { get; private set; }
    public int chaseHash { get; private set; }
    public int startledHash { get; private set; }
    public int stuckHash { get; private set; }
    public int slipHash { get; private set; }
    public int onBackHash { get; private set; }
    public int getUpHash { get; private set; }
    public int squashedHash { get; private set; }
    public int gettingHitHash { get; private set; }
    public int susHash { get; private set; }
    public int susWalkHash { get; private set; }
    public int searchingHash { get; private set; }

    private void Awake()
    {
        if (Anim)
        {
            walkHash = Animator.StringToHash("Walk");
            idleHash = Animator.StringToHash("Idle");
            chaseHash  = Animator.StringToHash("Chase");
            startledHash  = Animator.StringToHash("Startled");
            stuckHash  = Animator.StringToHash("Stuck");
            slipHash  = Animator.StringToHash("Slip");
            onBackHash  = Animator.StringToHash("On Ground Idle");
            getUpHash  = Animator.StringToHash("Get Up");
            squashedHash  = Animator.StringToHash("Squashed");
            gettingHitHash  = Animator.StringToHash("Getting Hit");
            susHash  = Animator.StringToHash("Sus Idle");
            susWalkHash  = Animator.StringToHash("Sus Walk");
            searchingHash  = Animator.StringToHash("Searching");
        }
    }
}
