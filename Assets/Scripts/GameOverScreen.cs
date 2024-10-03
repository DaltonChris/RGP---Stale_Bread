using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public TMP_Text scoreText; // Reference to the TMP Text UI element


    private void Start()
    {
        scoreText.text = "SCORE:" + ScoreManager.finalScore;
    }
}
