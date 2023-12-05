using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class playerStates : MonoBehaviour
{   
	[SerializeField]
	PlayerPickup pickup;
	[SerializeField]
	GameObject root;
	[SerializeField]
	SimpleCameraMovement fpscamscript;
	Vector3 playerRotation;
	[SerializeField]
	UpdateRotation rot;
	[SerializeField]
	SkinnedMeshRenderer ThirdPersonBaseMesh;
	[SerializeField]
	SkinnedMeshRenderer ThirdPersonSashMesh;
	[SerializeField]
	SkinnedMeshRenderer ThirdPersonSlingMesh;
	[SerializeField]
	SkinnedMeshRenderer ThirdPersonFaceMesh;
	[SerializeField]
	Camera FirstPersonCam;
	[SerializeField]
	SkinnedMeshRenderer FirstPersonHandsMesh;
	[SerializeField]
	SkinnedMeshRenderer FirstPersonSlingMesh;
	[SerializeField]
	Camera ThirdPersonCam;
	[SerializeField]
	Camera FirstPersonHandCam;
	
	[SerializeField]
	public FaceTexController face;
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
	public bool aiming;
	public bool armed;
	public bool firing;
	public InputAction movementAction;
	public InputAction walkAction;
	public InputAction crouchAction;
	public InputAction interactAction;
	public InputAction attackAction;
	public InputAction aimAction;
	public InputAction armAction;
	float lastPressTime;
	[SerializeField]
	float doublePressTime;
	// Accessed by slingshot manager -almost_friday
	public bool FPSorTPS { get; private set; } = true; // True for first person
	// Start is called before the first frame update
	void Awake()
	{
		move = this.GetComponent<Movement>();
		armAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Arm");
		aimAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Aim");
		interactAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Interact");
		attackAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Attack");
		walkAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Walk");
		movementAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Move");
		crouchAction = GetComponent<PlayerInput>().currentActionMap.FindAction("Crouch");
	}
	public void Crouch(){
		if(!armed){
			face.setSneaking();
		}
		crouching = true;
		standingHitbox.SetActive(false);
		crouchingHitbox.SetActive(true);
	}
	public void UnCrouch(){
		if(!armed){
			face.setBase();
		}
		crouching = false;
		standingHitbox.SetActive(true);
		crouchingHitbox.SetActive(false);
	}
	void ResetRolling(){
		//rolling = false;
		Crouch();
	}
	void ResetThrowing(){
		throwing = false;
	}
	public void ResetFiring(){
		firing = false;
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
		if(armAction.WasPressedThisFrame() && move.OnGround && !holding){
			
			if(armed){
				armed = false;
				if(crouching){
					face.setSneaking();
				}
				else{
					face.setBase();
				}
			}
			else{
				armed = true;
				face.setAiming();
			}
		}
		
		if(aimAction.WasPressedThisFrame() && !holding && !rolling){
			if(FPSorTPS){
				aiming = true;
				//Swap to first person!
				//ThirdPersonCam.transform.parent.GetComponent<OrbitCamera>().enabled = false;
				ThirdPersonCam.transform.parent.GetComponent<OrbitCamera>().BlockCamInput();
				FirstPersonHandsMesh.enabled = true;
				FirstPersonSlingMesh.enabled = true;
				rot.enabled = false;
				ThirdPersonBaseMesh.enabled=false;
				ThirdPersonFaceMesh.enabled=false;
				ThirdPersonSashMesh.enabled=false;
				ThirdPersonSlingMesh.enabled=false;
				ThirdPersonCam.enabled=false;
				ThirdPersonCam.GetComponent<AudioListener>().enabled = false;
				fpscamscript.enabled = true;
				fpscamscript.SnapFPStoTPS();
				FirstPersonCam.enabled=true;
				FirstPersonHandCam.enabled = true;
				FirstPersonCam.GetComponent<AudioListener>().enabled = true;
				move.playerInputSpace = FirstPersonCam.transform;
				FPSorTPS = !FPSorTPS;
				ThirdPersonCam.transform.parent.parent = FirstPersonCam.transform;
				
			}
			else{
				//swap to third person!
				//ThirdPersonCam.transform.parent.GetComponent<OrbitCamera>().enabled = true;
				aiming = false;
				FirstPersonHandsMesh.enabled = false;
				FirstPersonSlingMesh.enabled = false;
				ThirdPersonCam.transform.parent.GetComponent<OrbitCamera>().UnBlockCamInput();
				ThirdPersonCam.transform.parent.GetComponent<OrbitCamera>().ResetCameraAngles();
				rot.enabled = true;
				ThirdPersonBaseMesh.enabled=true;
				ThirdPersonFaceMesh.enabled=true;
				ThirdPersonSashMesh.enabled=true;
				if(armed){
					ThirdPersonSlingMesh.enabled=true;
				}
				ThirdPersonCam.enabled=true;
				ThirdPersonCam.GetComponent<AudioListener>().enabled = true;
				FirstPersonCam.enabled=false;
				FirstPersonHandCam.enabled = false;
				FirstPersonCam.GetComponent<AudioListener>().enabled = false;
				move.playerInputSpace = ThirdPersonCam.transform;
				FPSorTPS = !FPSorTPS;
				fpscamscript.enabled = false;
				ThirdPersonCam.transform.parent.parent = root.transform;
				
			}

		}
		if(holding){
			face.setStraining();
		}
		if(crouchAction.WasPressedThisFrame() && move.OnGround && !holding){
			if((Time.time - lastPressTime <= doublePressTime)&& moving && !aiming){
				StopCoroutine("StartCrouch");
				Debug.Log("ROLL!");
				rolling = true;
				Invoke("ResetRolling", doublePressTime);
				Crouch();
				armed = false;
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
		if(armed && attackAction.WasPerformedThisFrame()){
			firing = true;
		}
		if(!holding && interactAction.WasPerformedThisFrame()){
			if(pickup.objectsInTriggerSpace.Count > 0){
				holding = true;
				face.setStraining();			
			}

		}
		else if(holding && interactAction.WasPerformedThisFrame()){
			holding = false;
			face.setBase();
		}
		if(holding && attackAction.WasPerformedThisFrame()){
			throwing = true;
			Invoke("ResetThrowing", 1f);
			holding = false;
			face.setBase();
		}

        
    }
}
