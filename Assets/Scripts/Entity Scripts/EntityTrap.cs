using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTrap : EntityParent
{
	[SerializeField]
	BoxCollider holeCollider;
	[SerializeField]
	GameObject launchVolume;
	[SerializeField]
	public bool isHammer, isBananna, isHole, isGlove, isGlue;
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
		if(isHole){
			gameObject.transform.GetChild(1).gameObject.layer = 11;
		}
        base.PickUpObject(newParent);
        trapIsTriggered = false;
    }

    public override void PlaceObject(Transform newPos)
	{
		//gameObject.layer = 12;
		if(isHole){
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
				numberOfUses = numberOfUses - 1;
				
				if(isHammer){
					this.GetComponent<Animator>().SetBool("Triggered", true);
					//state dont exist yet
					//EnemyBaseAI baseAi = other.transform.parent.gameObject.GetComponent<EnemyBaseAI>();
					//baseAi.AI.SetState(EnemyBaseAI.SmashedState, baseAi);
					other.transform.parent.gameObject.GetComponent<Animator>().SetBool("isSmashed", true);
				}
				else if (isGlue){
					this.GetComponent<Rigidbody>().isKinematic = true;
					//state dont exist yet
					//EnemyBaseAI baseAi = other.transform.parent.gameObject.GetComponent<EnemyBaseAI>();
					//baseAi.AI.SetState(EnemyBaseAI.StuckState, baseAi);
				}
				else if(isBananna){
					this.GetComponent<Animator>().SetBool("Triggered", true);
					EnemyBaseAI baseAi = other.transform.parent.gameObject.GetComponent<EnemyBaseAI>();
					baseAi.AI.SetState(EnemyBaseAI.SlipState, baseAi);
					//other.transform.parent.gameObject.GetComponent<Animator>().SetBool("IsSlipping", true);
				}
				else if(isHole){
					Invoke("DespawnHole", 3f);
					//state dont exist yet
					//EnemyBaseAI baseAi = other.transform.parent.gameObject.GetComponent<EnemyBaseAI>();
					//baseAi.AI.SetState(EnemyBaseAI.FallingState, baseAi);
					//other.transform.parent.gameObject.GetComponent<Animator>().SetBool("isFalling", true);
				}
				else if(isGlove){
					this.GetComponent<Animator>().SetBool("Triggered", true);
					launchVolume.SetActive(true);
					//just get fuckin ragdolled kid lmao
					Invoke("DisableVolumeTrigger", .5f);
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
