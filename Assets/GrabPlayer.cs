using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPlayer : MonoBehaviour
{
	[SerializeField]
	GameObject playerDummy;
	bool isHolding;
	GameObject player2 = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    if(isHolding){
	    	if(player2 !=null){
	    		player2.transform.position = this.transform.position;
	    	}
	    }
    }
	public void EnableVolume(){
		this.GetComponent<BoxCollider>().enabled = true;
	}
	public void DisableVolume(){
		this.GetComponent<BoxCollider>().enabled = false;
	}
	// OnTriggerEnter is called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.transform.parent != null){
			if(other.gameObject.transform.parent.parent != null){
				if(other.gameObject.transform.parent.parent.GetComponent<Movement>()!= null){
					if(other.gameObject.transform.parent.parent.gameObject.GetComponent<Movement>().playerInputSpace.gameObject.GetComponent<OrbitCamera>().focus != null){
						//Got player root!
						player2 = other.gameObject.transform.parent.parent.gameObject;
						isHolding = true;
						Debug.Log("Grabbed Player Neck");
						//other.gameObject.transform.parent.parent.gameObject.GetComponent<Rigidbody>().isKinematic = true;
						//enable fake player
						playerDummy.SetActive(true);
						//update camera position
						other.gameObject.transform.parent.parent.gameObject.GetComponent<Movement>().playerInputSpace.gameObject.GetComponent<OrbitCamera>().focus = this.transform;
						//disable player movement
						other.gameObject.transform.parent.parent.gameObject.GetComponent<Movement>().blockMovement();
						//other.gameObject.transform.parent.parent.gameObject.GetComponent<Movement>().enabled = false;
						other.gameObject.transform.parent.parent.gameObject.GetComponent<playerStates>().choked = true;
						other.gameObject.transform.parent.parent.gameObject.GetComponent<playerStates>().standingHitbox.SetActive(false);
						other.gameObject.transform.parent.parent.gameObject.GetComponent<playerStates>().crouchingHitbox.SetActive(false);
						foreach (SkinnedMeshRenderer m in other.gameObject.transform.parent.parent.gameObject.GetComponent<PlayerColorChangeBehavior>().mesh){
							m.enabled = false;
						}
						other.gameObject.transform.parent.parent.gameObject.GetComponent<PlayerColorChangeBehavior>().face.enabled = false;
						//playerTransform = other.gameObject.transform.parent.parent.gameObject.transform;
					}
				}
			}
		}
	}
	public void ReleasePlayer(GameObject player){
		//Got player root!
		isHolding = false;
		player2 = null;
		Debug.Log("Grabbed Player Neck");
		//player.transform.parent.transform.position = new Vector3(0,0,0);
		//player.GetComponent<Rigidbody>().isKinematic = false;
		//enable fake player
		playerDummy.SetActive(false);
		//update camera position
		player.GetComponent<Movement>().playerInputSpace.gameObject.GetComponent<OrbitCamera>().focus = player.GetComponent<Movement>().center.transform;
		//disable player movement
		player.GetComponent<Movement>().unblockMovement();
		//player.GetComponent<Movement>().enabled = true;
		player.GetComponent<playerStates>().choked = false;
		player.GetComponent<playerStates>().crouching = false;
		player.GetComponent<playerStates>().standingHitbox.SetActive(true);
		player.GetComponent<playerStates>().crouchingHitbox.SetActive(false);
		foreach (SkinnedMeshRenderer m in player.GetComponent<PlayerColorChangeBehavior>().mesh){
			if(m.name != "Sling Mesh"){
				m.enabled = true;
			}	
		}
		player.GetComponent<PlayerColorChangeBehavior>().face.enabled = true;
		
		//playerTransform = other.gameObject.transform.parent.parent.gameObject.transform;
	}
}
