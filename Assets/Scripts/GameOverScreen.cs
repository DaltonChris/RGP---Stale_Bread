using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public TMP_Text scoreText; // Reference to the TMP Text UI element
    public TMP_Text breadText;

    private void Start()
    {
        scoreText.text = "SCORE:" + ScoreManager.finalScore;
        breadText.text = "BREAD COLLECTED:" + ScoreManager.finalBreads;
    }
}
