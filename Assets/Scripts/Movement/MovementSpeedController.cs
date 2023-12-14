using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is meant to control the character's speed and allow other scripts to place boosts or limits upon it.
//Travis Parks
public class MovementSpeedController : MonoBehaviour
{
	Movement movement;
	PlayerStates state;
    [SerializeField, Range(0f, 100f)]
    [Tooltip("speeds of the character, these states represent the speed when your character is jogging, walking, rolling")]
	public float baseSpeed = 10f, walkSpeed = 5f, rollSpeed = 30f, crouchWalkSpeed = 3f;
    // Start is called before the first frame update
    public float currentSpeed;

	// Awake is called when the script instance is being loaded.



    void Update() {
	    MovementState();

    }

	void Start() {
		state = GetComponent<PlayerStates>();
        movement = GetComponent<Movement>();
    }
    void MovementState(){
	    //change movement speeds universally
	    state.moving = state.movementAction.ReadValue<Vector2>().magnitude > 0;
	    if(state.crouching){
	    	currentSpeed = crouchWalkSpeed;
	    	state.walking = false;
	    }
	    if(!state.crouching ){
	    	currentSpeed = baseSpeed;
	    }
	    if (!state.walking && !state.crouching && !state.holding){
			currentSpeed = baseSpeed;
		}
	    else if((state.walking && !state.crouching) || (state.holding && !state.crouching)){
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
