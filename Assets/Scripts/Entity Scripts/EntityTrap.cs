using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTrap : EntityParent
{
	[SerializeField]
	GameObject launchVolume;
	[SerializeField]
	bool isHammer, isBananna, isHole, isGlove, isGlue;
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
		gameObject.layer = 11;
        base.PickUpObject(newParent);
        trapIsTriggered = false;
    }

    public override void PlaceObject(Transform newPos)
	{
		gameObject.layer = 12;
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
	// OnTriggerEnter is called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other)
	{
		//Debug.Log(other.gameObject.name);
		if(other.gameObject.tag == "AI"){
			if(numberOfUses > 0){
				numberOfUses = numberOfUses - 1;
				this.GetComponent<Animator>().SetBool("Triggered", true);
				if(isHammer){
					//state dont exist yet
					//EnemyBaseAI baseAi = other.transform.parent.gameObject.GetComponent<EnemyBaseAI>();
					//baseAi.AI.SetState(EnemyBaseAI.SmashedState, baseAi);
					other.transform.parent.gameObject.GetComponent<Animator>().SetBool("isSmashed", true);
				}
				else if (isGlue){
					//state dont exist yet
					//EnemyBaseAI baseAi = other.transform.parent.gameObject.GetComponent<EnemyBaseAI>();
					//baseAi.AI.SetState(EnemyBaseAI.StuckState, baseAi);
				}
				else if(isBananna){
					EnemyBaseAI baseAi = other.transform.parent.gameObject.GetComponent<EnemyBaseAI>();
					baseAi.AI.SetState(EnemyBaseAI.SlipState, baseAi);
					//other.transform.parent.gameObject.GetComponent<Animator>().SetBool("IsSlipping", true);
				}
				else if(isHole){
					//state dont exist yet
					//EnemyBaseAI baseAi = other.transform.parent.gameObject.GetComponent<EnemyBaseAI>();
					//baseAi.AI.SetState(EnemyBaseAI.FallingState, baseAi);
					//other.transform.parent.gameObject.GetComponent<Animator>().SetBool("isFalling", true);
				}
				else if(isGlove){
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
