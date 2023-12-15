using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Scene GameScene;
    public void StartGame()
    {
        SceneManager.LoadScene(GameScene.name);
    }
    public void Quit()
    {
#if UNITY_EDITOR
        Debug.Break();
#else
        Application.Quit();
#endif
    }
}
