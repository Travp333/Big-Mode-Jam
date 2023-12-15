using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatterable : MonoBehaviour
{
	enum type { breakable, target, artifact }
	[SerializeField]
	SFXManager sFX;
	[SerializeField]
	AudioSource audioSource;
	[SerializeField]
	type thisType = type.breakable;


	private void Awake()
	{
		switch (thisType)
		{
			case type.breakable:
				float v = Random.value;
				if (v > .6)
					audioSource.PlayOneShot(sFX.ceramic1);
				else
					audioSource.PlayOneShot(sFX.ceramic3);
				break;
			case type.target:
				audioSource.PlayOneShot(sFX.ceramic2);
				break;
			case type.artifact:
				audioSource.PlayOneShot(sFX.ceramic3);
				break;
		}

	}
}
