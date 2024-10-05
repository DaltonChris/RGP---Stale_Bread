using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score = 0; // The player's score
    public TMP_Text breadText; // Reference to the TMP Text UI element
    public static int finalBreads;
    public TMP_Text scoreText;
    public static int finalScore = 0;
    int collectedBread= 0;

    // Updates the UI on start
    private void Start()
    {
        UpdateScoreUI();
    }

    // Method to increment the score and update the UI
    public void AddScore(int amount)
    {
        score += amount;
        finalScore = score;
        UpdateScoreUI();
    }

    public void AddBread()
    {
        collectedBread++;
        finalBreads = collectedBread;
    }

    // Method to update the UI with the current score
    private void UpdateScoreUI()
    {
        breadText.text = "BREAD: " + collectedBread;
        scoreText.text = "SCORE: " + score;
    }
}
