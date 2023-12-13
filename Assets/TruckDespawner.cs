using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckDespawner : MonoBehaviour
{
	protected void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Truck"){
			Destroy(other.transform.parent.gameObject);
		}
	}
}
