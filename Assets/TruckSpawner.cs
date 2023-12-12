using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckSpawner : MonoBehaviour
{
	[SerializeField]
	GameObject WhiteTruck;
	[SerializeField]
	GameObject BlackTruck;
	[SerializeField]
	float spawnTimerUpper;
	[SerializeField]
	float spawnTimerLower;
	float spawnTimer = 0f;
	float spawnTimerCount = 0f;
	int blackorwhite;
	[SerializeField] public SpawnerType Type;
	public enum SpawnerType { LightOnly, DarkOnly, Both}
	GameObject spawnedTruck;
	[SerializeField]
	int truckSpeedUpper;
	[SerializeField]
	int truckSpeedLower;
    // Start is called before the first frame update
    void Start()
    {
	    SpawnTruck();
    }
	void SpawnTruck(){
		spawnTimer = Random.Range(spawnTimerUpper, spawnTimerLower);
		
		if(Type == SpawnerType.Both){
			blackorwhite = Random.Range(0,2);
			if(blackorwhite == 0){
				spawnedTruck = Instantiate(BlackTruck, this.transform.position, this.transform.rotation);
				spawnedTruck.GetComponent<TruckMovement>().movespeed = Random.Range(truckSpeedLower, truckSpeedUpper);
			}
			else{
				spawnedTruck = Instantiate(WhiteTruck, this.transform.position, this.transform.rotation);
				spawnedTruck.GetComponent<TruckMovement>().movespeed = Random.Range(truckSpeedLower, truckSpeedUpper);
			}	
		}
		else if(Type == SpawnerType.LightOnly){
			spawnedTruck = Instantiate(WhiteTruck, this.transform.position, this.transform.rotation);
			spawnedTruck.GetComponent<TruckMovement>().movespeed = Random.Range(truckSpeedLower, truckSpeedUpper);
		}
		else{
			spawnedTruck = Instantiate(BlackTruck, this.transform.position, this.transform.rotation);
			spawnedTruck.GetComponent<TruckMovement>().movespeed = Random.Range(truckSpeedLower, truckSpeedUpper);

		}

	}
    // Update is called once per frame
    void Update()
    {
	    if(spawnTimerCount < spawnTimer){
	    	spawnTimerCount += Time.deltaTime;
	    }
	    else{
	    	SpawnTruck();
	    	spawnTimerCount = 0f;
	    }
    }
}
