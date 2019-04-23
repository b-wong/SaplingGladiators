using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerSounds : MonoBehaviour
{

    public Player myPlayer;
    public GameObject blockSoundEmitter;

    // Start is called before the first frame update
    void Start()
    {
        if (myPlayer == null)
        {
            myPlayer = GetComponentInParent<Player>();
        }
    }

    public void PlayFModOneShot(string eventName)
    {
        RuntimeManager.PlayOneShot(eventName);
    }

    public void PlayFModOneShot(string eventName, Vector3 position)
    {
        RuntimeManager.PlayOneShot(eventName, position);
    }
}
