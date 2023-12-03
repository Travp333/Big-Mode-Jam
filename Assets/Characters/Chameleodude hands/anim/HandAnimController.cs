using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimController : MonoBehaviour
{
	int isArmedHash;
	int isFiringHash;
	int isMovingHash;
	int isRollingHash;
	[SerializeField]
	GameObject slingshot;
	[SerializeField]
	Animator bodyAnim;
	Animator animator;
	playerStates state;
	[SerializeField]
	public GameObject player = default;
	// Start is called before the first frame update
	public void ResetIsFiring(){
		state.firing = false;
		animator.SetBool(isFiringHash, false);
	}
    void Start()
	{
	    animator = GetComponent<Animator>();
		isArmedHash = Animator.StringToHash("isArmed");
		isFiringHash = Animator.StringToHash("isFiring");
		isMovingHash = Animator.StringToHash("isMoving");
		//isRollingHash = Animator.StringToHash("Rolling");
	    state = player.GetComponent<playerStates>();
    }

    // Update is called once per frame
    void Update()
	{
		animator.SetBool(isArmedHash, bodyAnim.GetBool("Armed"));
		//animator.SetBool(isRollingHash, bodyAnim.GetBool("Rolling"));
		animator.SetBool(isFiringHash, bodyAnim.GetBool("isFiring"));
	    bool isArmed = animator.GetBool(isArmedHash);
	    bool isFiring = animator.GetBool(isFiringHash);
		bool isRolling = bodyAnim.GetBool("Rolling");
		bool isMoving = animator.GetBool(isMovingHash);
	    bool armPressed = state.armed;
	    bool firePressed = state.firing;
		bool rollPressed = state.rolling;
		bool movePressed = state.moving;
	    
		if(!isMoving && movePressed && player.GetComponent<Movement>().OnGround && !isRolling){
			animator.SetBool(isMovingHash, true);
		}
		if(isMoving && (!movePressed || !player.GetComponent<Movement>().OnGround || isRolling)){
			animator.SetBool(isMovingHash, false);
		}
	    if(!isArmed && armPressed && !isRolling){
		    animator.SetBool(isArmedHash, true);
	    }
	    if(isArmed && (!armPressed || isRolling)){
		    animator.SetBool(isArmedHash, false);
			
	    }
	    if(!isFiring && firePressed && !isRolling){
		    animator.SetBool(isFiringHash, true);
	    }
    }
}
