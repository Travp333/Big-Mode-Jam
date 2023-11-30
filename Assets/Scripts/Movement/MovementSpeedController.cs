using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//this script is meant to control the character's speed and allow other scripts to place boosts or limits upon it.
//Travis Parks
public class MovementSpeedController : MonoBehaviour
{
    Movement movement;
    [SerializeField, Range(0f, 100f)]
    [Tooltip("speeds of the character, these states represent the speed when your character is jogging, walking, rolling")]
	public float baseSpeed = 10f, walkSpeed = 5f, rollSpeed = 30f, crouchWalkSpeed = 3f;
    // Start is called before the first frame update
    public float currentSpeed;
    [SerializeField]
	public bool walking = false;
	public bool crouching;
	public bool moving;
	public bool rolling;
	public bool holding;
	public bool throwing;
	public InputAction movementAction;
	public InputAction walkAction;
	public InputAction crouchAction;
	public InputAction interactAction;
	public InputAction attackAction;
	float lastPressTime;
	[SerializeField]
	
	float doublePressTime;
	// Awake is called when the script instance is being loaded.
	void Awake()
	{
		interactAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Interact");
		attackAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Attack");
		walkAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Walk");
		movementAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Move");
		crouchAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Crouch");
	}
    
	void ResetRolling(){
		rolling = false;
		crouching = true;
	}
	void ResetThrowing(){
		throwing = false;
	}

	IEnumerator StartCrouch()
	{
		yield return new WaitForSeconds(doublePressTime);
		Debug.Log("Croumch" + (Time.time - lastPressTime));
		rolling = false;
		if(crouching){
			crouching = false;
		}
		else{
			crouching = true;
		}
	}

    void Update() {
	    MovementState();
	    if(crouchAction.WasPressedThisFrame() && movement.OnGround && !holding){
	    	if((Time.time - lastPressTime <= doublePressTime)&& moving){
	    		StopCoroutine("StartCrouch");
	    		Debug.Log("ROLL!");
	    		rolling = true;
	    		Invoke("ResetRolling", doublePressTime);
	    		crouching = true;
	    	}
	    	else{
	    		lastPressTime = Time.time;
	    		StartCoroutine("StartCrouch");
	    	}
	    }
	    if(walkAction.IsPressed()){
	    	walking = true;
	    }
	    else{
	    	walking = false;
	    }
	    
	    if(holding && attackAction.WasPerformedThisFrame()){
	    	throwing = true;
	    	Invoke("ResetThrowing", 1f);
	    	holding = false;
	    }
	    else if(holding && interactAction.WasPerformedThisFrame()){
	    	holding = false;
	    }
    }

    void Start() {
        movement = GetComponent<Movement>();
    }
    void MovementState(){
	    //change movement speeds universally
	    moving = movementAction.ReadValue<Vector2>().magnitude > 0;
	    if(crouching){
	    	currentSpeed = crouchWalkSpeed;
	    	walking = false;
	    }
	    if(!crouching ){
	    	currentSpeed = baseSpeed;
	    }
	    if (!walking && !crouching && !holding){
			currentSpeed = baseSpeed;
		}
	    else if((walking && !crouching) || (holding && !crouching)){
            currentSpeed = walkSpeed;
        }
        if(currentSpeed <= 0){
            currentSpeed = 0;
        }
	}
    public Vector3 ProjectDirectionOnPlane (Vector3 direction, Vector3 normal) {
		return (direction - normal * Vector3.Dot(direction, normal)).normalized;
	}
}
