using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtCollider : MonoBehaviour
{
	public PlayerStats p;
	private void OnTriggerEnter(Collider other)
	{
		p = other.gameObject.GetComponentInParent<PlayerStats>();
		if (p != null) {
			p.hp--;
		}
	}

}
