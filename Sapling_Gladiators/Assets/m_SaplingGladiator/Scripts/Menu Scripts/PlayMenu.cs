using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{
    public Toggle toggle;

    public bool tutorialButton;

    public void classic()
    {
        SceneManager.LoadScene("ClassicGame");
    }

    public void custom()
    {
        SceneManager.LoadScene("CustomMenu");
    }

    public void back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void tutorial()
    {
        if (toggle.isOn == true){
            tutorialButton = true;
        }
        else
        {
            tutorialButton = false;
        }
    }

}
