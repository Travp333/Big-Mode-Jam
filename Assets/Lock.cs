using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
	[SerializeField]
	GameObject door;
	[SerializeField]
	GameObject UnlockedPrefab;
	[SerializeField]
	Transform UnlockPrefabSpawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void Unlock(){
		Instantiate(UnlockedPrefab, UnlockPrefabSpawn.position, UnlockPrefabSpawn.rotation);
		door.GetComponent<Interactable>().OpenDoor = true;
		Destroy(this.gameObject);
	}
}
