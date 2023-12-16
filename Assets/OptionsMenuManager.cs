using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class OptionsMenuManager : MonoBehaviour
{

    public GameObject SettingsMenu;
    public SimpleCameraMovement CameraMovement;
    public Slider FPSensSlider;
    public TMPro.TMP_Text FPSensAmountText;

    public OrbitCamera TPCameraMovement;
    public Slider TPSensSlider;
    public TMPro.TMP_Text TPSensAmountText;

    public delegate void UnstuckAction();
    public static event UnstuckAction Unstuck;

    public PlayerInput map;
    InputAction PauseAction;

    public static bool paused = false;

    private void OnEnable()
    {
        PauseAction.performed += Pause;
    }
    private void OnDisable()
    {
        PauseAction.performed -= Pause;
    }

    private void Awake()
    {
        SettingsMenu.SetActive(false);
        PauseAction = map.currentActionMap.FindAction("Pause");
        FPSensSlider.value = CameraMovement.sens;
        FPSensAmountText.text = FPSensSlider.value.ToString();

        TPSensSlider.value = TPCameraMovement.rotationSpeed;
        TPSensAmountText.text = TPSensSlider.value.ToString();
    }
    public void ChangeFirstPersonSensitivity(float value)
    {
        CameraMovement.sens = value;
        FPSensAmountText.text = value.ToString();
    }
    public void ChangeThirdPersonSensitivity(float value)
    {
        TPCameraMovement.rotationSpeed = value;
        TPSensAmountText.text = value.ToString();
    }
    public void UnstuckEverything()
    {
        Unstuck?.Invoke();
    }
    public void Pause(InputAction.CallbackContext callbackContext)
    {
        paused = !paused;
        if (paused)
        {
            SettingsMenu.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else
        {
            SettingsMenu.SetActive(false);
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}
