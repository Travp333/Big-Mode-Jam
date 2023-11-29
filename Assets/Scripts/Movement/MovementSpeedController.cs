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
	[Tooltip("speeds of the character, these states represent the speed when your character is jogging, walking")]
	public float baseSpeed = 10f, walkSpeed = 7f;
    // Start is called before the first frame update
    public float currentSpeed;
    [SerializeField]
    float factor = 1f;
    float lastFactor;
    [SerializeField]
	public bool slowed = false;
	public bool moving;
	
	public InputAction movementAction;
	public InputAction walkAction;
    //this will determine how severely speed is impacted

    // kindof a rough system atm, but currently you need to set factor to one of these set values then when you reset it you need to set it to its alternative
    //(.1, 10), (.2, 5), (.4, 2.5), (.5, 2), (.8, 1.25)
	// these are the only clean divisions i could think of so that we end up at the exact number we started with 
	// Awake is called when the script instance is being loaded.
	void Awake()
	{
		walkAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Walk");
		movementAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Move");
	}
    public void setFactor(float plug){
        factor = plug;
    }

    void Update() {
	    MovementState(factor);
	    if(walkAction.IsPressed()){
	    	slowed = true;
	    }
	    else{
	    	slowed = false;
	    }
    }

    void Start() {
        movement = GetComponent<Movement>();
    }
    void MovementState(float factor){
	    //change movement speeds universally
	    moving = movementAction.ReadValue<Vector2>().magnitude > 0;
		if (!slowed){
			currentSpeed = baseSpeed;
		}
        else if(slowed){
            currentSpeed = walkSpeed;
        }
        if(currentSpeed <= 0){
            currentSpeed = 0;
        }
        if (factor != lastFactor){
            walkSpeed *= factor;
            baseSpeed *= factor;
            currentSpeed *= factor;		
            lastFactor = factor;
        }
	}
    public Vector3 ProjectDirectionOnPlane (Vector3 direction, Vector3 normal) {
		return (direction - normal * Vector3.Dot(direction, normal)).normalized;
	}
}
