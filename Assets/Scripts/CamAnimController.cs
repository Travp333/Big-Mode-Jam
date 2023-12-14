using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAnimController : MonoBehaviour
{
	[SerializeField]
	PlayerStates state;
	Animator anim;

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		anim=GetComponent<Animator>();
	}
    // Update is called once per frame
    void Update()
    {
	    if(state.crouching){
	    	anim.SetBool("Crouched", true);
	    }
	    else{
	    	anim.SetBool("Crouched", false);
	    }
    }
}
