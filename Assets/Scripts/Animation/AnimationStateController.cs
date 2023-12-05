using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//I need to properly use hashes, im kinda half assing it here
// is on wall stays true after bumping into a ridigbody in water

public class AnimationStateController : MonoBehaviour
{
	[SerializeField]
	PlayerPickup pickup;
	[SerializeField]
	GameObject slingshot;
    public GameObject player = default;
	Movement sphere = default; 
	playerStates state;
	UpdateRotation rotation;
    Animator animator;
	int isRunningHash;
    int isOnSteepHash;
    int isJumpingHash;
    int onGroundHash;
	int isOnWallHash;
	int isWalkingHash;
	int isRollingHash;
	int isCrouchedHash;
	int isHoldingHash;
	int isThrowingHash;
	int isArmedHash;
	int isAimingHash;
	int isFallingHash;
	int isFiringHash;
    bool isOnGround;
    bool isOnWall;
	bool moveBlockGate;
    [HideInInspector]
    public bool isOnGroundADJ;
    bool isOnSteep;
    //bool isOnSteepADJ;
    bool JumpPressed;
    [SerializeField]
    [Tooltip("how long you need to be in the air before the 'onGround' bool triggers")]
    float OnGroundBuffer = .5f;
    [SerializeField]
    [Tooltip("how long isJumping stays true after pressing it ( maybe should be in movingsphere?)")]
    float JumpBuffer = .5f;
    bool JumpSwitch = true;
    float Groundstopwatch = 0;
	float Jumpstopwatch = 0;
    


	public void JumpAnimEvent(){
		sphere.JumpTrigger();
    }

	public void BlockMovement(){
		sphere.blockMovement();
	}
	public void UnBlockMovement(){
		sphere.unblockMovement();
	}
	public void ThrowHeldObject(){
		pickup.ThrowInput();
	}
	public void PutDownObject(){
		pickup.PutDown();
	}
	public void PickUpObject(){
		pickup.PickUp();
	}

	void Start() { 
		state = player.GetComponent<playerStates>();
		rotation = player.transform.GetChild(0).GetComponent<UpdateRotation>();
        sphere = player.GetComponent<Movement>();
        animator = GetComponent<Animator>();
	    isRunningHash = Animator.StringToHash("isRunning");
	    isWalkingHash = Animator.StringToHash("isWalking");
        isJumpingHash = Animator.StringToHash("isJumping");
        onGroundHash = Animator.StringToHash("OnGround");
        isOnWallHash = Animator.StringToHash("isOnWall");
		isFallingHash = Animator.StringToHash("isFalling");
		isRollingHash = Animator.StringToHash("Rolling");
		isCrouchedHash = Animator.StringToHash("Crouched");
		isHoldingHash = Animator.StringToHash("HoldingItem");
		isThrowingHash = Animator.StringToHash("Throwing");
		isArmedHash = Animator.StringToHash("Armed");
		isAimingHash = Animator.StringToHash("isAiming");
		isFiringHash = Animator.StringToHash("isFiring");

    }
    
	
    //this is meant to allow a sort of buffer, so bools stay true for a set amount of time
    void BoolAdjuster(){
        isOnGround = sphere.OnGround;
        isOnSteep = sphere.OnSteep;
        if (!isOnGround && !JumpPressed){
            Groundstopwatch += Time.deltaTime;
            if (Groundstopwatch >= OnGroundBuffer){
                isOnGroundADJ = false;
            }
        }
        if (!isOnGround && JumpPressed){
            isOnGroundADJ = false;
        }
        if(isOnGround){
            Groundstopwatch = 0;
            isOnGroundADJ = true;
        }
    }
	public void SetRollSpeed(){
		sphere.body.velocity = new Vector3 (this.transform.forward.x * sphere.speedController.rollSpeed, sphere.body.velocity.y, this.transform.forward.z * sphere.speedController.rollSpeed );
	}
	public void SetRollingSpeedFalse(){
		animator.SetBool("SettingRollingSpeed", false);
		state.rolling = false;
	}
	public void CancelHolding(){
		state.holding = false;
	}
	public void ResetArmedLayerWeight(){
		animator.SetLayerWeight(1, 0);
	}
	public void SpawnSlingShot(){
		if(state.aiming != true){
			slingshot.GetComponent<SkinnedMeshRenderer>().enabled = true;
			state.face.setAiming();
		}

	}
	public void HideSlingShot(){
		slingshot.GetComponent<SkinnedMeshRenderer>().enabled = false;
		if(state.crouching){
			state.face.setSneaking();
		}
		else{
			state.face.setBase();
		}
		EnterNullState();
	}
	public void EnterNullState(){
		animator.Play("NullState", 1);
	}
	public void ExitNullState(){
		animator.Play("Sling empty", 1);
	}
	public void ResetIsFiring(){
		state.firing = false;
		animator.SetBool(isFiringHash, false);
	}
	
    float jumpCount;
    float jumpCap = .2f;
	void Update() {
	
		
		if(animator.GetBool("AimCheck")){
			SpawnSlingShot();
			animator.SetBool("AimCheck", false);
		}
		if(animator.GetBool("MoveBlocked") == true){
			if(!moveBlockGate){
				rotation.SnapRotationToDirection();
				sphere.blockMovement();
				moveBlockGate = true;	
			}
		}
		else{
			if(moveBlockGate){
				sphere.unblockMovement();
				moveBlockGate = false;
			}
		}
		if(animator.GetBool("SettingRollingSpeed") == true){
			SetRollSpeed();
		}
        //Debug.Log(sphere.velocity.magnitude);
        BoolAdjuster();
	    bool JumpPressed = sphere.jumpAction.IsPressed();
        isOnGround = isOnGroundADJ;
        bool isFalling = animator.GetBool(isFallingHash);
        bool isOnWall = animator.GetBool(isOnWallHash);
	    bool isRunning = animator.GetBool(isRunningHash);
	    bool isWalking = animator.GetBool(isWalkingHash);
		bool isJumping = animator.GetBool(isJumpingHash);
		bool isCrouching = animator.GetBool(isCrouchedHash);
		bool isRolling = animator.GetBool(isRollingHash);
		bool isHolding = animator.GetBool(isHoldingHash);
		bool isThrowing = animator.GetBool(isThrowingHash);
		bool isArmed = animator.GetBool(isArmedHash);
		bool isFiring = animator.GetBool(isFiringHash);
		bool isAiming = animator.GetBool(isAimingHash);
	    bool movementPressed = state.moving;
		bool WalkPressed = state.walking;
		bool crouchPressed = state.crouching;
		bool rollPressed = state.rolling;
		bool holdPressed = state.holding;
		bool throwPressed = state.throwing;
		bool aimPressed = state.aiming;
		bool armpressed = state.armed;
		bool firePressed = state.firing;

        if (isOnGround){
            animator.SetBool(onGroundHash, true);
        }
        else if (!isOnGround){
	        animator.SetBool(onGroundHash, false);
	        animator.SetBool(isCrouchedHash, false);
	        state.UnCrouch();
	        state.holding = false;
	        if(!state.armed && !armpressed && !isArmed){
	        	//Debug.Log("Forcing base animation as you are in the air and not armed");
	        	state.face.setBase();
	        }
        }
        //This makes jump stay true a little longer after you press it, dependent on "JumpBuffer"
        if (JumpPressed){
            if(JumpSwitch){
                Jumpstopwatch = 0;
                animator.SetBool(isJumpingHash, true);
                JumpSwitch = false;
            }
            else{
                Jumpstopwatch += Time.deltaTime;
                    if(Jumpstopwatch >= JumpBuffer){
                        animator.SetBool(isJumpingHash, false);
                    }
            }   
        }
        //this activates when jump is not pressed, counts until jumpbuffer, then disables jump
        if(!JumpPressed){
            JumpSwitch = true;
            Jumpstopwatch += Time.deltaTime;
            if(Jumpstopwatch >= JumpBuffer){
                animator.SetBool(isJumpingHash, false);
            }
        }
        // if you are in the air, adding timer to give a little time before the falling animation plays
        if (!isOnGroundADJ && !isOnSteep){
            jumpCount += Time.deltaTime;
            if(jumpCount > jumpCap){
                animator.SetBool(isFallingHash, true);
	            animator.SetBool(isRunningHash, false);
	            animator.SetBool(isWalkingHash, false);
                jumpCount = 0f;
            }
        }
        else if(isOnGroundADJ || isOnSteep){
            jumpCount = 0f;
        }
        else if (!isOnGroundADJ && isOnSteep){
            animator.SetBool(isOnWallHash, true);
        }
        if (isOnGroundADJ){
            animator.SetBool(isFallingHash, false);
            animator.SetBool(isOnWallHash, false);
        }
        if (isOnSteep){
            animator.SetBool("isOnSteep", true);
        }
        if (!isOnSteep){
            animator.SetBool("isOnSteep", false);
            animator.SetBool(isOnWallHash, false);
        }
		if(!isFiring && firePressed && !isRolling){
			animator.SetBool(isFiringHash, true);
		}
		if(!isArmed && armpressed && !isRolling){
			animator.SetBool(isArmedHash, true);
			animator.SetLayerWeight(1, 1);
			ExitNullState();
		}
		if(isArmed && (!armpressed || isRolling)){
			animator.SetBool(isArmedHash, false);
			
		}
		if(!isHolding && holdPressed && isOnGroundADJ && !isCrouching && !isRolling){
			animator.SetBool(isHoldingHash, true);
			Debug.Log("Picking Up in animator");
		}
		if(isHolding && (!holdPressed || !isOnGroundADJ || isCrouching || isRolling)){
			animator.SetBool(isHoldingHash, false);
			Debug.Log("PuttingDown in animatior");
		}
		if(!isThrowing && throwPressed && isHolding && isOnGroundADJ){
			animator.SetBool(isThrowingHash, true);
		}
		if(isThrowing && (!throwPressed || !isOnGroundADJ)){
			animator.SetBool(isThrowingHash, false);
		}
		if(!isRolling && rollPressed && isOnGroundADJ && !isHolding && !aimPressed){
			animator.SetBool(isRollingHash, true);
			ResetArmedLayerWeight();
			state.face.setBase();
			HideSlingShot();
		}
		if(isRolling && (!rollPressed || !isOnGroundADJ || isHolding || aimPressed)){
			animator.SetBool(isRollingHash, false);
		}
		if(!isCrouching && crouchPressed && isOnGroundADJ && !isHolding){
			animator.SetBool(isCrouchedHash, true);
		}
		if(isCrouching && (!crouchPressed || !isOnGroundADJ || isRolling || isHolding)){
			animator.SetBool(isCrouchedHash, false);
		}
		if (!isWalking && (movementPressed && WalkPressed  && !isHolding)){
		    animator.SetBool(isWalkingHash, true);
	    }
		if (isWalking && (!movementPressed || !WalkPressed || isHolding )){
		    animator.SetBool(isWalkingHash, false);
	    }
		if (!isRunning && movementPressed && !WalkPressed && sphere.velocity.magnitude > 0 ){
            animator.SetBool(isRunningHash, true);
	    }
		if ((isRunning && !movementPressed) || WalkPressed ||sphere.velocity.magnitude <= 0.08f){
            animator.SetBool(isRunningHash, false);
	    }
    }

}
