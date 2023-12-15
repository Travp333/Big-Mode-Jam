using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckDespawner : MonoBehaviour
{
	protected void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.transform.parent != null){
			if(other.gameObject.transform.parent.GetComponent<TruckMovement>()!= null){
				Destroy(other.transform.parent.gameObject);
			}
		}
	}
}
