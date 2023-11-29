using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//updates what direction the player model is facing
public class UpdateRotation : MonoBehaviour

{
	[SerializeField]
	GameObject point;
	[SerializeField]
	float rotationSpeed = 720f;
	[SerializeField]
	float airRotationSpeed = 240f;
	[SerializeField]
    GameObject player = default;
    Movement sphere; 
    Rigidbody body;
    Vector3 DummyGrav;
    bool Gate = true;
    bool gravSwap;
    // Start is called before the first frame update
    void Start()
    {
		transform.rotation = Quaternion.LookRotation( transform.forward , CustomGravity.GetUpAxis(transform.position));
        sphere = player.GetComponent<Movement>();
        body = player.GetComponent<Rigidbody>();
    }

    void Update() {
        //transform.Rotate(0,5,0);
        UpdateSpins();
    }

	void gravFlip(){
			//Debug.Log("rotating...");
            Quaternion toRotation = Quaternion.LookRotation( transform.forward , CustomGravity.GetUpAxis(this.transform.position) );
			this.transform.rotation = Quaternion.RotateTowards (transform.rotation, toRotation, (rotationSpeed) * Time.deltaTime);
			Gate = true;
	}
    void UpdateSpins()
    {
		Vector3 player2Pointer = sphere.ProjectDirectionOnPlane(point.transform.position - transform.parent.gameObject.transform.position, CustomGravity.GetUpAxis(transform.position));
		Debug.DrawRay(this.transform.position, player2Pointer, Color.gray, 3f);
        Vector3 gravity = CustomGravity.GetUpAxis(this.transform.position);
		//the "gravSwap" logic is works in the sence that the bool represents if gravity is changing or not but this not creates an issue with priority, if it is before the 
		// connected body check, it overwrites it and forces its orientation    , which causes issues when standing on moving platforms. however, if it is after the connected body check,
		//standing on rigid bodies causes you to not update your rotation anymore. this comes back to the root issue of needing to be able to have a way to change the up vector without changing the forward vector. 
		// this is only an issue for constantly changing gravity, however. 

		if(DummyGrav == gravity){
			if(Gate){
				gravSwap = false;
			}
		}
		else{
            //Debug.Log("Gravity is changing!");
			gravSwap = true;
			DummyGrav = gravity;
			Gate = false;
		}
		// is gravity changing? 
		if (gravSwap){
				Invoke("gravFlip", .6f);
		}
		//else if (sphere.isAiming){
			//Debug.Log("Aiming");
		//	Quaternion toRotation = Quaternion.LookRotation(sphere.forwardAxis, gravity);
		//	transform.rotation = Quaternion.RotateTowards (transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
		//}
		//SHOULD MAKE IT SO THAT ROTATING IN AIR IS SLOWER
		else if (sphere.velocity.magnitude > .2f && (sphere.playerInput != Vector3.zero)){
			if(!sphere.OnGround){
				Quaternion toRotation = Quaternion.LookRotation(ProjectDirectionOnPlane(player2Pointer, gravity), gravity);
				transform.rotation = Quaternion.RotateTowards (transform.rotation, toRotation, airRotationSpeed * Time.deltaTime);
			}
			else{
				Quaternion toRotation = Quaternion.LookRotation(ProjectDirectionOnPlane(player2Pointer, gravity), gravity);
				transform.rotation = Quaternion.RotateTowards (transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
			}
			//Debug.Log("Moving");

		}
	}
    Vector3 ProjectDirectionOnPlane (Vector3 direction, Vector3 normal) {
		return (direction - normal * Vector3.Dot(direction, normal)).normalized;
	}
}
