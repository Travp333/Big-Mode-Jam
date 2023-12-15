using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    [SerializeField]
    PlayerColorChangeBehavior p;
    [SerializeField]
    AudioSource a,b;
	float t = 0f;

	private void Update()
	{
		if (p.IsBlack)
		{
			
			a.volume = 1;
			b.volume = 0;
		}
		else {
			a.volume = 0;
			b.volume = 1;
		}
	}
}
