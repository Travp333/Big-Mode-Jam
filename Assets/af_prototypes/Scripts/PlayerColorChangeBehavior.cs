using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorChangeBehavior : MonoBehaviour
{
    public static PlayerColorChangeBehavior Instance;
    public CanvasManager Canvas;

    public MeshRenderer mesh;
    public bool IsBlack { get; private set; } = true; // true for black, false for white
    public bool IsChanging{ get; private set; } = false; // true for black, false for white
    public float ColorTransitionTime = 0.3f;

    float _colorChangeTimer = 0;

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
        if (_colorChangeTimer > 0) _colorChangeTimer -= Time.deltaTime;
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ChangeColor();
            }
        }

    }
    private void ChangeColor()
    {
        IsChanging = true;
        Invoke("StopChanging", ColorTransitionTime);
        _colorChangeTimer = ColorTransitionTime;
        StartCoroutine(Canvas.ShowColorChangeIndicator(ColorTransitionTime));
        IsBlack = !IsBlack;
        mesh.material = IsBlack ? SceneData.Instance.Black : SceneData.Instance.White;
    }
    void StopChanging()
    {
        IsChanging = false;
    }
}
