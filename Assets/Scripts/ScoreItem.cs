using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    public int scoreValue = 1; // The amount of score this item gives

    // When the player collides with the item, add the score and destroy the item
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Find the ScoreManager and add the score
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.AddScore(scoreValue);
            }

            // Destroy the item
            Destroy(gameObject);
        }
    }
}
