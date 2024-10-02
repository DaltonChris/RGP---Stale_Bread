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
    public float dashRechargeTime = 10f; // Time in seconds to recharge a dash.

    public Transform groundCheck; // A point used to check if the player is grounded
    public float groundCheckRadius = 0.2f; // Radius of the ground check
    public LayerMask whatIsGround; // Layer to determine what counts as ground

    public int maxDashCharges = 1; // Maximum number of dash charges
    private int currentDashCharges = 0; // Current number of dash charges
    private bool isRecharging = false; // Flag to check if recharging is ongoing

    public float climbSpeed = 5f; // Speed for climbing the ladder
    private bool isOnLadder = false; // Tracks if the player is currently on a ladder
    private bool canDoubleJump = false; // Tracks if the player can double jump

    public TMP_Text dashChargeText; // Reference to the TMP Text UI element

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private bool isDashing;
    private float dashTime;

    public AudioClip dashSound1; 
    public AudioClip dashSound2;
    public AudioClip movementSound; // Sound played when moving left or right
    private AudioSource audioSource; // AudioSource to play sounds


    private bool IsFacingRight = true; // Tracks the player's facing direction, initialize to true (facing right)

    private PlayerAnimations playerAnim;//Reference to animations script

    public Slider HealthBar;
    public int MaxHealth = 100;
    public int Health;
    bool IsDamageable = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); 
        UpdateDashChargeUI(); // Initialize UI with current charge count.
        Health = MaxHealth;
        playerAnim = GetComponent<PlayerAnimations>();
    }


    void Update()
    {
        // Get horizontal input (A/D keys or Left/Right arrow keys)
        moveInput = Input.GetAxis("Horizontal");

        if (isOnLadder)
        {
            // Vertical movement for ladder climbing
            float verticalInput = Input.GetAxis("Vertical"); // W/S or Up/Down arrows
                                                             // Allow movement on both axes
            rb.velocity = new Vector2(moveInput * moveSpeed, verticalInput * climbSpeed);

            // Disable gravity while on the ladder
            rb.gravityScale = 0.1f;
        }
        else
        {
            // Re-enable gravity if not on ladder
            rb.gravityScale = 1f;

            if (!isDashing)
            {
                // Set the velocity for the Rigidbody2D
                rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

                // Play or stop the movement sound
                HandleMovementSound();

                // Check if the player is grounded
                isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

                // Jumping logic
                 if (isGrounded)
                {
                    canDoubleJump = true; // Reset double jump when player is grounded

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                        playerAnim.ChangeAnimation(PlayerAnimations.AnimationState.JUMP);
                    }
                }
                else if (canDoubleJump && Input.GetKeyDown(KeyCode.Space))
                {
                    // Double jump logic
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    canDoubleJump = false; // Disable further double jumps
                    playerAnim.ChangeAnimation(PlayerAnimations.AnimationState.JUMP);

                    // Optional: You can add a sound or effect for the double jump
                    Debug.Log("Double Jump activated!");
                }


                //Uses velocity to determine the general movement animations
                if (isGrounded)
                {
                    if (rb.velocity.x != 0)
                    {
                        playerAnim.ChangeAnimation(PlayerAnimations.AnimationState.RUN);
                    }
                    else
                    {
                        playerAnim.ChangeAnimation(PlayerAnimations.AnimationState.IDLE);
                    }
                }
                else
                {
                    if (rb.velocity.y > 0)
                    {
                        playerAnim.ChangeAnimation(PlayerAnimations.AnimationState.JUMP);
                    }
                    else
                    {
                        playerAnim.ChangeAnimation(PlayerAnimations.AnimationState.FALL);
                    }
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
        
        if (moveInput != 0)
        {
            isDashing = true;
            dashTime = dashDuration;

        
            if (Input.GetKey(KeyCode.W) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                // Dash diagonally upwards 
                rb.velocity = new Vector2(moveInput * dashSpeed, jumpForce);
            }
            else
            {
                // Normal horizontal dash
                rb.velocity = new Vector2(moveInput * dashSpeed, rb.velocity.y);
            }

            // Play one of the two dash sounds randomly
            PlayDashSound();

            currentDashCharges--; // Consume one dash charge
            UpdateDashChargeUI();

            playerAnim.ChangeAnimation(PlayerAnimations.AnimationState.DASH);
        }
    }

    void PlayDashSound()
    {
        // Randomly select one sound and plays on dash
        AudioClip selectedDashSound = Random.Range(0, 2) == 0 ? dashSound1 : dashSound2;
        audioSource.PlayOneShot(selectedDashSound);
    }

    void HandleMovementSound()
    {
        // Check if the player is moving and on grounded
        if (Mathf.Abs(moveInput) > 0 && isGrounded && !isOnLadder)
        {
            // If the movement sound is not already playing, play it
            if (!audioSource.isPlaying)
            {
                audioSource.loop = true; // Set looping for continuous movement sound
                audioSource.clip = movementSound;
                audioSource.Play();
            }
        }
        else
        {
            // If the player stops moving or is not on the ground, stop the sound
            if (audioSource.isPlaying && audioSource.clip == movementSound)
            {
                audioSource.Stop();
            }
        }
    }


    void EndDash()
    {
        isDashing = false;

        playerAnim.ChangeAnimation(PlayerAnimations.AnimationState.FALL);
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

        if (other.CompareTag("Ladder"))
        {
            isOnLadder = true; // Player is now on the ladder
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isOnLadder = false; // Player is off the ladder
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
        dashChargeText.text = "BUTTER x " + currentDashCharges;
    }

    public void TakeDamage(int dmg)
    {
        StartCoroutine(DamageTimer());
        if (!IsDamageable) { return; }
        IsDamageable = false;
        Health -= dmg;
        HealthBar.value = Health;
    }

    IEnumerator DamageTimer()
    {
        
        yield return new WaitForSeconds(1f);
        IsDamageable = true;
    }
}
