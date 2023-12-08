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
    bool isCarryingObject;
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
		//chargeThrow = 1f;
		//cancelThrow = false;
		//throwMeter.value = 1;
    }

    // Update is called once per frame
    void Update()
	{
		//again i am calling this on an animation now instead
		// if (!isCarryingObject && Pickup.WasPressedThisFrame() && objectsInTriggerSpace.Count > 0) // Pick up object
		    // {
		    //    PickUp();
		    //}
	    // else if (isCarryingObject && Pickup.WasPressedThisFrame()) // Place object
		    // {
		    //PutDown();
		    //}
	    //ThrowInput();
        if (isCarryingObject)
            pickupIndicator.SetActive(false);
    }
	public void PickUp(){
		Debug.Log("PICKINGUP");
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
			holdingObject.GetComponent<EntityParent>().PickUpObject(pickupHoldingParent);
		}
		else if(holdingObject.transform.parent.GetComponent<EntityParent>() != null){
			holdingObject.transform.parent.GetComponent<EntityParent>().PickUpObject(pickupHoldingParent);
		}
		isCarryingObject = true;
		pickupIndicator.SetActive(false);
		FindObjectOfType<playerStates>().holding = true;
	}
    
	public void PutDown(){
		//Debug.Log("PUTTINGDOWN");
		isCarryingObject = false;
		//RugTrap!
		if(holdingObject.transform.parent.GetComponent<EntityParent>()){
			holdingObject.transform.parent.GetComponent<EntityParent>().PlaceObject(placeObjectPosition);
			pickupIndicator.SetActive(false);
			objectsInTriggerSpace.Clear();
			holdingObject.transform.parent.GetComponent<EntityParent>().canBePickedUp = false;
		}
		else if(holdingObject.GetComponent<EntityParent>() != null){
			holdingObject.GetComponent<EntityParent>().PlaceObject(placeObjectPosition);
			pickupIndicator.SetActive(true);
		}
		FindObjectOfType<playerStates>().holding = false;
		FindObjectOfType<playerStates>().face.setBase();
	}
    
	//Kinda gutted this, but basically I am gong to call this on the animation playing instead of the button press so it syncs better, 
	//also removed charging for now
	//travis
	public void ThrowInput()
    {
	    //if (isCarryingObject && Input.GetMouseButton(0) && !cancelThrow) // Charge throw
	    // {
	    //     chargeThrow += Time.deltaTime * 4;
	    //     cancelThrow = false;
	    //     if (chargeThrow >= 5)
	    //         chargeThrow = 5;
	    //      throwMeter.value = chargeThrow;
	    //  }
	    // if (isCarryingObject && Input.GetMouseButtonUp(0) && !cancelThrow) // Throw object
	    //  {
	    Debug.Log("Throwing!");
	    isCarryingObject = false;
	    
	    
	    if(holdingObject.GetComponent<EntityParent>() != null){
		    holdingObject.GetComponent<EntityParent>().ThrowObject(throwForce, rot.transform.forward);
	    }
	    else if(holdingObject.transform.parent.gameObject.GetComponent<EntityParent>()!= null)
	    {
	    	holdingObject.transform.parent.gameObject.GetComponent<EntityParent>().ThrowObject(throwForce, rot.transform.forward);
	    }
	    //chargeThrow = 1f;
	    //cancelThrow = false;
	    //throwMeter.value = chargeThrow;
	    FindObjectOfType<playerStates>().holding = false;
	    //  }

	    // if (isCarryingObject && Input.GetMouseButtonDown(1)) // Cancel throw
	    //  {
	    //      chargeThrow = 1f;
	    //     cancelThrow = true;
	    //      throwMeter.value = chargeThrow;
	    //  }
	    //  if (cancelThrow && Input.GetMouseButtonUp(0)) // Let go of LMB after the throw was canceled
	    //  {
     //       chargeThrow = 1f;
     //       cancelThrow = false;
	    //   }
    }

    private void OnTriggerEnter(Collider other)
	{
		
		//Duplicated this so that the rug trap can work, the collider had to be on a child object
		if(other.transform.parent != null){
			if(other.transform.parent.gameObject.GetComponent<EntityParent>()){
				if(other.transform.parent.gameObject.GetComponent<EntityParent>().canBePickedUp){
					if(!objectsInTriggerSpace.Contains(other.transform.parent.gameObject)){
						objectsInTriggerSpace.Add(other.transform.parent.gameObject);
						pickupIndicator.SetActive(true);
					}
				}
			}
		}
		if(other.gameObject.GetComponent<EntityParent>())
		{
			if(!objectsInTriggerSpace.Contains(other.gameObject)){
				if(other.gameObject.GetComponent<EntityParent>().canBePickedUp){
					objectsInTriggerSpace.Add(other.gameObject);
					pickupIndicator.SetActive(true);
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

        if (objectsInTriggerSpace.Count >= 1)
            pickupIndicator.SetActive(true);
        else
            pickupIndicator.SetActive(false);
    }
}
