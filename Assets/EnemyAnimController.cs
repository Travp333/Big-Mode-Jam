using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimController : MonoBehaviour
{
	int isSuspiciousHash;
	int isWalkingHash;
	int isChashingHash;
	int isStuckHash;
	int isSmashedHash;
	int isSlippingHash;
	int isOnBackHash;
	int isTakingDamageHash;
	int isFallingHash;
	int isSearchingHash;
	int isNoticingHash;
	[SerializeField]
	[Tooltip("These values are what you would edit to get the enemy ai to have their animations sync to their actions. ie, set walkDesired true, they start walking, etc.")]
	public bool caughtInTrap, stuckDesired, susDesired, walkDesired, chaseDesired, smashDesired, slipDesired, onBackDesired, damageDesired, fallingDesired, searchingDesired, noticingDesired;
	[SerializeField]
	float slipTimer = 5f, stuckTimer = 10f;
	Animator anim;
	public void ResetOnBack(){
		anim.SetBool(isOnBackHash, false);
		onBackDesired = false;
	}
	public void ResetIsStuck(){
		anim.SetBool(isStuckHash, false);
		stuckDesired = false;
	}
    void Start()
    {
	    anim = GetComponent<Animator>();
	    isSuspiciousHash = Animator.StringToHash("isSuspicious");
	    isWalkingHash = Animator.StringToHash("isWalking");
	    isChashingHash = Animator.StringToHash("isChasing");
	    isStuckHash = Animator.StringToHash("isStuck");
	    isSmashedHash = Animator.StringToHash("isSmashed");
	    isSlippingHash = Animator.StringToHash("isSlipping");
	    isOnBackHash = Animator.StringToHash("isOnBack");
	    isTakingDamageHash = Animator.StringToHash("TakeDamage");
	    isFallingHash = Animator.StringToHash("isFalling");
	    isSearchingHash = Animator.StringToHash("isSearching");
	    isNoticingHash = Animator.StringToHash("isNoticing");
    }

    // Update is called once per frame
    void Update()
    {
	    bool isSuspicious = anim.GetBool(isSuspiciousHash);
	    bool isWalking = anim.GetBool(isWalkingHash);
	    bool isChasing = anim.GetBool(isChashingHash);
	    bool isSmashed = anim.GetBool(isSmashedHash);
	    bool isSlipping = anim.GetBool(isSlippingHash);
	    bool isOnBack = anim.GetBool(isOnBackHash);
	    bool isTakingDamage = anim.GetBool(isTakingDamageHash);
	    bool isFalling = anim.GetBool(isFallingHash);
	    bool isSearching = anim.GetBool(isSearchingHash);
	    bool isNoticing = anim.GetBool(isNoticingHash);
	    bool isStuck = anim.GetBool(isStuckHash);
	    caughtInTrap = isSlipping || isSmashed || isOnBack || isFalling || isSmashed || isStuck;
	    /*
	    susDesired;
	    walkDesired;
	    chaseDesired;
	    smashDesired;
	    slipDesired;
	    onBackDesired;
	    damageDesired;
	    fallingDesired;
	    searchingDesired;
	    noticingDesired;
	    */
	    //suspicious is just when the Ai thinks it sees you, but isnt sure yet
	    if(!isSuspicious && susDesired && !caughtInTrap){
	    	anim.SetBool(isSuspiciousHash, true);
	    }
	    if(isSuspicious && (!susDesired || caughtInTrap)){
	    	anim.SetBool(isSuspiciousHash, false);
	    }
	    //searching is for when the AI has already seen you, but then they lost you and are looking for you
	    if(!isSearching && searchingDesired && !caughtInTrap){
		    anim.SetBool(isSearchingHash, true);
	    } 
	    if(isSearching && ( !searchingDesired || caughtInTrap)){
		    anim.SetBool(isSearchingHash, false);
	    } 
	    //Noticing is for when AI 100 percent sees the character and is going to begin chase
	    if(!isNoticing && noticingDesired && !caughtInTrap){
		    anim.SetBool(isNoticingHash, true);
	    } 
	    if(isNoticing && ( !noticingDesired || caughtInTrap)){
		    anim.SetBool(isNoticingHash, false);
	    } 
	    if(!isWalking && walkDesired && !caughtInTrap && !isChasing){
	    	anim.SetBool(isWalkingHash, true);
	    }
	    if(isWalking && (!walkDesired || caughtInTrap  || isChasing)){
	    	anim.SetBool(isWalkingHash, false);
	    }
	    if(!isChasing && chaseDesired && !caughtInTrap && !isWalking){
	    	anim.SetBool(isChashingHash, true);
	    }
	    if(isChasing && (!chaseDesired || caughtInTrap  || isWalking)){
	    	anim.SetBool(isChashingHash, false);
	    }
	    // block movement during state
	    //doesnt need to be set false, once youre in you stay in
	    if(!isSmashed && smashDesired){
	    	anim.SetBool(isSmashedHash, true);
	    }	
	    // block movement during state
	    //same as above, once youre in you stay in. need to delete gameobject too, or at least disable collider
	    if(!isFalling && fallingDesired){
	    	anim.SetBool(isFallingHash, true);
	    }
	    // block movement during state
	    //isOnBack will be reset by a method so that we can decide how long they stay down after slipping
	    if(!isSlipping && slipDesired){
	    	anim.SetBool(isSlippingHash, true);
	    	anim.SetBool(isOnBackHash, true);
	    	Invoke("ResetOnBack", slipTimer);
	    }
	    if(isSlipping && (!slipDesired)){
	    	anim.SetBool(isSlippingHash, false);
	    }
	    // block movement during state
	    if(!isTakingDamage && damageDesired && !caughtInTrap){
	    	//Hoping this makes the takign damage more snappy and overwrites all other animations instantly
	    	anim.Play("Getting Hit", 0);
	    }
	    // block movement during state
	    // false will be called in a separate method so we can determine how long he is stuck in the trap
	    if(!isStuck && stuckDesired){
	    	anim.SetBool(isStuckHash, true);
	    	Invoke("ResetIsStuck", stuckTimer);
	    }

	
		
    }
}
