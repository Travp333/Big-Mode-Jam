using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Travis Parks
//This script controls the speed that the conveyor belt animation plays
public class BeltSpeedController : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    float speed;
    // Start is called before the first frame update
    public void reverseDirection(){
        speed = -speed;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", speed);
    }
}
