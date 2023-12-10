using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerPickup : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
	{
		rot = Player.GetComponentInChildren<UpdateRotation>();
		Pickup = Player.GetComponent<PlayerInput>().currentActionMap.FindAction("Interact");
        isCarryingObject = false;
        objectsInTriggerSpace = new List<GameObject>();
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
		if(holdingObject.GetComponent<EntityTrap>() != null){
			holdingObject.GetComponent<EntityTrap>().PickUpObject(pickupHoldingParent);
		}
		else if(holdingObject.transform.parent.GetComponent<EntityTrap>() != null){
			holdingObject.transform.parent.GetComponent<EntityTrap>().PickUpObject(pickupHoldingParent);
		}
		isCarryingObject = true;
		FindObjectOfType<playerStates>().holding = true;
	}
	
    
	public void PutDown(){
		isCarryingObject = false;
		//RugTrap!
		if(holdingObject.GetComponent<EntityTrap>()!= null){
			if(holdingObject.GetComponent<EntityTrap>().Type == EntityTrap.TrapType.Hole){
				holdingObject.GetComponent<EntityTrap>().PlaceObject(placeObjectPosition);
				holdingObject.GetComponent<EntityTrap>().canBePickedUp = false;
			}
			else if(!(holdingObject.GetComponent<EntityTrap>().Type == EntityTrap.TrapType.Hole)){
				//objectsInTriggerSpace.Add(holdingObject);
				holdingObject.GetComponent<EntityTrap>().PlaceObject(placeObjectPosition);
			}
		}
		else{
			Debug.Log(holdingObject + " does not contain an entity trap component");
		}
		FindObjectOfType<playerStates>().holding = false;
		FindObjectOfType<playerStates>().face.setBase();
	}

	public void ThrowInput()
    {
	    Debug.Log("Throwing!");
	    isCarryingObject = false;
	    
	    
	    if(holdingObject.GetComponent<EntityParent>() != null){
		    holdingObject.GetComponent<EntityParent>().ThrowObject(throwForce, rot.transform.forward);
	    }
	    else if(holdingObject.transform.parent.gameObject.GetComponent<EntityParent>()!= null)
	    {
	    	holdingObject.transform.parent.gameObject.GetComponent<EntityParent>().ThrowObject(throwForce, rot.transform.forward);
	    }
	    FindObjectOfType<playerStates>().holding = false;
    }

	protected void OnTriggerStay(Collider other)
	{
		if(other.transform.parent != null){
			if(other.transform.parent.gameObject.GetComponent<EntityTrap>()){
				if(other.transform.parent.gameObject.GetComponent<EntityTrap>().canBePickedUp){
					if(!objectsInTriggerSpace.Contains(other.transform.parent.gameObject)){
						objectsInTriggerSpace.Add(other.transform.parent.gameObject);
					}
				}
			}
			if(other.transform.parent.gameObject.GetComponent<EntityTrap>()){
				if(!other.transform.parent.gameObject.GetComponent<EntityTrap>().canBePickedUp){
					if(objectsInTriggerSpace.Contains(other.transform.parent.gameObject)){
						objectsInTriggerSpace.Remove(other.transform.parent.gameObject);
					}
				}
			}
		}
		if(other.gameObject.GetComponent<EntityTrap>())
		{
			if(!objectsInTriggerSpace.Contains(other.gameObject)){
				if(other.gameObject.GetComponent<EntityTrap>().canBePickedUp){
					objectsInTriggerSpace.Add(other.gameObject);
				}
			}
			if(objectsInTriggerSpace.Contains(other.gameObject)){
				if(!other.gameObject.GetComponent<EntityTrap>().canBePickedUp){
					objectsInTriggerSpace.Remove(other.gameObject);
				}
			}

		}



		
	}

    private void OnTriggerEnter(Collider other)
	{
		if(other.transform.parent != null){
			if(other.transform.parent.gameObject.GetComponent<EntityTrap>()){
				if(other.transform.parent.gameObject.GetComponent<EntityTrap>().canBePickedUp){
					if(!objectsInTriggerSpace.Contains(other.transform.parent.gameObject)){
						objectsInTriggerSpace.Add(other.transform.parent.gameObject);
					}
				}
			}
		}
		if(other.gameObject.GetComponent<EntityTrap>())
		{
			if(!objectsInTriggerSpace.Contains(other.gameObject)){
				if(other.gameObject.GetComponent<EntityTrap>().canBePickedUp){
					objectsInTriggerSpace.Add(other.gameObject);
				}
			}

        }
    }

    private void OnTriggerExit(Collider other)
	{
        if (objectsInTriggerSpace.Contains(other.gameObject))
	        objectsInTriggerSpace.Remove(other.gameObject);
		if(other.transform.parent != null){
			if (objectsInTriggerSpace.Contains(other.transform.parent.gameObject))
				objectsInTriggerSpace.Remove(other.transform.parent.gameObject);
		}
    }
}
