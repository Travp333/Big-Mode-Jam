using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	protected void OnCollisionEnter(Collision collisionInfo)
	{
		Debug.Log(collisionInfo.gameObject.name);
		if(collisionInfo.gameObject.tag == "Player"){
			if(collisionInfo.gameObject.GetComponent<playerStates>() != null){
				if(collisionInfo.gameObject.GetComponent<playerStates>().rolling){
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
					//Instantiate(ragdoll, ragdollSpawnPos.position, Quaternion.identity);
					//Destroy(this.transform.parent.gameObject);
				}
			}
		}
	}
}
