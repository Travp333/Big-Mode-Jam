using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyAnimStateController : MonoBehaviour
{
	[SerializeField]
	public GameObject HandVolume;
	//[SerializeField]
	GameObject player;
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
    public int grabHash { get; private set; }
    public int grabWalkHash { get; private set; }
    public int dumpHash { get; private set; }
	
	public void SpawnVolume(){
		HandVolume.GetComponent<GrabPlayer>().EnableVolume();
		//HandVolume.SetActive(true);
	}
	public void DespawnVolume(){
		HandVolume.GetComponent<GrabPlayer>().DisableVolume();
		//HandVolume.SetActive(false);
	}
	public void ReleasePlayer(){
		HandVolume.GetComponent<GrabPlayer>().ReleasePlayer(player);
	}
	
    private void Awake()
	{
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")){
			if(g.GetComponent<PlayerStates>()!= null){
				player = g;
			}
		}
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
            grabHash  = Animator.StringToHash("Pickup");
            grabWalkHash  = Animator.StringToHash("Pickup Walk");
            dumpHash  = Animator.StringToHash("pickup Dunk");
        }
    }
}
