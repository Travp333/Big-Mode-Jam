using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent of all entities, like collectables and traps
/// Parent: allow the player to pick this up and place it down
/// Children-Traps: when placed it activates a trigger and reaction for when an enemy steps on it
/// </summary>
public class EntityParent : MonoBehaviour
{
    public bool canBePickedUp;
    [SerializeField] float pickUpTime;

    bool isBeingPickedUp;
    float beingPickedUpTime;
    Vector3 initialPosition;
    Quaternion initialRotation;
    Rigidbody rb;
    BoxCollider boxCollider;
	float placeDownGravity = -50f;

    // Start is called before the first frame update
    // Switched to awake so references work even when not active
    public virtual void Awake()
    {
        beingPickedUpTime = 0;
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // -almost_friday: setting the trap's parent to the player alone should make this unnecessary 
    public virtual void Update()
    {
        if (isBeingPickedUp)
        {
            beingPickedUpTime += Time.deltaTime;
            transform.position = Vector3.Lerp(initialPosition, transform.parent.position, beingPickedUpTime / pickUpTime);
	        //transform.rotation = Quaternion.Lerp(initialRotation, transform.parent.rotation, beingPickedUpTime / pickUpTime);
            if (transform.position == transform.parent.position)
                isBeingPickedUp = false;
        }
    }

    public virtual void PickUpObject(Transform newParent)
	{
		
        transform.SetParent(newParent);
        //transform.position = newParent.position; // Surpy: Taking this out so it smooth transitions into picking to match closer to animation time
        isBeingPickedUp = true;
        beingPickedUpTime = 0;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        boxCollider.enabled = false;
        
		rb.isKinematic = true; // Prevents physics from affecting the trap when moved by a parent
		gameObject.layer = 11;
	}
    
	void LockCheck(){
		if(GetComponent<Rigidbody>().velocity.magnitude < 1f){
			//would be nice to have somr sort of snap to ground method here, if you get it leaning on something the velocity is zero and it will snap its rotation and lock floating in the ground
			gameObject.layer = 12;
			transform.rotation = new Quaternion (0,0,0,0);
			GetComponent<Rigidbody>().isKinematic = true;
		}
		else{
			Invoke("LockCheck", 1f);
		}
	}

    public virtual void PlaceObject(Transform newPos)
    {
        transform.SetParent(null);
        transform.position = newPos.position;
	    //transform.rotation = newPos.rotation;
        boxCollider.enabled = true;
        rb.isKinematic = false; // Reenables physics
        rb.velocity = new Vector3(0, placeDownGravity, 0); // Surpy: when placing it floats down, this gives it a little force going down, it feels better
	    isBeingPickedUp = false;
	    transform.rotation = new Quaternion (0,0,0,0);
	    
	    //attempts to lock the traps rigidbody in place
	    Invoke("LockCheck", 1f);
    }

	public virtual void ThrowObject(float force, Vector3 direction)
	{
        boxCollider.enabled = true;
        rb.isKinematic = false; // Reenables physics
	    rb.AddForce(direction * force);
        transform.SetParent(null);
		isBeingPickedUp = false;
		transform.rotation = new Quaternion (0,0,0,0);
		gameObject.layer = 12;
		Invoke("LockCheck", 1f);
    }
}
