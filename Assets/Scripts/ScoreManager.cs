using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score = 0; // The player's score
    public TMP_Text scoreText; // Reference to the TMP Text UI element

    // Updates the UI on start
    private void Start()
    {
        UpdateScoreUI();
    }

    // Method to increment the score and update the UI
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    // Method to update the UI with the current score
    private void UpdateScoreUI()
    {
        scoreText.text = "BREAD: " + score;
    }
}
