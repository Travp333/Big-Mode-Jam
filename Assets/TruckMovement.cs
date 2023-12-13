using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckMovement : MonoBehaviour
{
	[SerializeField]
	public int movespeed = 10;
	public Vector3 userDirection = Vector3.forward;


	public void Update()
	{
		transform.Translate(userDirection * movespeed * Time.deltaTime); 
	}
}
