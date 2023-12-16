using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killplane : MonoBehaviour
{
	[SerializeField]
	GameObject respawnPoint;
	protected void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player"){

			if(other.gameObject.transform.parent.transform.parent.gameObject.GetComponent<Movement>() != null){
				Debug.Log("Saving Player from fallig through level");
				if(!other.gameObject.transform.parent.transform.parent.gameObject.GetComponent<PlayerStates>().choked){
					other.gameObject.transform.parent.transform.parent.gameObject.transform.position = respawnPoint.transform.position;
				}
			}
		}
	}
}
