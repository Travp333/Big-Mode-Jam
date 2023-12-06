using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;

public class RagdollSwap : MonoBehaviour
{
	UnityEngine.AI.NavMeshAgent agent;
	[SerializeField]
	Rigidbody RagdollPelvis;
	[SerializeField]
	GameObject EnemyPrefab;
	[SerializeField]
	SkinnedMeshRenderer[] BaseMeshes;
	[SerializeField]
	SkinnedMeshRenderer[] RagdollMeshes;
	[SerializeField]
	GameObject Ragdoll;
	[SerializeField]
	GameObject BaseEnemy;
	[SerializeField]
	GameObject baseRig;
	[SerializeField]
	GameObject ragdollRig;
	[SerializeField]
	Transform ragdollSpawnPos; 
	[SerializeField]
	GameObject Player;
	[SerializeField]
	LayerMask mask;
	public EnemyBaseAI enemyAI;
	public bool ragdollBlock;
	// Start is called before the first frame update
	void ResetRagdollBlock(){
		ragdollBlock = false;
	}
	void SnapToGround(){
		RaycastHit hit;
		if(Physics.Raycast(this.transform.position, -this.transform.up, out hit, 10f, mask)){
			BaseEnemy.transform.position = new Vector3(BaseEnemy.transform.position.x, hit.point.y, BaseEnemy.transform.position.z);
		}
	}
	void Awake(){
		ragdollBlock = true;

		Invoke("ResetRagdollBlock", 2f);
	}
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    //this.transform.position = RagdollPelvis.transform.position;
    }
	public void StartRagdoll(){
		if(!ragdollBlock){
			BaseEnemy.GetComponent<Animator>().enabled = false;
			BaseEnemy.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
			Ragdoll.GetComponent<Animator>().enabled = false;
			ragdollRig.SetActive(true);
			baseRig.SetActive(false);
			foreach(SkinnedMeshRenderer s in BaseMeshes){
				s.enabled = false;
			}
			foreach(SkinnedMeshRenderer s in RagdollMeshes){
				s.enabled = true;
			}
			GetComponent<CapsuleCollider>().enabled = false;
			gameObject.tag = "Untagged";
			
			//Invoke("RevertRagdoll", 10f); // Temporarily getting replaced by AI state change
		}
		
	}
	public void Kill(){
		if(!ragdollBlock){
			BaseEnemy.GetComponent<Animator>().enabled = false;
			BaseEnemy.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
			Ragdoll.GetComponent<Animator>().enabled = false;
			ragdollRig.SetActive(true);
			baseRig.SetActive(false);
			foreach(SkinnedMeshRenderer s in BaseMeshes){
				s.enabled = false;
			}
			foreach(SkinnedMeshRenderer s in RagdollMeshes){
				s.enabled = true;
			}
			GetComponent<CapsuleCollider>().enabled = false;
			gameObject.tag = "Untagged";
		}
		
	}
	
	public void RevertRagdoll(){
		if(RagdollPelvis.velocity.magnitude < 5f){
			
			ragdollRig.SetActive(false);
			BaseEnemy.transform.position = RagdollPelvis.transform.position;
			Ragdoll.transform.position = this.transform.position;
			//SnapToGround();
			BaseEnemy.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
			BaseEnemy.GetComponent<Animator>().enabled = true;
			Ragdoll.GetComponent<Animator>().enabled = true;
			ragdollRig.transform.position = baseRig.transform.position;
			
			baseRig.SetActive(true);
			foreach(SkinnedMeshRenderer s in BaseMeshes){
				s.enabled = true;
			}
			foreach(SkinnedMeshRenderer s in RagdollMeshes){
				s.enabled = false;
			}
			GetComponent<CapsuleCollider>().enabled = true;
			gameObject.tag = "AI";
;
			//BaseEnemy.GetComponent<Animator>().Play("Get Up"); // Gets called in RiseState
			
			//GameObject g = Instantiate(EnemyPrefab, this.transform.position, Quaternion.identity);
			
			//if(g.GetComponent<Animator>()!=null){
			//	g.GetComponent<Animator>().
			//}
			//	Destroy(BaseEnemy);
		}
		else{
			Invoke("RevertRagdoll", 3f);
		}

	}
	protected void OnCollisionEnter(Collision collisionInfo)
	{
		Debug.Log(collisionInfo.gameObject.name);
		if(collisionInfo.gameObject.tag == "Player"){
			if(collisionInfo.gameObject.GetComponent<playerStates>() != null){
				if(collisionInfo.gameObject.GetComponent<playerStates>().rolling){
					enemyAI.AI.SetState(EnemyBaseAI.RagdollState, enemyAI);
				}
			}
		}
	}
}
