using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class playerStates : MonoBehaviour
{
	[SerializeField]
	FaceTexController face;
	[SerializeField]
	GameObject standingHitbox;
	[SerializeField]
	GameObject crouchingHitbox;
	Movement move;
	public bool walking;
	public bool crouching;
	public bool moving;
	public bool rolling;
	public bool holding;
	public bool throwing;
	public InputAction movementAction;
	public InputAction walkAction;
	public InputAction crouchAction;
	public InputAction interactAction;
	public InputAction attackAction;
	float lastPressTime;
	[SerializeField]
	float doublePressTime;
	// Start is called before the first frame update
	void Awake()
	{
		move = this.GetComponent<Movement>();
		interactAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Interact");
		attackAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Attack");
		walkAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Walk");
		movementAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Move");
		crouchAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Crouch");
	}
	void Crouch(){
		face.setSneaking();
		crouching = true;
		standingHitbox.SetActive(false);
		crouchingHitbox.SetActive(true);
	}
	void UnCrouch(){
		face.setBase();
		crouching = false;
		standingHitbox.SetActive(true);
		crouchingHitbox.SetActive(false);
	}
	void ResetRolling(){
		rolling = false;
		Crouch();
	}
	void ResetThrowing(){
		throwing = false;
	}
	IEnumerator StartCrouch()
	{
		yield return new WaitForSeconds(doublePressTime);
		Debug.Log("Croumch" + (Time.time - lastPressTime));
		rolling = false;
		if(crouching){
			UnCrouch();
		}
		else{
			Crouch();
		}
	}
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
	{
		if(holding){
			face.setStraining();
		}
		if(crouchAction.WasPressedThisFrame() && move.OnGround && !holding){
			if((Time.time - lastPressTime <= doublePressTime)&& moving){
				StopCoroutine("StartCrouch");
				Debug.Log("ROLL!");
				rolling = true;
				Invoke("ResetRolling", doublePressTime);
				Crouch();
			}
			else{
				lastPressTime = Time.time;
				StartCoroutine("StartCrouch");
			}
		}
		if(walkAction.IsPressed()){
			walking = true;
		}
		else{
			walking = false;
		}
	    
		if(holding && attackAction.WasPerformedThisFrame()){
			throwing = true;
			Invoke("ResetThrowing", 1f);
			holding = false;
			face.setBase();
		}
		else if(holding && interactAction.WasPerformedThisFrame()){
			holding = false;
			face.setBase();
		}
        
    }
}
