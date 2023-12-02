using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public string tempTagName;
    private void OnTriggerEnter(Collider other)
    {
        // Need proper trigger on only enemies
        if (other.gameObject.tag == tempTagName)
        {
            transform.parent.gameObject.GetComponent<EntityTrap>().ActivateTrap(other.gameObject);
        }
    }
}
