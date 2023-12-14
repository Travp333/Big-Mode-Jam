using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//handles "breaking" a breakable object, as wella s exploding explodable objects. upon being called, it will delete the original object, then spawn a prefab of debris with a force added to it, giving 
// the effect of a shatter. if its explosive, this force also effects the environment as well as the shards. after that, the shards despawn after a set amount of time 
//Travis Parks
public class Shatter : MonoBehaviour
{
	[SerializeField]
	GameObject door;
	[SerializeField]
	GameObject[] belt;
	ArtifactRespawner startingPodium;
    public GameObject shatterPrefab;
    [Tooltip("What shattered mesh spawns")]
    public GameObject shatterSpawnPos;
    [SerializeField]
    float breakSpeed = 40f;
    GameObject player;
	GameObject sounds;
	float time = .5f;
	float count;
	float delayedMagnitude;
	protected void Update()
	{
		if(count < time){
			count += Time.deltaTime;
		}
		else{
			//Debug.Log("SPEED IS " + this.gameObject.GetComponent<Rigidbody>().velocity.magnitude);
			count = 0f;
			delayedMagnitude = this.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
		}
	}
	void OnCollisionEnter(Collision other) {
		if(other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.tag != "Player" && (other.gameObject.GetComponent<Rigidbody>().velocity.magnitude > breakSpeed)){
		    spawnShatter();
	    }
	    else if (delayedMagnitude > breakSpeed){
	    	delayedMagnitude = 0f;
	    	spawnShatter();
	    }
    }

	void Start() {
		//startingPodium
		foreach(GameObject s in GameObject.FindGameObjectsWithTag("Podium")){
			startingPodium = s.GetComponent<ArtifactRespawner>();
		}
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player")){
            if(g.GetComponent<Movement>()!=null){
	            player = g;
	            //sounds = g.transform.GetComponentInChildren<SoundCaller>().gameObject;
                //sounds = g.transform.GetChild(5).gameObject;
            }
        }
    }
    public void oneShot(float time){
        Invoke("spawnShatter", time);
    }
	void spawnShatter(){
		if(door != null){
			door.GetComponent<Interactable>().OpenDoor = true;
		}
		if(belt != null){
			foreach(GameObject b in belt){
				b.GetComponent<ConveyorBelt>().EnableBelt();
			}
			
		}
		if(this.gameObject.GetComponent<isArtifact>() != null){
			startingPodium.RespawnArtifact();
		}
		Instantiate(shatterPrefab, shatterSpawnPos.transform.position, shatterSpawnPos.transform.rotation);
		if(player.GetComponent<playerStates>().holding){
			player.GetComponent<playerStates>().pickup.gameObject.GetComponent<PlayerPickup>().PutDown();
			player.GetComponent<playerStates>().pickup.gameObject.GetComponent<PlayerPickup>().RemoveFromList(this.gameObject);
		}
        Destroy(this.gameObject);
    }
}
