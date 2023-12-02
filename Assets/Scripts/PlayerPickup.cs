using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Transform pickupHoldingParent;
    public Transform placeObjectPosition;
    public GameObject pickupIndicator;

    bool isCarryingObject;
    List<GameObject> objectsInTriggerSpace;
    GameObject holdingObject;
    // Start is called before the first frame update
    void Start()
    {
        isCarryingObject = false;
        objectsInTriggerSpace = new List<GameObject>();
        pickupIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCarryingObject && Input.GetKeyDown(KeyCode.E) && objectsInTriggerSpace.Count > 0) // Pick up object
        {
            holdingObject = objectsInTriggerSpace[0];
            foreach(GameObject obj in objectsInTriggerSpace)
            {
                if (Vector3.Distance(transform.position, holdingObject.transform.position) > Vector3.Distance(transform.position, obj.transform.position))
                {
                    holdingObject = obj;
                }
            }
            holdingObject.GetComponent<EntityParent>().PickUpObject(pickupHoldingParent);
            isCarryingObject = true;
            pickupIndicator.SetActive(false);
        }
        else if (isCarryingObject && Input.GetKeyDown(KeyCode.E)) // Place object
        {
            isCarryingObject = false;
            holdingObject.GetComponent<EntityParent>().PlaceObject(placeObjectPosition);
            pickupIndicator.SetActive(true);
        }

        if (isCarryingObject)
            pickupIndicator.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EntityParent>() && !objectsInTriggerSpace.Contains(other.gameObject))
        {
            objectsInTriggerSpace.Add(other.gameObject);
            pickupIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInTriggerSpace.Contains(other.gameObject))
            objectsInTriggerSpace.Remove(other.gameObject);

        if (objectsInTriggerSpace.Count >= 1)
            pickupIndicator.SetActive(true);
        else
            pickupIndicator.SetActive(false);
    }
}
