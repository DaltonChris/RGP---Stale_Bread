using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float detectionRadius = 0.6f; // Radius for detecting player
    public LayerMask playerLayer; // Layer for the player

    private void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (hit != null)
        {
            // If player detected, increment the coin counter and destroy the coin
            StefanPlayer player = hit.GetComponent<StefanPlayer>();
            if (player != null)
            {
                player.IncrementCoinCounter();
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the detection radius in the editor for debugging
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
