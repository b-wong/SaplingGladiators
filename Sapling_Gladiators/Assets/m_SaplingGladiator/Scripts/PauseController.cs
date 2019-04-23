using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class PauseController : MonoBehaviour
{

    public static PauseController pauseController;
    public GameObject blackOverlay;
    private VolumeController volumeController;
    private FMOD.Studio.Bus masterBus;
    public bool paused = false;
    public bool allowPause = true;
    private float pauseWaitTime = 0.2f;

    private Transform targetTransform;

    // Start is called before the first frame update
    void Start()
    {
        if (pauseController == null)
        {
            pauseController = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded+= OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Cancel") != 0)
        {
            if (allowPause)
            {
                TogglePause();
                StartCoroutine(WaitForPause());
            }
        }
        if (targetTransform)
        {
            gameObject.transform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, -2f);
        }
    }

    public void TogglePause()
    {
        paused = !paused;
        if (paused)
        {
            RuntimeManager.PlayOneShot("event:/UI Pause Start");
            Time.timeScale = 0;
            if (blackOverlay)
            {
                blackOverlay.SetActive(true);
            }
            if (masterBus.isValid())
            {
                masterBus.setPaused(true);
            }
        }
        else
        {
            Time.timeScale = 1;
            RuntimeManager.PlayOneShot("event:/UI Pause Stop");
            if (blackOverlay)
            {
                blackOverlay.SetActive(false);
            }
            if (volumeController != null)
            {
                masterBus.setPaused(false);
            }
        }
    }

    public IEnumerator WaitForPause()
    {
        allowPause = false;
        yield return new WaitForSecondsRealtime(pauseWaitTime);
        allowPause = true;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (volumeController != null)
        {
            volumeController = GameObject.FindObjectOfType<VolumeController>();
            if (volumeController != null)
            {
                masterBus = volumeController.MasterBus;
            }
        }
        targetTransform = Camera.main.transform;
    }

}
