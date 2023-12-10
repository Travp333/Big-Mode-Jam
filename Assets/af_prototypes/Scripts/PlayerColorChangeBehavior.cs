using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerColorChangeBehavior : MonoBehaviour
{
	public SkinnedMeshRenderer face;
    public static PlayerColorChangeBehavior Instance;
    public CanvasManager Canvas;
	public InputAction colorSwap;
	public SkinnedMeshRenderer[] mesh;
    public bool IsBlack { get; private set; } = false; // true for black, false for white
    public bool IsChanging{ get; private set; } = false; // true for black, false for white
    public float ColorTransitionTime = 0.3f;

    float _colorChangeTimer = 0;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else
        {
            Debug.LogError("There can't be multiple instances of " + this.GetType().FullName);
            Destroy(this);
        }
	    colorSwap = GetComponent<PlayerInput>().currentActionMap.FindAction("ColorSwap");
    }
    private void Update()
	{
		//Debug.Log(IsBlack);
        if (_colorChangeTimer > 0) _colorChangeTimer -= Time.deltaTime;
        else
        {
	        if (colorSwap.WasPressedThisFrame())
            {
                ChangeColor();
            }
        }

    }
    private void ChangeColor()
    {
        IsChanging = true;
        Invoke("StopChanging", ColorTransitionTime);
        _colorChangeTimer = ColorTransitionTime;
        if (Canvas) StartCoroutine(Canvas.ShowColorChangeIndicator(ColorTransitionTime));
	    IsBlack = !IsBlack;
	    foreach (SkinnedMeshRenderer m in mesh){
	    	Material[] m2 = m.sharedMaterials;
	    	if(m){
	    		if(IsBlack){
	    			face.material = SceneData.Instance.faceWhite;
	    			m2[0] = SceneData.Instance.Black;
	    			m2[1] = SceneData.Instance.White;
	    		}
	    		else{
	    			face.material = SceneData.Instance.faceBlack;
		    		m2[1] = SceneData.Instance.Black;
	    			m2[0] = SceneData.Instance.White;
	    		}
	    		m.sharedMaterials = m2;
	    	}
	    	
		    //if (m) m.sharedMaterials[0] = IsBlack ? SceneData.Instance.Black : SceneData.Instance.White;
		    //if (m) m.sharedMaterials[1] = IsBlack ? SceneData.Instance.White : SceneData.Instance.Black;
	    }
    }
    void StopChanging()
    {
        IsChanging = false;
    }
}
