using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChecker : MonoBehaviour
{
	[SerializeField]
	SkinnedMeshRenderer[] mesh;
	[SerializeField]
	SkinnedMeshRenderer face;
	PlayerColorChangeBehavior color;
	bool localIsBlack;
	bool gate = true; // true  = closed
    // Start is called before the first frame update
    void Start()
    {
	    color = GameObject.FindFirstObjectByType<PlayerColorChangeBehavior>();
    }

	void UpdateMeshes(){
		localIsBlack = color.IsBlack;
		foreach (SkinnedMeshRenderer m in mesh){
			Material[] m2 = m.sharedMaterials;
			if(m){
				if(color.IsBlack){
					face.material = SceneData.Instance.faceWhite;
					//color.updateFaceExpression();
					m2[0] = SceneData.Instance.Black;
					m2[1] = SceneData.Instance.White;
				}
				else{
					face.material = SceneData.Instance.faceBlack;
					//color.updateFaceExpression();
					m2[1] = SceneData.Instance.Black;
					m2[0] = SceneData.Instance.White;
				}
				m.sharedMaterials = m2;
			}
	    	
			//if (m) m.sharedMaterials[0] = IsBlack ? SceneData.Instance.Black : SceneData.Instance.White;
			//if (m) m.sharedMaterials[1] = IsBlack ? SceneData.Instance.White : SceneData.Instance.Black;
		}
		gate = true;
	}
    // Update is called once per frame
    void Update()
    {
	    if(!gate){
	    	UpdateMeshes();
	    }
	    else{
	    	if(localIsBlack != color.IsBlack){
	    		Debug.Log("Color Changed!");
	    		gate = false;
	    	}
	    }
    }
}
