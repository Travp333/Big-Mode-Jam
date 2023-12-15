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



		if (p.IsBlack) {
			if (a.volume < 1){
				a.volume += .02f; }
			if (b.volume > 0)
			{
				b.volume -= .02f;
			}
		}
		if (!p.IsBlack )
		{
			if (a.volume > 0){
				a.volume -= .02f;}
			if (b.volume < 1){
				b.volume += .02f;}
		}
		if (a.volume > 1) { a.volume = 1f; }
		if (b.volume > 1) { b.volume = 1f; }





	}
}
