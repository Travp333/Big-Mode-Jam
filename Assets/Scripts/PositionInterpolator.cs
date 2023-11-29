using UnityEngine;

//moves a thing from one spot to another 
public class PositionInterpolator : MonoBehaviour {
    [SerializeField]
	Transform relativeTo = default;

	[SerializeField]
	Rigidbody body = default;
	
	[SerializeField]
	Vector3 from = default, to = default;
	
	public void Interpolate (float t) {
		Vector3 p;
		if (relativeTo) {
			p = Vector3.LerpUnclamped(
				relativeTo.TransformPoint(from), relativeTo.TransformPoint(to), t
			);
		}
		else {
			p = Vector3.LerpUnclamped(from, to, t);
		}
		body.MovePosition(p);
	}
}
