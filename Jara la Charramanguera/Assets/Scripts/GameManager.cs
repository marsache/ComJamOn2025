using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //public AudioSource music;
    //public bool startPlaying;
    //public BeatScroller beatScroller;

    public static GameManager instance;

    public int currentScore;
    public int scorePerNote = 5;
    public int scorePerGoodNote = 10;
    public int scorePerPerfectNote = 15;
    public int scorePerMissedNote = -5;

    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;

    public Text scoreText;
    public Text multiText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        scoreText.text = "Puntuación: 0";
        currentMultiplier = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!startPlaying)
        //{
        //    if (Input.anyKeyDown)
        //    {
        //        startPlaying = true;

        //        music.Play();
        //    }
        //}
    }

    public void NoteHit()
    {
        if (currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;

            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }

        multiText.text = "Multiplicador: x" + currentMultiplier;

        //currentScore += scorePerNote * currentMultiplier;
        scoreText.text = "Puntuación: " + currentScore;
    }

    public void NormalHit()
    {
        currentScore += scorePerNote * currentMultiplier;
        NoteHit();
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote * currentMultiplier;
        NoteHit();
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote * currentMultiplier;
        NoteHit();
    }

    public void NoteMissed()
    {
        currentMultiplier = 1;
        multiplierTracker = 0;

        currentScore += scorePerMissedNote;

        multiText.text = "Multiplicador: x" + currentMultiplier;
        scoreText.text = "Puntuación: " + currentScore;
    }
}
