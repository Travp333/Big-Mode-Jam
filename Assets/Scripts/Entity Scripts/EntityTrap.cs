using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTrap : EntityParent
{
    public bool trapIsTriggered;
    [Tooltip("Set to -1 for infinite")]
    public int numberOfUses;
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        trapIsTriggered = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void PickUpObject(Transform newParent)
    {
        base.PickUpObject(newParent);
        trapIsTriggered = false;
    }

    public override void PlaceObject(Transform newPos)
    {
        base.PlaceObject(newPos);
        trapIsTriggered = true;
    }

    public virtual bool ActivateTrap(GameObject triggeredTrap)
    {
        if (!trapIsTriggered || numberOfUses == 0)
            return false;
        numberOfUses--;

        return true;
    }
}
