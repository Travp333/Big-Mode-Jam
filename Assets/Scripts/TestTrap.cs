using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrap : MonoBehaviour
{
	[SerializeField]
	float radius = 6f;
	[SerializeField]
	float power = 3500f;
	[SerializeField]
	float upModifier = 10f;
	
	protected void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.gameObject.name);
		if (other.tag == "AI")
		{
			/*			if(other.gameObject.GetComponent<RagdollSwap>() != null){
							other.gameObject.GetComponent<RagdollSwap>().StartRagdoll();
							Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius);
							foreach (Collider hit in colliders)
							{
								Rigidbody rb = hit.GetComponent<Rigidbody>();
								if(hit.tag == "ragdoll" && hit.gameObject.name == "Head" || hit.gameObject.name == "spine.001" || hit.gameObject.name == "spine" ||hit.gameObject.name == "upper_arm.L"  ||hit.gameObject.name == "upper_arm.R"  ){
									//rb.AddTorque(((this.transform.right * power) + (this.transform.up * upModifier)), ForceMode.Impulse);
									rb.AddForce(((this.transform.right * power) + (this.transform.up * upModifier)), ForceMode.Impulse);
									//rb.AddExplosionForce(power, ExplodeOrigin.position, radius, upModifier);
								}
							}
						}*/
			EnemyBaseAI enemy;
			if (other.transform.root.TryGetComponent(out enemy))
			{
				enemy.AI.SetState(EnemyBaseAI.RagdollState, enemy);
				Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius);
				foreach (Collider hit in colliders)
				{
					Rigidbody rb = hit.GetComponent<Rigidbody>();
					if (hit.tag == "ragdoll" && hit.gameObject.name == "Head" || hit.gameObject.name == "spine.001" || hit.gameObject.name == "spine" || hit.gameObject.name == "upper_arm.L" || hit.gameObject.name == "upper_arm.R")
					{
						//rb.AddTorque(((this.transform.right * power) + (this.transform.up * upModifier)), ForceMode.Impulse);
						rb.AddForce(((this.transform.right * power) + (this.transform.up * upModifier)), ForceMode.Impulse);
						//rb.AddExplosionForce(power, ExplodeOrigin.position, radius, upModifier);
					}
				}
			}
		}
	}
}
