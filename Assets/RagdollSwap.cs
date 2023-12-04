using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSwap : MonoBehaviour
{
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
	// Start is called before the first frame update
	void Awake(){
		RaycastHit hit;
		if(Physics.Raycast(this.transform.position, -this.transform.up, out hit, 10f, mask)){
			BaseEnemy.transform.position = hit.point;
		}
	}
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void StartRagdoll(){
		BaseEnemy.GetComponent<Animator>().enabled = false;
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
		
		Invoke("RevertRagdoll", 10f);
		
	}
	
	public void RevertRagdoll(){
		if(RagdollPelvis.velocity.magnitude < 5f){
			GameObject g = Instantiate(EnemyPrefab, RagdollPelvis.transform.position, Quaternion.identity);
			
			if(g.GetComponent<Animator>()!=null){
				g.GetComponent<Animator>().Play("Get Up");
			}
				Destroy(BaseEnemy);
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
					StartRagdoll();
				}
			}
		}
	}
}
