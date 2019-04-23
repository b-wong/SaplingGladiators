using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class VolumeController : MonoBehaviour
{
    public FMOD.Studio.Bus MasterBus;
    public FMOD.Studio.Bus MusicBus;
    public FMOD.Studio.Bus SFXBus;
    public FMOD.Studio.Bus UIBus;
    public FMOD.Studio.Bus DialogueBus;
    public FMOD.Studio.Bus EnvironmentBus;

    public float MasterVolume = 1f;
    public float MusicVolume = 0.5f;
    public float SFXVolume = 0.5f;
    public float UIVolume = 0.5f;
    public float DialogueVolume = 0.5f;
    public float EnvironmentVolume = 0.5f; 

    // Start is called before the first frame update
    void Start()
    {
        MasterBus = RuntimeManager.GetBus("bus:/MasterVol");
        MusicBus = RuntimeManager.GetBus("bus:/MasterVol/Music");
        SFXBus = RuntimeManager.GetBus("bus:/MasterVol/SFX");
        UIBus = RuntimeManager.GetBus("bus:/MasterVol/UI");
        DialogueBus = RuntimeManager.GetBus("bus:/MasterVol/Dialogue");
        EnvironmentBus = RuntimeManager.GetBus("bus:/MasterVol/Environment");
    }

    // Update is called once per frame
    void Update()
    {
        MasterBus.setVolume(MasterVolume);
        MusicBus.setVolume(MusicVolume);
        SFXBus.setVolume(SFXVolume);
        UIBus.setVolume(UIVolume);
        DialogueBus.setVolume(DialogueVolume);
        EnvironmentBus.setVolume(EnvironmentVolume);
    }

    public void MasterVolumeLevel(float level)
    {
        MasterVolume = level;
    }

    public void MusicLevel(float level)
    {
        MusicVolume = level;
    }

    public void SFXLevel(float level)
    {
        SFXVolume = level;
    }

    public void UILevel(float level)
    {
        UIVolume = level;
    }

    public void DialogueLevel(float level)
    {
        DialogueVolume = level;
    }

    public void EnvironmentLevel(float level)
    {
        EnvironmentVolume = level;
    }

    public void MuteAll(bool value)
    {
        MasterBus.setMute(value);
    }
}
