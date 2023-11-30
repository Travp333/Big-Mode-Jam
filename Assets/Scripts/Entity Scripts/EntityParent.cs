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
    [SerializeField]
    float pickUpTime;

    bool isBeingPickedUp;
    float beingPickedUpTime;
    Vector3 initialPosition;
    Quaternion initialRotation;
    Rigidbody rb;
    BoxCollider boxCollider;

    // Start is called before the first frame update
    public virtual void Start()
    {
        beingPickedUpTime = 0;
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (isBeingPickedUp)
        {
            beingPickedUpTime += Time.deltaTime;
            transform.position = Vector3.Lerp(initialPosition, transform.parent.position, beingPickedUpTime / pickUpTime);
            transform.rotation = Quaternion.Lerp(initialRotation, transform.parent.rotation, beingPickedUpTime / pickUpTime);
            if (transform.position == transform.parent.position)
                isBeingPickedUp = false;
        }
    }

    public virtual void PickUpObject(Transform newParent)
    {
        transform.SetParent(newParent);
        isBeingPickedUp = true;
        beingPickedUpTime = 0;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        boxCollider.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
    }

    public virtual void PlaceObject(Transform newPos)
    {
        transform.SetParent(null);
        transform.position = newPos.position;
        transform.rotation = newPos.rotation;
        boxCollider.enabled = true;
        rb.constraints = ~RigidbodyConstraints.FreezePositionY;
        rb.velocity = new Vector3(0, -19.62f, 0);
        isBeingPickedUp = false;
    }
}
