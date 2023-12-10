using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollCloner : MonoBehaviour
{
	[SerializeField]
	EnemyAnimController CloneFrom;
	EnemyAnimController CloneTo;
	// Update is called once per frame
	protected void Start()
	{
		CloneTo = this.GetComponent<EnemyAnimController>();
	}
    void Update()
    {
	    CloneTo.caughtInTrap = CloneFrom.caughtInTrap;
	    CloneTo.stuckDesired = CloneFrom.stuckDesired;
	    CloneTo.susDesired = CloneFrom.susDesired;
	    CloneTo.walkDesired = CloneFrom.walkDesired;
	    CloneTo.chaseDesired = CloneFrom.chaseDesired;
	    CloneTo.smashDesired = CloneFrom.smashDesired;
	    CloneTo.slipDesired = CloneFrom.slipDesired;
	    CloneTo.onBackDesired = CloneFrom.onBackDesired;
	    CloneTo.damageDesired = CloneFrom.damageDesired;
	    CloneTo.fallingDesired = CloneFrom.fallingDesired;
	    CloneTo.searchingDesired = CloneFrom.searchingDesired;
	    CloneTo.noticingDesired = CloneFrom.noticingDesired;
    }
}
