using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class TemporaryPlayerMovement : MonoBehaviour
{
    public float Speed;
    public float RotationSensitivity = 5;

    Vector2 _inputs;
    float _rotationInput;
    Vector3 _velocity;
    Rigidbody _rigid;
    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        _inputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _rotationInput = Input.GetAxis("Mouse X") * RotationSensitivity;

        transform.Rotate(0,_rotationInput * Time.unscaledDeltaTime, 0);

        _velocity.y = _rigid.velocity.y;
        _velocity.x = _inputs.x * Speed;
        _velocity.z = _inputs.y * Speed;

        _velocity = Quaternion.AngleAxis(transform.localEulerAngles.y, Vector3.up) * _velocity;
        _rigid.velocity = _velocity; // Rigidbody velocity is framerate independent
    }
}
