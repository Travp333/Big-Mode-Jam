/*
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FaceTexController))]
public class customInspector : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FaceTexController face = (FaceTexController)target;
        if(GUILayout.Button("Set Happy")){
            face.setHappy();
        }
	    if(GUILayout.Button("Set Straining")){
	        face.setStraining();
        }
        if(GUILayout.Button("Set Base")){
            face.setBase();
        }
        if(GUILayout.Button("Set Scared")){
            face.setScared();
        }
	    if(GUILayout.Button("Set Holding Breath")){
	        face.setHoldingBreath();
        }
	    if(GUILayout.Button("Set Aiming")){
		    face.setAiming();
        }
	    if(GUILayout.Button("Set Trance")){
		    face.setTrance();
	    }
	    if(GUILayout.Button("Set Sneaking")){
		    face.setSneaking();
	    }
    }
}
*/