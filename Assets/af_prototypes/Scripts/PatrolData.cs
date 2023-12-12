using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class PatrolData : MonoBehaviour
{
    public Vector3[] Points;
    public PatrolType Type;
    public int PatrolIndex;
    private bool reverse = false;
    public void SetNextPatrolPoint()
    {
        PatrolIndex += reverse ? -1 : 1;
        if (PatrolIndex > Points.Length - 1 || PatrolIndex < 0)
        {
            if (Type == PatrolType.Loop)
            {
                PatrolIndex = 0;
            }
            else if (Points.Length > 1)
            {
                PatrolIndex = reverse ? 1 : Points.Length - 2; // get the second to last index
                reverse = !reverse;
            } else
            {
                PatrolIndex = 0;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Points.Length > 0)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                Gizmos.DrawWireSphere(Points[i], 1);
	            //Handles.Label(Points[i] + Vector3.up, "Point " + (i + 1));
            }
        }
    }
    public enum PatrolType
    {
        Loop, Mirror
    }
}
