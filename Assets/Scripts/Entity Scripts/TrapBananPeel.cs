using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBananPeel : EntityTrap
{
    public override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override bool ActivateTrap(GameObject triggeredTrap)
    {
        if (!base.ActivateTrap(triggeredTrap)) {
            Destroy(gameObject);
        }
        //Enemies should not be destroyed anymore :(
        //Destroy(triggeredTrap);

        return true;
    }
}
