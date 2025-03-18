using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;
using System.Runtime.InteropServices;
using System;

public class FMODSceneManager : MonoBehaviour
{
    public string fmodEventPath; // Path to the FMOD event
    private FMOD.Studio.EventInstance eventInstance;

    public Button button;

    void Start()
    {
        // Start the FMOD event when the scene starts
        StartFMODEvent();
    }

    void StartFMODEvent()
    {
        // Create the FMOD event instance
        eventInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEventPath);

        // Start the event
        eventInstance.start();
    }

    void OnEnable()
    {
        // Register the callback to handle scene changes
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unregister the scene loaded callback when this object is disabled
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This method is called every time a new scene is loaded
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // Stop the FMOD event when the scene changes
        StopFMODEvent();
    }

    void StopFMODEvent()
    {
        // Stop the FMOD event if it is playing
        if (eventInstance.isValid())
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }


}
