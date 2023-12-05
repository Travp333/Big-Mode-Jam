using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPickup : MonoBehaviour
{
    public Transform pickupHoldingParent;
    public Transform placeObjectPosition;
    public GameObject pickupIndicator;
    public float throwForce;
    public Slider throwMeter;

    bool isCarryingObject;
    List<GameObject> objectsInTriggerSpace;
    GameObject holdingObject;
    float chargeThrow;
    bool cancelThrow;
    // Start is called before the first frame update
    void Start()
    {
        isCarryingObject = false;
        objectsInTriggerSpace = new List<GameObject>();
        pickupIndicator.SetActive(false);
        chargeThrow = 1f;
        cancelThrow = false;
        throwMeter.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCarryingObject && Input.GetKeyDown(KeyCode.R) && objectsInTriggerSpace.Count > 0) // Pick up object
        {
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
            holdingObject.GetComponent<EntityParent>().PickUpObject(pickupHoldingParent);
            isCarryingObject = true;
            pickupIndicator.SetActive(false);
            FindObjectOfType<playerStates>().holding = true;
        }
        else if (isCarryingObject && Input.GetKeyDown(KeyCode.R)) // Place object
        {
            isCarryingObject = false;
            holdingObject.GetComponent<EntityParent>().PlaceObject(placeObjectPosition);
            pickupIndicator.SetActive(true);
            FindObjectOfType<playerStates>().holding = false;
        }
        ThrowInput();

        if (isCarryingObject)
            pickupIndicator.SetActive(false);
    }

    private void ThrowInput()
    {
        if (isCarryingObject && Input.GetMouseButton(0) && !cancelThrow) // Charge throw
        {
            chargeThrow += Time.deltaTime * 4;
            cancelThrow = false;
            if (chargeThrow >= 5)
                chargeThrow = 5;
            throwMeter.value = chargeThrow;
        }
        if (isCarryingObject && Input.GetMouseButtonUp(0) && !cancelThrow) // Throw object
        {
            isCarryingObject = false;
            holdingObject.GetComponent<EntityParent>().ThrowObject(throwForce * chargeThrow);
            chargeThrow = 1f;
            cancelThrow = false;
            throwMeter.value = chargeThrow;
            FindObjectOfType<playerStates>().holding = false;
        }

        if (isCarryingObject && Input.GetMouseButtonDown(1)) // Cancel throw
        {
            chargeThrow = 1f;
            cancelThrow = true;
            throwMeter.value = chargeThrow;
        }
        if (cancelThrow && Input.GetMouseButtonUp(0)) // Let go of LMB after the throw was canceled
        {
            chargeThrow = 1f;
            cancelThrow = false;
        }
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
