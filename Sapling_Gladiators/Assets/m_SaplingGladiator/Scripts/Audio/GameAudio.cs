using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class GameAudio : MonoBehaviour
{
    public void PlayFModOneShot(string eventName)
    {
        RuntimeManager.PlayOneShot(eventName);
    }

    public void PlayFModOneShot(string eventName, Vector3 position)
    {
        RuntimeManager.PlayOneShot(eventName, position);
        
    }
}
