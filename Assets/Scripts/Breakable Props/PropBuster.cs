using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Breaks or damages any breakable object this object collides with
//Travis Parks
public class PropBuster : MonoBehaviour
{
    Shatter otherExplosive;
    void OnCollisionEnter(Collision other) {
	    if(other.gameObject.GetComponent<Rigidbody>() != null){
		    //Debug.Log("Propbuster Hit a thing");
		    if (other.gameObject.tag == "Breakable"){
			    //Debug.Log("that thing is breakable");
			    otherExplosive = other.gameObject.GetComponent<Shatter>();
			    otherExplosive.oneShot(0);
            }
        }
    }
}


