using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Object playScene;

    void Start()
    {
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(playScene.name);
    }

    public void QuitGame()
    {
        Debug.Log("quit!");
        Application.Quit();
    }
}
