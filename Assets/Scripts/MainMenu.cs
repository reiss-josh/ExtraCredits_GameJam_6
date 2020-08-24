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
        StartCoroutine(PlayGameRoutine());
    }

    public IEnumerator PlayGameRoutine()
    {
        yield return new WaitForSeconds(0.25f);
        Debug.Log("loading Scene");
        SceneManager.LoadScene(1);
    }

    public IEnumerator QuitGameRoutine()
    {
        yield return new WaitForSeconds(0.25f);
        Debug.Log("quit!");
        Application.Quit();
    }

    public void QuitGame()
    {
        StartCoroutine(QuitGameRoutine());
    }

    public void OptionsMenu()
    {
        GetComponent<AudioSource>().Play();
    }
}
