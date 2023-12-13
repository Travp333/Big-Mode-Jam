using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeRoomHitBox : MonoBehaviour
{
	Transform end;
	public void EnableHitbox(Transform endPoint){
		GetComponent<BoxCollider>().enabled = true;
		end = endPoint;
	}
	public void DisableHitbox(){
		GetComponent<BoxCollider>().enabled = false;
		end = null;
	}
	protected void OnTriggerStay(Collider other)
	{
		if(other.gameObject.transform.parent.parent.GetComponent<Movement>()!= null){
			if(end!= null){
				other.gameObject.transform.parent.parent.gameObject.transform.position = end.position;
				GetComponent<BoxCollider>().enabled = false;
			}
		}
	}
}
