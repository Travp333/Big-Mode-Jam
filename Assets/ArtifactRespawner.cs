using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactRespawner : MonoBehaviour
{
	[SerializeField]
	GameObject artifact;
	[SerializeField]
	Transform spawnPos;
	public void RespawnArtifact(){
		Instantiate(artifact, spawnPos.position, Quaternion.identity);
	}
}
