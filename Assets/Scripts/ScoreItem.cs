using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    public int scoreValue = 10; // The amount of score this item gives
    public AudioClip ItemPickUpSFX;
    public AudioSource audioSource;
    // When the player collides with the item, add the score and destroy the item
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Find the ScoreManager and add the score
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            SfxManager.Instance.PlaySfx(ItemPickUpSFX);
            if (scoreManager != null)
            {
                scoreManager.AddScore(scoreValue);
                scoreManager.AddBread();
            }
            
            // Destroy the item
            Destroy(gameObject);
        }
    }
}
