using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTexController : MonoBehaviour
{
	public bool isHoldingBreath, isScared, isStraining, isHappy, isTrance, isAiming, isSneaking;
    bool flipFlop;
    bool isBlinking;
	Vector2 holdBreathID = new Vector2(-0.625f, 0.125f);
    Vector2 strainedID = new Vector2(0.25f, -0.25f), strainedBlink1ID = new Vector2(-.125f * 4f, -.125f -.125f), strainedBlink2ID = new Vector2(-.125f - .125f * 2f, -.125f -.125f), strainedLeftID = new Vector2(-.125f, -.125f - .125f), strainedLeftBlinkID = new Vector2(-.125f -.125f, -.125f - .125f), strainedRightID = new Vector2(.125f, -.125f - .125f), strainedRightBlinkID = new Vector2(0f, -.125f -125f -.125f);
    Vector2 baseID = new Vector2(0f, 0f), baseBlink1ID = new Vector2(0f +.125f*2f, 0f + .125f), baseBlink2ID = new Vector2(-.125f * 5f, 0f), baseLeftID = new Vector2(-.125f * 3f, 0f), baseLeftBlinkID = new Vector2(-.125f * 4f, 0f), baseRightID = new Vector2(-.125f * 1f, 0f), baseRightBlinkID = new Vector2(-.125f * 2f, 0f);
	Vector2 TranceID = new Vector2(-0.375f, -0.5f), TranceFlipID = new Vector2(-0.375f - .125f, -0.5f), TranceBlink1ID = new Vector2(-0.375f - .125f*2f, -0.5f),  TranceFlipBlink1ID = new Vector2(-0.375f + .125f*5f, -0.5f + .125f), TranceBlink2 = new Vector2(0f, -.625f);
    Vector2 scaredID = new Vector2(0.125f, -0.375f), scaredBlink1ID = new Vector2(-.125f*5f, -.125f * 3f), scaredBlink2ID = new Vector2(-.125f*4f, -.125f * 3f), scaredRightID = new Vector2(0f, -.125f*3f), scaredRightBlinkID = new Vector2(-.125f, -.125f*3f), scaredLeftID = new Vector2(-.125f -.125f, -.125f*3f), scaredLeftBlinkID = new Vector2(- .125f*3f, -.125f*3f);
    Vector2 happyID = new Vector2(-0.125f, -0.125f), happyBlink1ID = new Vector2(-0.125f + .125f*2f, -0.125f + .125f), happyBlink2ID = new Vector2(.125f + .125f, 0f), happyLeftID = new Vector2(.125f * -4f, -.125f), happyLeftBlinkID = new Vector2(-0.125f + .125f*4f, -0.125f), happyRightID = new Vector2(-.125f -.125f, -.125f), happyRightBlinkID = new Vector2(.125f * -3f, -.125f);
	Vector2 sneakingID = new Vector2(-0.5f , -.625f), sneakingBlink1ID = new Vector2(-.25f, -.5f), sneakingBlink2ID = new Vector2(-0.125f, -.5f), sneakingLeftID = new Vector2(.125f, -.5f), sneakingLeftBlinkID = new Vector2(0f, -.5f), sneakingRightID = new Vector2(-.625f, -.625f), sneakingRightBlinkID = new Vector2(.25f, -.5f);
	Vector2 aimingID = new Vector2(-0.125f , -.625f), aimingBlink1ID = new Vector2(-.375f, -.625f), aimingBlink2ID = new Vector2(-0.25f, -.625f);
	bool isLookingLeft, isLookingRight;
    [SerializeField]
    bool forceLeft, forceRight, forceStraight;
    [SerializeField]
	float blinkTimerLow = 5f, blinkTimerHigh = 10f;
	//[SerializeField]
	float tranceTime = .5f;
    //Start is called before the first frame update
    void Start(){
        Base();
    }

	public void setHoldingBreath(){
        StopAllCoroutines();
        isHoldingBreath = true;
        isScared = false;
        isStraining = false;
        isHappy = false;
		isTrance = false;
		isAiming = false;
		isSneaking = false;
        //Debug.Log("angry");
		this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", holdBreathID);
        Base();

	}
	public void setAiming(){
		StopAllCoroutines();
		isAiming = true;
		isSneaking = false;
		isHoldingBreath = false;
		isScared = false;
		isStraining = false;
		isHappy = false;
		isTrance = false;
		//Debug.Log("angry");
		this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", aimingID);
		Base();

	}
	public void setSneaking(){
		StopAllCoroutines();
		isAiming = false;
		isSneaking = true;
		isHoldingBreath = false;
		isScared = false;
		isStraining = false;
		isHappy = false;
		isTrance = false;
		//Debug.Log("angry");
		this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", sneakingID);
		Base();

	}
    public void setBase(){
	    StopAllCoroutines();
	    isAiming = false;
	    isSneaking = false;
        isHoldingBreath = false;
        isScared = false;
        isStraining = false;
        isHappy = false;
        isTrance = false;
        //Debug.Log("base");
        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", baseID);

        Base();
    }
    public void setScared(){
	    StopAllCoroutines();
	    isAiming = false;
	    isSneaking = false;
        isHoldingBreath = false;
        isScared = true;
        isStraining = false;
        isHappy = false;
        isTrance = false;
        //Debug.Log("scared");
        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", scaredID);
        Base();
    }
    public void setTrance(){
	    StopAllCoroutines();
	    isAiming = false;
	    isSneaking = false;
        isHoldingBreath = false;
        isScared = false;
        isStraining = false;
        isHappy = false;
        isTrance = true;
        //Debug.Log("trance");
        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", TranceID);
        Base();
    }
    public void setHappy(){
	    StopAllCoroutines();
	    isAiming = false;
	    isSneaking = false;
        isHoldingBreath = false;
        isScared = false;
        isStraining = false;
        isHappy = true;
        isTrance = false;
        //Debug.Log("happy");
        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", happyID);
        Base();
    }
	public void setStraining(){
        StopAllCoroutines();
        isHoldingBreath = false;
        isScared = false;
        isStraining = true;
        isHappy = false;
		isTrance = false;
		isAiming = false;
		isSneaking = false;
        //Debug.Log("sad");
		this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", strainedID);
        Base();
    }

    void tranceFlip(){
        if(isTrance && !isBlinking){
	        //      if(flipFlop){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", TranceID);
	            //         flipFlop = !flipFlop;
	            //     }
	        //     else{
	            //         this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", TranceFlipID);
	            //        flipFlop = !flipFlop;
	            //    }
	        //    Invoke("tranceFlip", tranceTime);
	        }

    }
    void Base(){
	    if(isHoldingBreath){
		    this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", holdBreathID);
        }
        else if(isScared){
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", scaredLeftID);
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", scaredRightID);
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", scaredID);
            }
        }
        else if(isStraining){
            if(isLookingLeft){
	            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", strainedLeftID);
            }
            else if (isLookingRight){
	            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", strainedRightID);
            }
            else{
	            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", strainedID);
            }
        }
        else if(isHappy){
            if(isLookingLeft){
	            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", happyLeftID);
            }
            else if (isLookingRight){
	            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", happyRightID);
            }
            else{
	            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", happyID);
            }
        }
        else if(isAiming){
			this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", aimingID);
        }
        else if(isSneaking){
	        if(isLookingLeft){
		        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", sneakingLeftID);
	        }
	        else if (isLookingRight){
		        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", sneakingRightID);
	        }
	        else{
		        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", sneakingID);
	        }
        }
        else if(isTrance){
            tranceFlip();
        }
        else{
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", baseLeftID);
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", baseRightID);
            }
            else{
	            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", baseID);
            }
        }
        StartCoroutine(callBlinkDown());
    }


    void BlinkDown(){
	    if(isHoldingBreath){
	             this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", holdBreathID);
	     }
	    else if(isScared){
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", scaredLeftBlinkID);
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", scaredRightBlinkID);
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", scaredBlink1ID);
            }
        }
        else if(isStraining){
            if(isLookingLeft){
	            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", strainedLeftBlinkID);
            }
            else if (isLookingRight){
	            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", strainedRightID);
            }
            else{
	            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", strainedBlink1ID);
            }
        }
        else if(isHappy){
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", happyLeftBlinkID);
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", happyRightBlinkID);
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", happyBlink1ID);                
            }
        }
        else if(isTrance){
            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", TranceBlink1ID);
	        Invoke("flipTrance", tranceTime);
        }
        else if(isAiming){
	        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", aimingBlink1ID);
        }
        else if(isSneaking){
	        if(isLookingLeft){
		        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", sneakingLeftBlinkID);
	        }
	        else if (isLookingRight){
		        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", sneakingRightBlinkID);
	        }
	        else{
		        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", sneakingBlink1ID);                
	        }
        }
        else{
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", baseLeftBlinkID);
            }
            else if(isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", baseRightBlinkID);
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", baseBlink1ID);
            }
        }
        StartCoroutine(callBlink());
    }
	void Blink(){
		if(isHoldingBreath){
			this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", holdBreathID);
		}
		else if(isScared){
            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", scaredBlink2ID);
        }
        else if(isStraining){
	        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", strainedBlink2ID);
        }
        else if(isHappy){
            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", happyBlink2ID);
        }
        else if(isTrance){
	        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", TranceBlink2);
        }
        else if(isAiming){
	        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", aimingBlink2ID);
        }
        else if(isSneaking){
	        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", sneakingBlink2ID);
        }
        else{
            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", baseBlink2ID);
        }
        StartCoroutine(callBlinkUp());
    }
	void BlinkUp(){
		if(isHoldingBreath){
			this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", holdBreathID);
		}
		else if(isScared){
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", scaredLeftBlinkID);
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", scaredRightBlinkID);
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", scaredBlink1ID);
            }
        }
        else if(isStraining){
            if(isLookingLeft){
	            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", strainedLeftBlinkID);
            }
            else if (isLookingRight){
	            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", strainedRightBlinkID);
            }
            else{
	            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", strainedBlink1ID);
            }
        }
        else if(isHappy){
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", happyLeftBlinkID);
            }
            else if (isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", happyRightBlinkID);
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", happyBlink1ID);
            }
        }
        else if(isTrance){
            this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", TranceBlink1ID);
	        Invoke("flipTrance", tranceTime);
        }
        else if(isAiming){
			this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", aimingBlink1ID);
        }
        else if(isSneaking){
	        if(isLookingLeft){
		        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", sneakingLeftBlinkID);
	        }
	        else if (isLookingRight){
		        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", sneakingRightBlinkID);
	        }
	        else{
		        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", sneakingBlink1ID);
	        }
        }
        else{
            if(isLookingLeft){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", baseLeftBlinkID);
            }
            else if(isLookingRight){
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", baseRightBlinkID);
            }
            else{
                this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", baseBlink1ID);
            }
        }
        StartCoroutine(callBase());
    }
    void flipTrance(){
        this.GetComponent<SkinnedMeshRenderer>().material.SetTextureOffset("_BaseMap", TranceFlipBlink1ID);
    }

    IEnumerator callBlink(){
        yield return new WaitForSeconds(.05f);
        Blink();
    }
    IEnumerator callBlinkUp(){
        yield return new WaitForSeconds(.1f);
        BlinkUp();
    }
    IEnumerator callBlinkDown(){
        yield return new WaitForSeconds(Random.Range(blinkTimerLow,blinkTimerHigh));
        BlinkDown();
        isBlinking = true;
    }
    IEnumerator callBase(){
        yield return new WaitForSeconds(.05f);
        isBlinking = false;
        chooseViewDirection();
        Base();
        
    }

    void chooseViewDirection(){
        if(forceLeft){
            isLookingLeft = true;
            isLookingRight = false;
        }
        else if(forceRight){
            isLookingRight = true;
            isLookingLeft = false;
        }
        else if(forceStraight){
            isLookingRight = false;
            isLookingLeft = false;
        }
        else{
            int random = Random.Range(0, 10);
            if(random == 4){
                isLookingLeft = true;
            }
            else if (random == 5){
                isLookingRight = true;
            }
            else{
                isLookingRight = false;
                isLookingLeft = false;
            }
        }
    }

}
