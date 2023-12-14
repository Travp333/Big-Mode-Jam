using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatrolData : MonoBehaviour
{
    [SerializeField] private Vector3[] Points;
    public PatrolType Type;
    public int PatrolIndex;
    private bool reverse = false;
    public Vector3 TargetPoint
    {
        get
        {
            return Points[PatrolIndex];
        }
    }
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
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (Points.Length > 0)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                Gizmos.DrawWireSphere(Points[i], 1);
            }
        }
    }
#endif
    public enum PatrolType
    {
        Loop, Mirror
    }
}
