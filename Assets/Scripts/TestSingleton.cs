using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSingleton : MonoBehaviour
{
    public static TestSingleton Instance;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else
        {
            Debug.LogError("There can't be multiple instances of " + this.name);
            Destroy(this);
        }
    }
}
