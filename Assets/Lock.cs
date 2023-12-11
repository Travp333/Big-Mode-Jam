using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
	[SerializeField]
	GameObject UnlockedPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void Unlock(){
		Instantiate(UnlockedPrefab, this.transform.position, Quaternion.identity);
		Destroy(this.gameObject);
	}
}
