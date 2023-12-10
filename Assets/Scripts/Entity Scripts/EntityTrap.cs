using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTrap : EntityParent
{
	[SerializeField]
	BoxCollider holeCollider;
	[SerializeField]
	GameObject launchVolume;
/*	[SerializeField]
	bool isHammer, isBananna, isHole, isGlove, isGlue;*/ //replaced with enum below
	[SerializeField] TrapType Type;
	public enum TrapType { Hammer, Banana, Hole, PunchGlove, Glue }
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
		//gameObject.layer = 11;
		/*		if(isHole){
					gameObject.transform.GetChild(1).gameObject.layer = 11;
				}*/
		if (Type == TrapType.Hole)
		{
			gameObject.transform.GetChild(1).gameObject.layer = 11;
		}
		base.PickUpObject(newParent);
        trapIsTriggered = false;
    }

    public override void PlaceObject(Transform newPos)
	{
		//gameObject.layer = 12;
		if(Type == TrapType.Hole)
		{
			//gameObject.transform.GetChild(1).gameObject.layer = 12;
			this.GetComponent<BoxCollider>().enabled = true;
			this.GetComponent<Animator>().SetBool("Armed", true);
			numberOfUses = 1;
			this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX |RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			canBePickedUp = false;
		}
        base.PlaceObject(newPos);
        trapIsTriggered = true;
    }
    public virtual bool ActivateTrap(GameObject triggeredTrap) // returns false if the trap should be destroyed
    {
        if (!trapIsTriggered || numberOfUses == 0)
            return false;
        numberOfUses--;

        return true;
    }
	void DisableVolumeTrigger(){
		launchVolume.SetActive(false);
	}
	void DespawnHole(){
		this.GetComponent<Animator>().SetBool("Triggered", true);
	}
	// OnTriggerEnter is called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other)
	{
		//Debug.Log(other.gameObject.name);
		if(other.gameObject.tag == "AI"){
			if(numberOfUses > 0){
				numberOfUses --;
				EnemyBaseAI baseAi = other.transform.parent.gameObject.GetComponent<EnemyBaseAI>();

				if (baseAi)
                {
					switch (Type)
					{
					case TrapType.Hammer:
							this.gameObject.GetComponent<Animator>().SetBool("Triggered", true);
							baseAi.AI.SetState(EnemyBaseAI.SmashedState,baseAi);
							break;
						case TrapType.Glue:
							baseAi.AI.SetState(EnemyBaseAI.gluedState, baseAi);
							break;
					case TrapType.Banana:
							this.gameObject.GetComponent<Animator>().SetBool("Triggered", true);
							baseAi.AI.SetState(EnemyBaseAI.SlipState, baseAi);
							break;
					case TrapType.PunchGlove:
							this.gameObject.GetComponent<Animator>().SetBool("Triggered", true);
							launchVolume.SetActive(true);
							Invoke("DisableVolumeTrigger", .5f);
						//baseAi.AI.SetState(EnemyBaseAI.RagdollState, baseAi);
							break;
					case TrapType.Hole:
							Invoke("DespawnHole", 3f);
							break;
					}
				}

			}
		}
	}
	// public void OnCollisionEnter(Collision col)
	// {
	    // EnemyBaseAI enemy;
	    // if (col.collider.tag == "Enemy")
	        // {
	        // if (col.transform.root.TryGetComponent(out enemy))
	            // {
	            //ActivateTrap(col.gameObject);
	            //enemy.AI.SetState(EnemyBaseAI.StunnedState, enemy);
	            //}
	    //  }
	    //}
}
