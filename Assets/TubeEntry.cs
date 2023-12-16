using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeEntry : MonoBehaviour
{
	[SerializeField]
	GameObject safeZoneVolume;
	[SerializeField]
	GameObject tubeExit;
	[SerializeField]
	GameObject safeRoom;
	OrbitCamera Orbitcam;
	Movement player;
	[SerializeField]
	
	GameObject tubeCamSpot;
    // Start is called before the first frame update
    void Start()
	{
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")){
			if(g.GetComponent<Movement>()!= null){
				player = g.GetComponent<Movement>();
			}
			if(g.GetComponent<OrbitCamera>()!= null){
				Orbitcam = g.GetComponent<OrbitCamera>();
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void ResetEnter(){
		this.GetComponent<Animator>().SetBool("Enter", false);
	}
	public void ResetExit(){
		tubeExit.transform.parent.GetComponent<Animator>().SetBool("Exit", false);
	}
	public void SetCamToExit(){
		
		Orbitcam.focus = tubeExit.transform;
		Invoke("SpawnAtExit", 2f);
	}
	public void DisableHitbox(){
		safeZoneVolume.GetComponent<SafeRoomHitBox>().DisableHitbox();
	}
	public void SpawnAtExit(){
		safeZoneVolume.GetComponent<SafeRoomHitBox>().EnableHitbox(tubeExit.transform);
		player.GetComponent<Movement>().unblockMovement();
		player.transform.position = tubeExit.transform.position;
		tubeExit.transform.parent.GetComponent<Animator>().SetBool("Exit", true);
		//this.GetComponent<Animator>().SetBool("Exit", true);
		Invoke("ResetExit", .1f);
		Orbitcam.focus = player.GetComponent<Movement>().center.transform;
		Invoke("DisableHitbox", 1f);
		player.gameObject.GetComponent<PlayerStates>().SetFPSBlock(false);
		player.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		
	}
	public void DumpPlayer(Transform playerRoot)
    {
        Movement mov = playerRoot.GetComponentInChildren<Movement>();
        PlayerStates states = playerRoot.GetComponentInChildren<PlayerStates>();
        if (mov != null)
        {
            if (mov.playerInputSpace.gameObject != null)
            {
                if (states.FPSorTPS == false)
                {
                    //Debug.Log("Tubed while in first person something");
                    states.ForceThirdPerson();
                }
                if (states.holding == true)
                {
                    //Debug.Log("Tubed while holding something");
                    states.pickup.PutDown();
                }
                states.SetFPSBlock(true);
                this.GetComponent<Animator>().SetBool("Enter", true);
                Invoke("ResetEnter", .1f);
                Orbitcam.focus = tubeCamSpot.transform;
                mov.blockMovement();
                player.transform.position = safeRoom.transform.position;
                Invoke("SetCamToExit", 2f);
            }
        }
    }
	// OnTriggerEnter is called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.gameObject.name);
		if (other.tag == "AI")
		{
			EnemyBaseAI ai = other.transform.root.GetComponent<EnemyBaseAI>();
            ai.tube = this;
			if (ai) ai.AI.SetState(EnemyBaseAI.DumpingState, ai);
		}
/*        if (other.gameObject.tag == "Player")
        {
            if (other.transform.parent.parent.gameObject.GetComponent<Movement>() != null)
            {
                if (other.transform.parent.parent.gameObject.GetComponent<Movement>().playerInputSpace.gameObject != null)
                {
                    if (other.transform.parent.parent.gameObject.GetComponent<PlayerStates>().FPSorTPS == false)
                    {
                        //Debug.Log("Tubed while in first person something");
                        other.transform.parent.parent.gameObject.GetComponent<PlayerStates>().ForceThirdPerson();
                    }
                    if (other.transform.parent.parent.gameObject.GetComponent<PlayerStates>().holding == true)
                    {
                        //Debug.Log("Tubed while holding something");
                        other.transform.parent.parent.gameObject.GetComponent<PlayerStates>().pickup.PutDown();
                    }
                    player.gameObject.GetComponent<PlayerStates>().SetFPSBlock(true);
                    this.GetComponent<Animator>().SetBool("Enter", true);
                    Invoke("ResetEnter", .1f);
                    Orbitcam.focus = tubeCamSpot.transform;
                    player.GetComponent<Movement>().blockMovement();
                    player.transform.position = safeRoom.transform.position;
                    Invoke("SetCamToExit", 2f);
                }
            }
        }*/
    }
}
