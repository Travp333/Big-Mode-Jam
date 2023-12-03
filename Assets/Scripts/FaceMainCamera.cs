using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMainCamera : MonoBehaviour
{
    public RectTransform rect;
    Transform _camTransform;
    private void Awake()
    {
        _camTransform = Camera.main.transform;
    }
    void Update()
    {
        if (rect) rect.LookAt(_camTransform.position);
        else if (_camTransform) _camTransform.LookAt(_camTransform.position);
        transform.Rotate(0, 180, 0);
    }
}
