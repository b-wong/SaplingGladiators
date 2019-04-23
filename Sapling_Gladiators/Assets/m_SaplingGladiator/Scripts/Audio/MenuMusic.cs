using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MenuMusic : MonoBehaviour
{
    public static MenuMusic menuMusic;
    public StudioEventEmitter musicEmitter;

    // Start is called before the first frame update
    void Start()
    {
        if (menuMusic == null)
        {
            menuMusic = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "ClassicGame" || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "CustomGame")
        {
            Destroy(gameObject);
        }
    }
}
