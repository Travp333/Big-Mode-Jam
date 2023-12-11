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
	GameObject player;
	[SerializeField]
	
	GameObject tubeCamSpot;
    // Start is called before the first frame update
    void Start()
    {
        
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
		
	}
	// OnTriggerEnter is called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other)
	{
		
		if(other.gameObject.tag == "Player"){
			if(other.transform.parent.parent.gameObject.GetComponent<Movement>() != null){
				if(other.transform.parent.parent.gameObject.GetComponent<Movement>().playerInputSpace.gameObject != null){
					Debug.Log("TESTTTT");
					this.GetComponent<Animator>().SetBool("Enter", true);
					Invoke("ResetEnter", .1f);
					other.transform.parent.parent.gameObject.GetComponent<Movement>().playerInputSpace.gameObject.GetComponent<OrbitCamera>().focus = tubeCamSpot.transform;
					Orbitcam = other.transform.parent.parent.gameObject.GetComponent<Movement>().playerInputSpace.gameObject.GetComponent<OrbitCamera>();
					player = other.transform.parent.parent.gameObject;
					other.transform.parent.parent.gameObject.GetComponent<Movement>().blockMovement();
					player.transform.position = safeRoom.transform.position;
						Invoke("SetCamToExit", 2f);
				}
			}
		}
	}
}
