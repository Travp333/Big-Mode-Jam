using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPlayer : MonoBehaviour
{
	[SerializeField]
	GameObject playerDummy;
	public bool isHolding { get; set; }
	GameObject player2 = null;
	Movement move;
	OrbitCamera orbCam;
    // Start is called before the first frame update
    void Start()
    {
	    foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player")){
	    	if(g.GetComponent<Movement>() != null){
	    		move = g.GetComponent<Movement>();
	    	}
	    	if(g.GetComponent<OrbitCamera>()){
	    		orbCam = g.GetComponent<OrbitCamera>();
	    	}
	    }
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
					if(orbCam.focus != null){
						if(move.gameObject.GetComponent<PlayerStates>().FPSorTPS == false){
							//Debug.Log("Grabbed in first person!");
							move.gameObject.GetComponent<PlayerStates>().ForceThirdPerson();
						}
						if(move.gameObject.GetComponent<PlayerStates>().holding == true){
							//Debug.Log("Grabbed while holding something");
							move.gameObject.GetComponent<PlayerStates>().pickup.PutDown();
						}
						move.gameObject.GetComponent<PlayerStates>().SetFPSBlock(true);
						//Got player root!
						player2 = other.gameObject.transform.parent.parent.gameObject;
						isHolding = true;
						//Debug.Log("Grabbed Player Neck");
						playerDummy.SetActive(true);
						//update camera position
						orbCam.focus = this.transform;
						//disable player movement
						move.blockMovement();
						//other.gameObject.transform.parent.parent.gameObject.GetComponent<Movement>().enabled = false;
						move.gameObject.GetComponent<PlayerStates>().choked = true;
						move.gameObject.GetComponent<PlayerStates>().standingHitbox.SetActive(false);
						move.gameObject.GetComponent<PlayerStates>().crouchingHitbox.SetActive(false);
						foreach (SkinnedMeshRenderer m in move.gameObject.GetComponent<PlayerColorChangeBehavior>().mesh){
							m.enabled = false;
						}
						move.gameObject.GetComponent<PlayerColorChangeBehavior>().face.enabled = false;
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
		orbCam.focus = move.center.transform;
		//disable player movement
		player.GetComponent<Movement>().unblockMovement();
		//player.GetComponent<Movement>().enabled = true;
		player.GetComponent<PlayerStates>().SetFPSBlock(false);
		player.GetComponent<PlayerStates>().choked = false;
		player.GetComponent<PlayerStates>().crouching = false;
		player.GetComponent<PlayerStates>().standingHitbox.SetActive(true);
		player.GetComponent<PlayerStates>().crouchingHitbox.SetActive(false);
		foreach (SkinnedMeshRenderer m in player.GetComponent<PlayerColorChangeBehavior>().mesh){
			if(m.name != "Sling Mesh" && m.name != "FPSArms" && m.name != "FPSSling"){
				m.enabled = true;
			}	
		}
		player.GetComponent<PlayerColorChangeBehavior>().face.enabled = true;
		
		//playerTransform = other.gameObject.transform.parent.parent.gameObject.transform;
	}
}
