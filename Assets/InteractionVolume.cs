using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionVolume : MonoBehaviour
{
	[SerializeField]
	GameObject[] trigger;
	// OnTriggerEnter is called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player"){
			Debug.Log("HIT player");
			foreach(GameObject g in trigger){
				if(g.GetComponent<ConveyorBelt>() != null){
					g.GetComponent<ConveyorBelt>().EnableBelt();
				}
				if(g.GetComponent<Interactable>() != null){
					Debug.Log("DOOR!!");
					g.GetComponent<Interactable>().OpenDoor = true;
					Destroy(this.gameObject);
				}
			}

		}
	}
}
