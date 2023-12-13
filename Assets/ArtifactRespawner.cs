using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactRespawner : MonoBehaviour
{
	[SerializeField]
	GameObject artifact;
	[SerializeField]
	Transform spawnPos;
	void spawn(){
		Instantiate(artifact, spawnPos.position, Quaternion.identity);
	}
	public void RespawnArtifact(){
		Invoke("spawn", 2f);
	}
}
