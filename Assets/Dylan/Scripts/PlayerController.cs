using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of the player movement
    public float jumpForce = 10f; // Force applied when the player jumps
    public float dashSpeed = 20f; // Speed of the dash
    public float dashDuration = 0.2f; // Duration of the dash
    public float dashRechargeTime = 10f; // Time in seconds to recharge a dash
    public Transform groundCheck; // A point used to check if the player is grounded
    public float groundCheckRadius = 0.2f; // Radius of the ground check
    public LayerMask whatIsGround; // Layer to determine what counts as ground

    public int maxDashCharges = 1; // Maximum number of dash charges
    private int currentDashCharges = 0; // Current number of dash charges
    private bool isRecharging = false; // Flag to check if recharging is ongoing

    public TMP_Text dashChargeText; // Reference to the TMP Text UI element

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private bool isDashing;
    private float dashTime;

    private bool IsFacingRight = true; // Tracks the player's facing direction, initialize to true (facing right)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        UpdateDashChargeUI(); // Initialize UI with current charge count.
    }

    void Update()
    {
        // Get horizontal input (A/D keys or Left/Right arrow keys)
        moveInput = Input.GetAxis("Horizontal");

        if (!isDashing)
        {
            // Set the velocity for the Rigidbody2D
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            // Check if the player is grounded
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

            // Jumping logic
            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            // Dash input logic
            if (currentDashCharges > 0 && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
            {
                StartDash();
            }
        }
        else
        {
            // Continue dashing
            dashTime -= Time.deltaTime;

            if (dashTime <= 0)
            {
                EndDash();
            }
        }

        // Start recharging if no charges are available and not already recharging
        if (currentDashCharges == 0 && !isRecharging)
        {
            StartCoroutine(RechargeDash());
        }

        Turncheck(); // Check if the player needs to turn
    }

    private void Turncheck()
    {
        if (Input.GetAxis("Horizontal") > 0 && !IsFacingRight)
        {
            Turn();
        }
        else if (Input.GetAxis("Horizontal") < 0 && IsFacingRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        // Flip the player
        IsFacingRight = !IsFacingRight;
        Vector3 rotator = transform.rotation.eulerAngles;
        rotator.y = IsFacingRight ? 0f : 180f;
        transform.rotation = Quaternion.Euler(rotator);
    }

    void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        rb.velocity = new Vector2(moveInput * dashSpeed, rb.velocity.y);
        currentDashCharges--; // Consume one dash charge
        UpdateDashChargeUI();
    }

    void EndDash()
    {
        isDashing = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DashFuel"))
        {
            if (currentDashCharges < maxDashCharges)
            {
                currentDashCharges++; // Add a dash charge
                Destroy(other.gameObject); // Remove the fuel source after collecting
                UpdateDashChargeUI();
            }
        }
    }

    IEnumerator RechargeDash()
    {
        isRecharging = true;
        yield return new WaitForSeconds(dashRechargeTime);
        if (currentDashCharges < maxDashCharges)
        {
            currentDashCharges++; // Regenerate one dash charge
            UpdateDashChargeUI(); // Update the UI after regenerating a charge
        }
        isRecharging = false;
    }

    void UpdateDashChargeUI()
    {
        dashChargeText.text = "Butter Dashes: " + currentDashCharges;
    }
}
