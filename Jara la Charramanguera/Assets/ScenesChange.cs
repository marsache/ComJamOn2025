using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesChange : MonoBehaviour
{
    private FMOD.Studio.EventInstance eventInstance;
    public string eventName;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        //eventInstance = FMODUnity.RuntimeManager.CreateInstance("event:/" + eventName);
        eventInstance = gameManager.GetComponent<BeatSystem>().getEventInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreditsScene()
    {
        SceneManager.LoadScene(2);
    }

    public void PlayScene()
    {
        SceneManager.LoadScene(1);
    }

    public void TitleScene()
    {
        SceneManager.LoadScene(0);
    }

    public void Huerto()
    {
        SceneManager.LoadScene(5);
    }
    public void Jota()
    {
        StopFMODEvent();
        SceneManager.LoadScene(1);
    }

    public void Car()
    {
        SceneManager.LoadScene(6);
    }

    public void AR()
    {
        SceneManager.LoadScene(3);
    }

    public void FDI()
    {
        SceneManager.LoadScene(7);
    }

    public void John()
    {
        SceneManager.LoadScene(4);
    }

    public void End()
    {
        SceneManager.LoadScene(8);
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
