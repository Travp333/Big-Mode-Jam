using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBananPeel : EntityTrap
{
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override bool ActivateTrap(GameObject triggeredTrap)
    {
        if (!base.ActivateTrap(triggeredTrap))
            return false;
        
        Destroy(triggeredTrap);

        return true;
    }
}
