using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
}
