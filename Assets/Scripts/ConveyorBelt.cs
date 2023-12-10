using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
	GameObject g;
    [SerializeField]
    bool isEndPiece = false;
    [SerializeField]
    float speed;
    [SerializeField]
    [Tooltip("amount of spin added to objects on the conveyor belt")]
    float spinAmount;
    List<GameObject> pushingObjects = new List<GameObject>();
	void OnTriggerEnter(Collider other) {
		//Debug.Log(other.gameObject.name);
        if(other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.GetComponent<Rigidbody>().isKinematic == false && other.gameObject.layer != 16 && other.gameObject.tag != "Breakable" && other.gameObject.tag != "Explosive"&& other.gameObject.tag != "Player" && pushingObjects.Contains(other.gameObject) == false){
            //Debug.Log("an object with a rigidbody just got added");
            pushingObjects.Add(other.gameObject);
        }
        if(other.transform.parent != null){
            if(other.transform.parent.gameObject.GetComponent<Rigidbody>() != null  && other.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic == false && other.transform.parent.gameObject.tag != "Breakable" && other.transform.parent.gameObject.tag != "Explosive" && other.transform.parent.gameObject.tag != "Player" && pushingObjects.Contains(other.transform.parent.gameObject) == false){
                //Debug.Log("an object with a rigibody in its parent just got added");
                pushingObjects.Add(other.transform.parent.gameObject);
            }
        }
		if(other.gameObject.tag == "Player"){
			if(other.gameObject.tag != "Volumes" && (other.gameObject.transform.parent.transform.parent.gameObject.tag == "Player" || other.gameObject.tag == "Player" && pushingObjects.Contains(other.gameObject.transform.parent.transform.parent.gameObject) == false)){
	            //Debug.Log("A player just got added");
				pushingObjects.Add(other.gameObject.transform.parent.transform.parent.gameObject);
			}
		}
    }
	void OnTriggerExit(Collider other) {
        if(other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.GetComponent<Rigidbody>().isKinematic == false && other.gameObject.layer != 16 && other.gameObject.tag != "Player" && pushingObjects.Contains(other.gameObject) == true){
	        //Debug.Log(other.gameObject.name + " just got removed");
            pushingObjects.Remove(other.gameObject);
            if(isEndPiece){
                //Debug.Log("Lil Speed Boost");
                other.gameObject.GetComponent<Rigidbody>().velocity = other.gameObject.GetComponent<Rigidbody>().velocity + this.transform.right * (speed);
            }
        }
        if(other.transform.parent != null){
            if(other.transform.parent.gameObject.GetComponent<Rigidbody>() != null && other.transform.parent.gameObject.tag != "Player" && pushingObjects.Contains(other.transform.parent.gameObject) == true){
                //Debug.Log("An object with a parent rigidbody just got removed");
                pushingObjects.Remove(other.transform.parent.gameObject);
                if(isEndPiece){
                    //Debug.Log("Lil Speed Boost");
                    other.transform.parent.gameObject.GetComponent<Rigidbody>().velocity = other.transform.parent.gameObject.GetComponent<Rigidbody>().velocity + this.transform.right * (speed);
                }
            }
        }
		if(other.gameObject.tag == "Trap"){
			
		}
		if(other.gameObject.tag == "Player"){
			if(other.gameObject.tag != "Volumes" && (other.gameObject.transform.parent.transform.parent.gameObject.tag == "Player" && pushingObjects.Contains(other.gameObject.transform.parent.transform.parent.gameObject) == true)){
	            //Debug.Log("A player just got removed");
			    pushingObjects.Remove(other.gameObject.transform.parent.transform.parent.gameObject);
	            if(isEndPiece){
		            //Debug.Log("Lil Speed Boost for " + other.gameObject.transform.parent.transform.parent.gameObject.name);
		            other.gameObject.transform.parent.transform.parent.gameObject.GetComponent<Rigidbody>().velocity = other.gameObject.transform.parent.transform.parent.gameObject.GetComponent<Rigidbody>().velocity + this.transform.right * (speed);
	            }
		    }
		}
    }
    void FixedUpdate()
    {
        for (int i = 0; i < pushingObjects.Count; i++){
            if (pushingObjects[i] == null){
                //Debug.Log("REMOVED VIA DESTRUCTION");
                pushingObjects.Remove(pushingObjects[i].gameObject);
            }
            else if(pushingObjects[i].gameObject.layer == 16){
                //Debug.Log("REMOVED VIA LAYER");
                pushingObjects.Remove(pushingObjects[i].gameObject);
            }
            else{
                if(speed != 0){
                    pushingObjects[i].transform.position = pushingObjects[i].transform.position + this.transform.right * (speed * Time.deltaTime);
                }
                if(spinAmount != 0){
                    pushingObjects[i].transform.Rotate(new Vector3(0f, Time.deltaTime * spinAmount), Space.World);
                }
            }
        }
    }
}


