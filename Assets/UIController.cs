using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
	[SerializeField]
	GameObject BlackBG;
	[SerializeField]
	GameObject WhiteBG;
	[SerializeField]
	GameObject WhiteStandingIcon;
	[SerializeField]
	GameObject WhiteCrouchingIcon;
	[SerializeField]
	GameObject BlackCrouchingIcon;
	[SerializeField]
	GameObject HiddenIcon;
	[SerializeField]
	GameObject SusIcon;
	[SerializeField]
	GameObject DetectedIcon;
	[SerializeField]
	GameObject BlackStandingIcon;
	[SerializeField]
	GameObject WhiteSlingShotIcon;
	[SerializeField]
	GameObject BlackSlingShotIcon;
	[SerializeField]
	GameObject WhiteChokeIcon;
	[SerializeField]
	GameObject BlackChokeIcon;
	PlayerStates states;
	PlayerColorChangeBehavior color;
	bool LightorDark; //true = black, false = white
    // Start is called before the first frame update
    void Start()
    {
	    states = GameObject.FindFirstObjectByType<PlayerStates>();
	    foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player")){
	    	if (g.GetComponent<PlayerColorChangeBehavior>()!= null){
	    		color = g.GetComponent<PlayerColorChangeBehavior>();
	    	}
	    }
    }

    // Update is called once per frame
    void Update()
    {
	    if(color.IsBlack){
	    	LightorDark = true;
	    	BlackBG.SetActive(true);
	    	WhiteBG.SetActive(false);
	    }
	    else{
	    	LightorDark = false;
		    BlackBG.SetActive(false);
	    	WhiteBG.SetActive(true);
	    }
	    if(states.choked){
		    if(LightorDark){
	    		BlackCrouchingIcon.SetActive(false);
	    		BlackStandingIcon.SetActive(false);
	    		WhiteCrouchingIcon.SetActive(false);
	    		WhiteStandingIcon.SetActive(false);
	    		BlackChokeIcon.SetActive(true);
	    		WhiteChokeIcon.SetActive(false);
	    	}
	    	else{
		    	BlackCrouchingIcon.SetActive(false);
	    		BlackStandingIcon.SetActive(false);
	    		WhiteCrouchingIcon.SetActive(false);
	    		WhiteStandingIcon.SetActive(false);
		    	BlackChokeIcon.SetActive(false);
	    		WhiteChokeIcon.SetActive(true);
	    	}
	    }
	    else if(states.crouching){
	    	if(LightorDark){
	    		BlackCrouchingIcon.SetActive(true);
	    		BlackStandingIcon.SetActive(false);
	    		WhiteCrouchingIcon.SetActive(false);
	    		WhiteStandingIcon.SetActive(false);
		    	BlackChokeIcon.SetActive(false);
	    		WhiteChokeIcon.SetActive(false);
	    	}
	    	else{
		    	BlackCrouchingIcon.SetActive(false);
	    		BlackStandingIcon.SetActive(false);
	    		WhiteCrouchingIcon.SetActive(true);
	    		WhiteStandingIcon.SetActive(false);
		    	BlackChokeIcon.SetActive(false);
	    		WhiteChokeIcon.SetActive(false);
	    	}
	    }
	    else{
		    if(LightorDark){
	    		BlackCrouchingIcon.SetActive(false);
	    		BlackStandingIcon.SetActive(true);
	    		WhiteCrouchingIcon.SetActive(false);
	    		WhiteStandingIcon.SetActive(false);
			    BlackChokeIcon.SetActive(false);
	    		WhiteChokeIcon.SetActive(false);
	    	}
	    	else{
		    	BlackCrouchingIcon.SetActive(false);
	    		BlackStandingIcon.SetActive(false);
	    		WhiteCrouchingIcon.SetActive(false);
	    		WhiteStandingIcon.SetActive(true);
		    	BlackChokeIcon.SetActive(false);
	    		WhiteChokeIcon.SetActive(false);
	    	}
	    }
	    if(states.armed){
		    if(LightorDark){
	    		BlackSlingShotIcon.SetActive(true);
	    		WhiteSlingShotIcon.SetActive(false);

	    	}
	    	else{
	    		BlackSlingShotIcon.SetActive(false);
	    		WhiteSlingShotIcon.SetActive(true);
	    	}
	    }
	    else{
		    BlackSlingShotIcon.SetActive(false);
		    WhiteSlingShotIcon.SetActive(false);
	    }
    }
}
