using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorChangeBehavior : MonoBehaviour
{
    public static PlayerColorChangeBehavior Instance;

    public MeshRenderer mesh;
    public bool IsBlack { get; private set; } = true; // true for black, false for white
    public Material CurrentMaterial => IsBlack ? SceneData.Instance.Black : SceneData.Instance.White;



    private void Awake()
    {
        if (!Instance) Instance = this;
        else
        {
            Debug.LogError("There can't be multiple instances of " + this.GetType().FullName);
            Destroy(this);
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            IsBlack = !IsBlack;
            mesh.material = CurrentMaterial;
        }
    }
}
