using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
	[SerializeField]
	GameObject mainMat;
	[SerializeField]
	Material movingMat;
	[SerializeField]
	Material nonMovingMat;
	[SerializeField]
	bool isEnabled;
	GameObject g;
    [SerializeField]
    bool isEndPiece = false;
    [SerializeField]
    float speed;
    [SerializeField]
    [Tooltip("amount of spin added to objects on the conveyor belt")]
    float spinAmount;
	public List<GameObject> pushingObjects = new List<GameObject>();
	void OnTriggerEnter(Collider other) {
		//Debug.Log(other.gameObject.name);
		if(other.gameObject.tag != "ragdoll" && other.gameObject.tag != "BeltIgnore" && other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.GetComponent<Rigidbody>().isKinematic == false && other.gameObject.tag != "Player" && pushingObjects.Contains(other.gameObject) == false){
			//Debug.Log("an object ( "+ other.gameObject.name+ " )  with a rigidbody just got added");
            pushingObjects.Add(other.gameObject);
        }
        if(other.transform.parent != null){
	        if(other.gameObject.tag != "ragdoll" && other.gameObject.tag != "BeltIgnore" && other.transform.parent.gameObject.GetComponent<Rigidbody>() != null  && other.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic == false && other.transform.parent.gameObject.tag != "Player" && pushingObjects.Contains(other.transform.parent.gameObject) == false){
		        //Debug.Log("an object ( "+ other.transform.parent.gameObject.name+ " )  with a rigibody in its parent just got added");
                pushingObjects.Add(other.transform.parent.gameObject);
            }
        }
		if(other.gameObject.tag == "Player"){
			if(!other.gameObject.transform.parent.transform.parent.gameObject.GetComponent<playerStates>().crouching &&  other.gameObject.tag != "Volumes" && ((other.gameObject.transform.parent.transform.parent.gameObject.tag == "Player") && !pushingObjects.Contains(other.gameObject.transform.parent.transform.parent.gameObject))){
				//Debug.Log("A player ( "+ other.gameObject.transform.parent.transform.parent.gameObject+ " )  just got added");
				pushingObjects.Add(other.gameObject.transform.parent.transform.parent.gameObject);
			}
		}
		if(other.gameObject.tag == "AI"){
			if(other.gameObject.transform.parent.GetComponent<EnemyBaseAI>() && !pushingObjects.Contains(other.gameObject.transform.parent.gameObject)){
				//Debug.Log("An AI ( "+ other.gameObject.transform.parent.gameObject+ " ) just got added");
				pushingObjects.Add(other.gameObject.transform.parent.gameObject);
			}
		}
		if(other.gameObject.tag == "ragdoll" && other.gameObject.tag != "BeltIgnore" && other.gameObject.name == "spine"){
			if(!pushingObjects.Contains(other.gameObject.transform.parent.gameObject)){
				//Debug.Log("A ragdoll ( "+ other.gameObject.transform.parent.gameObject+ " ) just got added by its hip");
				pushingObjects.Add(other.gameObject.transform.parent.gameObject);
			}
		}
		if(other.gameObject.tag == "ragdoll" && other.gameObject.tag != "BeltIgnore" && (other.gameObject.name == "forearm.L" || other.gameObject.name == "forearm.R")){
			if(!pushingObjects.Contains(other.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.gameObject)){
				//Debug.Log("A ragdoll ( "+ other.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.gameObject+ " ) just got added by its arms");
				pushingObjects.Add(other.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.gameObject);
			}
		}
    }
	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag != "ragdoll" && other.gameObject.tag != "BeltIgnore" && other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.GetComponent<Rigidbody>().isKinematic == false && other.gameObject.tag != "Player" && pushingObjects.Contains(other.gameObject) == true){
			//Debug.Log("an object ( "+ other.gameObject.name+ " )  with a rigidbody just got removed");
            pushingObjects.Remove(other.gameObject);
            if(isEndPiece){
                //Debug.Log("Lil Speed Boost");
                other.gameObject.GetComponent<Rigidbody>().velocity = other.gameObject.GetComponent<Rigidbody>().velocity + this.transform.right * (speed);
            }
        }
        if(other.transform.parent != null){
	        if(other.gameObject.tag != "ragdoll" && other.gameObject.tag != "BeltIgnore" && other.transform.parent.gameObject.GetComponent<Rigidbody>() != null && other.transform.parent.gameObject.tag != "Player" && pushingObjects.Contains(other.transform.parent.gameObject) == true){
		        //Debug.Log("An object ( "+ other.transform.parent.gameObject+ " ) with a parent rigidbody just got removed");
                pushingObjects.Remove(other.transform.parent.gameObject);
                if(isEndPiece){
                    //Debug.Log("Lil Speed Boost");
                    other.transform.parent.gameObject.GetComponent<Rigidbody>().velocity = other.transform.parent.gameObject.GetComponent<Rigidbody>().velocity + this.transform.right * (speed);
                }
            }
        }
		if(other.gameObject.tag == "Player"){
			if(other.gameObject.tag != "Volumes" && (other.gameObject.transform.parent.transform.parent.gameObject.tag == "Player" && pushingObjects.Contains(other.gameObject.transform.parent.transform.parent.gameObject) == true)){
				//Debug.Log("A player ( "+ other.gameObject.transform.parent.transform.parent.gameObject+ " ) just got removed");
			    pushingObjects.Remove(other.gameObject.transform.parent.transform.parent.gameObject);
	            if(isEndPiece){
		            //Debug.Log("Lil Speed Boost for " + other.gameObject.transform.parent.transform.parent.gameObject.name);
		            other.gameObject.transform.parent.transform.parent.gameObject.GetComponent<Rigidbody>().velocity = other.gameObject.transform.parent.transform.parent.gameObject.GetComponent<Rigidbody>().velocity + this.transform.right * (speed);
	            }
		    }
		}
		if(other.gameObject.tag == "AI"){
			if(other.gameObject.transform.parent.GetComponent<EnemyBaseAI>() && pushingObjects.Contains(other.gameObject.transform.parent.gameObject)){
				//Debug.Log("An AI ( "+ other.gameObject.transform.parent.gameObject+ " ) just got removed");
				pushingObjects.Remove(other.gameObject.transform.parent.gameObject);
			}
		}
		if(other.gameObject.tag == "ragdoll" && other.gameObject.name == "spine"){
			if(pushingObjects.Contains(other.gameObject.transform.parent.gameObject)){
				//Debug.Log("A ragdoll ( "+ other.gameObject.transform.parent.gameObject+ " ) just got removed by its hip");
				pushingObjects.Remove(other.gameObject.transform.parent.gameObject);
			}
		}
		if(other.gameObject.tag == "ragdoll" && (other.gameObject.name == "forearm.L" || other.gameObject.name == "forearm.R")){
			if(pushingObjects.Contains(other.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.gameObject)){
				//Debug.Log("A ragdoll ( "+ other.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.gameObject+ " ) just got removed by its arms");
				pushingObjects.Remove(other.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.gameObject);
			}
		}
	}
	public void EnableBelt(){
		Debug.Log("Changing Mat");
		mainMat.GetComponent<MeshRenderer>().material = movingMat;
		isEnabled = true;
	}
	public void DisableBelt(){
		Debug.Log("Changing Mat");
		mainMat.GetComponent<MeshRenderer>().material = nonMovingMat;
		isEnabled = false;
	}
    void FixedUpdate()
	{

        for (int i = 0; i < pushingObjects.Count; i++){
	        //if (pushingObjects[i] == null){
                //Debug.Log("REMOVED VIA DESTRUCTION");
	            //     pushingObjects.Remove(pushingObjects[i].gameObject);
		         //}
	        if(pushingObjects[i].GetComponent<EntityParent>() != null){
		        if(pushingObjects[i].GetComponent<EntityParent>().isPickedUp){
			        //Debug.Log("REMOVED VIA PICKUP");
		            pushingObjects.Remove(pushingObjects[i].gameObject);
		        }
		        else if(speed != 0 && isEnabled){
			        pushingObjects[i].transform.position = pushingObjects[i].transform.position + this.transform.right * (speed * Time.deltaTime);

		        }
		        if(spinAmount != 0){
			        pushingObjects[i].transform.Rotate(new Vector3(0f, Time.deltaTime * spinAmount), Space.World);
		        }
	        }
	        else if(pushingObjects[i].tag == "BeltIgnore"){
		        //Debug.Log("REMOVED VIA RAGDOLL");
			    pushingObjects.Remove(pushingObjects[i].gameObject);
	        }
	        else if(speed != 0 && isEnabled){
                pushingObjects[i].transform.position = pushingObjects[i].transform.position + this.transform.right * (speed * Time.deltaTime);
            }
            if(spinAmount != 0){
                pushingObjects[i].transform.Rotate(new Vector3(0f, Time.deltaTime * spinAmount), Space.World);
            }
        }
    }
}


