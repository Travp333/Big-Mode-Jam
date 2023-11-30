using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JTempPlayerController : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(h, 0f, v).normalized;
        if (direction.magnitude >= 0.1f)
        {
            transform.LookAt(transform.position + direction, transform.up);
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
