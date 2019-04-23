using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class MainMenu : MonoBehaviour
{

    public void playMenu()
    {
        SceneManager.LoadScene("PlayMenu");
    }

    public void optionMenu()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void exitMenu()
    {
        Application.Quit();
    }

    public void attributionMenu()
    {
        SceneManager.LoadScene("AttributionsMenu");
    }

}
