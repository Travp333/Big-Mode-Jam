using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EntityTrap : EntityParent
{
	//[SerializeField]
	//public bool isPickedUp;
	[SerializeField]
	BoxCollider holeCollider;
	[SerializeField]
	GameObject launchVolume;
/*	[SerializeField]
	bool isHammer, isBananna, isHole, isGlove, isGlue;*/ //replaced with enum below
	[SerializeField] public TrapType Type;
	public enum TrapType { Hammer, Banana, Hole, PunchGlove, Glue }
    public bool trapIsTriggered;
    [Tooltip("Set to -1 for infinite")]
    public int numberOfUses;
	
	[SerializeField]
	SFXManager sfx;
	
	AudioSource audioSource;

    public override void Awake()
    {
        base.Awake();
        trapIsTriggered = false;
		audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void PickUpObject(Transform newParent)
	{
		//gameObject.layer = 11;
		if (Type == TrapType.Hole)
		{
			gameObject.transform.GetChild(1).gameObject.layer = 11;
		}
		//isPickedUp = true;
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
		//isPickedUp = false;
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
					TriggerTrapSound(Type);
					switch (Type)
					{
					case TrapType.Hammer:
							this.gameObject.GetComponent<Animator>().SetBool("Triggered", true);
							baseAi.AI.SetState(EnemyBaseAI.SmashedState,baseAi);
							canBePickedUp = false;
							break;
						case TrapType.Glue:
							baseAi.AI.SetState(EnemyBaseAI.gluedState, baseAi);
							this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
							canBePickedUp = false;
							break;
					case TrapType.Banana:
							this.gameObject.GetComponent<Animator>().SetBool("Triggered", true);
							baseAi.AI.SetState(EnemyBaseAI.SlipState, baseAi);
							canBePickedUp = false;
							break;
					case TrapType.PunchGlove:
							this.gameObject.GetComponent<Animator>().SetBool("Triggered", true);
							launchVolume.SetActive(true);
							Invoke("DisableVolumeTrigger", .5f);
							canBePickedUp = false;
						//baseAi.AI.SetState(EnemyBaseAI.RagdollState, baseAi);
							break;
					case TrapType.Hole:
							canBePickedUp = false;
							baseAi.AI.SetState(EnemyBaseAI.FallInHoleState, baseAi);
							Destroy(baseAi.gameObject, 5);
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
	private void TriggerTrapSound(TrapType type) {
		switch (type)
		{
			case TrapType.Hammer:
				audioSource.PlayOneShot(sfx.metalSlam);
				break;
			case TrapType.Glue:
				audioSource.PlayOneShot(sfx.schlorp);
				break;
			case TrapType.Banana:
				audioSource.PlayOneShot(sfx.schlorp);
				break;
			case TrapType.PunchGlove:
				audioSource.PlayOneShot(sfx.spring);
				break;
			case TrapType.Hole:
				
				break;
		}
	}
	
}
