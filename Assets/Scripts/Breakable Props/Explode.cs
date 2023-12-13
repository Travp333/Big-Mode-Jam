using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Travis
public class Explode : MonoBehaviour
{
    [SerializeField] 
    private AudioSource[] bombAudioSource = null;
    Rigidbody body; 
    [SerializeField]
    float radius;
    [SerializeField]
    float power; 
    [SerializeField]
    float upModifier;
    GameObject player;  
    void Start()
    {
        foreach (GameObject G in GameObject.FindGameObjectsWithTag("Player")){
            if(G.GetComponent<Movement>() != null){
                player = G;
            }
        }

        body = GetComponent<Rigidbody>();
        Vector3 explosionPos = transform.position;
        	Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        	foreach (Collider hit in colliders)
        	{
            	Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (hit.transform.IsChildOf(this.transform)){
                    if (rb != null)
                        rb.AddExplosionForce(power, explosionPos, radius, upModifier);
                    }
        }
    }
}
