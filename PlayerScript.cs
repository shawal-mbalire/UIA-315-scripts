using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [Header("Player Health Things")]
    private float playerHealth = 1000f;
    private float presentHealth;
    public HealthBar healthBar;

    [Header("Player Movement")]
    public float playerSpeed = 1.9f;
    public float currentPlayerSpeed = 0f;
    public float playerSprint = 3f;
    public float currentPlayerSprint = 0f;

    [Header("Player Animator Gravity")]
    public CharacterCOntroller cC;
    public float gravity = -9.81f;
    public Animator animator;

    [Header("Player Jumping & velocity")]
    public float jumpRange = 1f;
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    Vector3 velocity;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        presentHealth = playerHealth;
        healthBar.GiveFullHealth(playerHealth);
    }

    private void Update()
    {
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

        if (onSurface && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //gravity
        velocity.y += gravity * turnCalmTime.deltaTime;
        cC.Move(velocity * turnCalmTime.deltaTime);

        playerMove();

        Jump();

        Sprint();
    }
    void playerMove()
    {
        // Here we are taking the input for player movement
        float hortizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        //assigning the direction of the player movement
        Vector3 direction = new Vector3(hortizontal_axis, 0f, vertical_axis).normalized;

        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("Walk", true);
            animator.SetBool("Running", false);
            animator.SetBool("Idle", false);
            animator.SetTrigger("Jump");
            animator.SetBool("AimWalk", false);
            animator.SetBool("IdleAim", false);
            // to rotate player at the camera angle and make him to move
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, trunCalmTime);
            //smoothing the turning of player
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //moving the player with the help of chacacter controllerx
            characterController.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);

            //updating speed
            currentPlayerSpeed = playerSpeed;
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetTrigger("Jump");
            animator.SetBool("Walk", false);
            animator.SetBool("Running", false);
            animator.SetBool("AimWalk", false);
            currentPlayerSpeed = 0f;
        }

    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && onSurface)
        {
            animator.SetBool("Walk", false);
            animator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
        }
        else
        {
            animator.ResetTrigger("Jump");
        }
    }

    void Sprint()
    {
        if (Input.GetButton("Sprint") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && onSurface)
        {
            // Here we are taking the input for player movement
            float hortizontal_axis = Input.GetAxisRaw("Horizontal");
            float vertical_axis = Input.GetAxisRaw("Vertical");

            //assining the direction of the player movement
            Vector3 direction = new Vector3(hortizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Running", true);
                animator.SetBool("Walk", false);
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", false);
                // to rotate player at the camera angle and make him to move
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, trunCalmTime);
                //smoothing the turning of player
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                //moving the player with the help of chacacter controllerx
                characterController.Move(moveDirection.normalized * playerSprint * Time.deltaTime);

                //updating speed
                currentPlayerSprint = playerSprint;
            }
            else
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
                currentPlayerSprint = 0f;
            }
        }

    }
    // player hit damage
    public void playerHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthBar.SetHealth(presentHealth);
        if (presentHealth<=0)
        {
            playerDie();
        }
    }

    //player die
    private void PlayerDie()
    {
        Cursor.lockState = CursorLockMode.None;

        ObjectComparer.Destroy(gameObject);
    }
}
