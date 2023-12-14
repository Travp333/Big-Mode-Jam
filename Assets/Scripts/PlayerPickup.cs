using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerPickup : MonoBehaviour
{
	[SerializeField]
	GameObject holdingBigOne;
	[SerializeField]
	GameObject Player;
	public InputAction Pickup;
    public Transform pickupHoldingParent;
    public Transform placeObjectPosition;
    public GameObject pickupIndicator;
    public float throwForce;
    public Slider throwMeter;
	UpdateRotation rot;
	public bool isCarryingObject;
	public List<GameObject> objectsInTriggerSpace;
    GameObject holdingObject;
    float chargeThrow;
	bool cancelThrow;
	[SerializeField]
	float keyRadius = 5f;
	[SerializeField]
	LayerMask mask;
	Collider[] colliders;
    // Start is called before the first frame update
    void Start()
	{
		rot = Player.GetComponentInChildren<UpdateRotation>();
		Pickup = Player.GetComponent<PlayerInput>().currentActionMap.FindAction("Interact");
        isCarryingObject = false;
        objectsInTriggerSpace = new List<GameObject>();
        pickupIndicator.SetActive(false);
    }

	public void RemoveFromList(GameObject obj){
		if (objectsInTriggerSpace.Contains(obj))
			objectsInTriggerSpace.Remove(obj);
		pickupIndicator.SetActive(false);
	}

    // Update is called once per frame
    void Update()
	{
		if (isCarryingObject)
		{
			pickupIndicator.SetActive(false);
		}
		else if (!isCarryingObject && objectsInTriggerSpace.Count >= 1){
			pickupIndicator.SetActive(true);
		}
		else if(!isCarryingObject && objectsInTriggerSpace.Count <= 0){
			pickupIndicator.SetActive(false);
		}
    }
	public void PickUp(){
		//Debug.Log("PICKINGUP");
		objectsInTriggerSpace.RemoveAll(s => s == null);
		holdingObject = objectsInTriggerSpace[0];

		foreach(GameObject obj in objectsInTriggerSpace)
		{
			if (obj == null)
				continue;
			if (obj && Vector3.Distance(transform.position, holdingObject.transform.position) > Vector3.Distance(transform.position, obj.transform.position))
			{
				holdingObject = obj;
			}
		}
		if(holdingObject.GetComponent<EntityParent>() != null){

			if(holdingObject.gameObject.tag == "BigOne"){
				holdingObject.GetComponent<EntityParent>().PickUpBigOne(pickupHoldingParent);
				FindObjectOfType<playerStates>().holdingBigOne = true;
				holdingBigOne.SetActive(true);
			}
			else{
				holdingObject.GetComponent<EntityParent>().PickUpObject(pickupHoldingParent);
				FindObjectOfType<playerStates>().holding = true;
			}
		}
		else if(holdingObject.transform.parent.GetComponent<EntityParent>() != null){
			holdingObject.transform.parent.GetComponent<EntityParent>().PickUpObject(pickupHoldingParent);
			FindObjectOfType<playerStates>().holding = true;
		}
		isCarryingObject = true;

	}


	public void PutDown(){
		isCarryingObject = false;
		//RugTrap!
		if(holdingObject != null){
			if(holdingObject.GetComponent<EntityParent>()!= null){
				if(holdingObject.GetComponent<EntityTrap>()!= null){
					if(holdingObject.GetComponent<EntityTrap>().Type == EntityTrap.TrapType.Hole){
						holdingObject.GetComponent<EntityParent>().PlaceObject(placeObjectPosition);
						holdingObject.GetComponent<EntityParent>().canBePickedUp = false;
					}
					else if(!(holdingObject.GetComponent<EntityTrap>().Type == EntityTrap.TrapType.Hole)){
						//objectsInTriggerSpace.Add(holdingObject);
						holdingObject.GetComponent<EntityParent>().PlaceObject(placeObjectPosition);
					}
				}
				else{
					if(holdingObject.tag == "Key"){
						Debug.Log("Droped Key");
						colliders = Physics.OverlapSphere(this.transform.position, keyRadius);
						foreach(Collider hit in colliders){
							if(hit.gameObject.GetComponent<Lock>()){
								hit.gameObject.GetComponent<Lock>().Unlock();
								if(objectsInTriggerSpace.Contains(holdingObject)){
									objectsInTriggerSpace.Remove(holdingObject);
								}
								Destroy(holdingObject);
							}
						}
						if(holdingObject != null){
							holdingObject.GetComponent<EntityParent>().PlaceObject(placeObjectPosition);
						}
					}
					else if(holdingObject.gameObject.tag == "BigOne"){
						holdingObject.GetComponent<EntityParent>().PlaceObject(placeObjectPosition);
						FindObjectOfType<playerStates>().holdingBigOne = false;
						holdingBigOne.SetActive(false);
					}
					else{
						holdingObject.GetComponent<EntityParent>().PlaceObject(placeObjectPosition);
					}
				}
			}
			else{
				Debug.Log(holdingObject + " does not contain an entity trap component");
			}
			FindObjectOfType<PlayerStates>().holding = false;
			FindObjectOfType<PlayerStates>().face.setBase();
		}
	}

	public void ThrowInput()
    {
	    //Debug.Log("Throwing!");
	    isCarryingObject = false;


	    if(holdingObject.GetComponent<EntityParent>() != null){
		    if(holdingObject.gameObject.tag == "BigOne"){
			    holdingObject.GetComponent<EntityParent>().ThrowObject(throwForce, rot.transform.forward);
			    FindObjectOfType<playerStates>().holdingBigOne = false;
			    holdingBigOne.SetActive(false);
		    }
		    else{
			    holdingObject.GetComponent<EntityParent>().ThrowObject(throwForce, rot.transform.forward);

		    }
	    }

	    else if(holdingObject.transform.parent.gameObject.GetComponent<EntityParent>()!= null)
	    {
	    	holdingObject.transform.parent.gameObject.GetComponent<EntityParent>().ThrowObject(throwForce, rot.transform.forward);
	    }
	    FindObjectOfType<playerStates>().holding = false;
	    FindObjectOfType<playerStates>().face.setBase();
    }

	protected void OnTriggerStay(Collider other)
	{
		if(other.transform.parent != null){
			if(other.transform.parent.gameObject.GetComponent<EntityParent>()){
				if(other.transform.parent.gameObject.GetComponent<EntityParent>().canBePickedUp){
					if(!objectsInTriggerSpace.Contains(other.transform.parent.gameObject)){
						objectsInTriggerSpace.Add(other.transform.parent.gameObject);
					}
				}
			}
			if(other.transform.parent.gameObject.GetComponent<EntityParent>()){
				if(!other.transform.parent.gameObject.GetComponent<EntityParent>().canBePickedUp){
					if(objectsInTriggerSpace.Contains(other.transform.parent.gameObject)){
						objectsInTriggerSpace.Remove(other.transform.parent.gameObject);
					}
				}
			}
		}
		if(other.gameObject.GetComponent<EntityParent>())
		{
			//Debug.Log("AGHHHHHH");
			if(!objectsInTriggerSpace.Contains(other.gameObject)){
				if(other.gameObject.GetComponent<EntityParent>().canBePickedUp){
					objectsInTriggerSpace.Add(other.gameObject);
				}
			}
			if(objectsInTriggerSpace.Contains(other.gameObject)){
				if(!other.gameObject.GetComponent<EntityParent>().canBePickedUp){
					objectsInTriggerSpace.Remove(other.gameObject);
				}
			}

		}




	}

    private void OnTriggerEnter(Collider other)
	{
		if(other.transform.parent != null){
			if(other.transform.parent.gameObject.GetComponent<EntityParent>()){
				if(other.transform.parent.gameObject.GetComponent<EntityParent>().canBePickedUp){
					if(!objectsInTriggerSpace.Contains(other.transform.parent.gameObject)){
						objectsInTriggerSpace.Add(other.transform.parent.gameObject);
					}
				}
			}
		}
		if(other.gameObject.GetComponent<EntityParent>())
		{
			if(!objectsInTriggerSpace.Contains(other.gameObject)){
				if(other.gameObject.GetComponent<EntityParent>().canBePickedUp){
					objectsInTriggerSpace.Add(other.gameObject);
				}
			}

        }
    }

    private void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag != "Environment"){
			//Debug.Log(other.gameObject.name + " Just left range!");
	        if (objectsInTriggerSpace.Contains(other.gameObject))
		        objectsInTriggerSpace.Remove(other.gameObject);
			if(other.transform.parent != null){
				if (objectsInTriggerSpace.Contains(other.transform.parent.gameObject))
					objectsInTriggerSpace.Remove(other.transform.parent.gameObject);
			}
		}
    }
}
