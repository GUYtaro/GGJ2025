using UnityEngine;
using UnityEngine.UI;

public class FirstPersonMovement : MonoBehaviour
{
    public Animator animator;
    public Transform cameraTransform; // Reference for camera position and rotation
    public float moveSpeed = 5f;      // Movement speed
    public float verticalSpeed = 3f; // Speed for moving up/down
    public float rotationSpeed = 300f; // Rotation speed

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrain = 10f;
    public float staminaRegen = 5f;
    public float regenDelay = 2f;

    [Header("UI")]
    public Scrollbar staminaBar;

    [Header("Audio Settings")]
    public AudioSource movementSound;   // AudioSource for movement
    public AudioSource stopSound;       // AudioSource for stop sound

    private float regenTimer;
    private float verticalRotation = 0f; // Vertical rotation of the camera
    private bool isMoving = false;       // Tracks if the player is moving

    void Start()
    {
        // Initialize stamina
        currentStamina = maxStamina;

        // Hide and lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();      // Handle player movement
        HandleMouseRotation(); // Handle camera rotation
        RegenerateStamina();   // Regenerate stamina
        UpdateStaminaBar();    // Update the stamina bar
    }

    void HandleMovement()
    {
        // Get movement input
        float moveX = Input.GetAxis("Horizontal"); // Left/Right
        float moveZ = Input.GetAxis("Vertical");   // Forward/Backward
        float moveY = 0f;

        // Handle vertical movement
        if (Input.GetKey(KeyCode.Space)) moveY = verticalSpeed;        // Move up
        if (Input.GetKey(KeyCode.LeftShift)) moveY = -verticalSpeed;   // Move down

        // Calculate movement vector
        Vector3 movement = (cameraTransform.right * moveX + cameraTransform.forward * moveZ + Vector3.up * moveY).normalized;

        // Handle stamina and movement
        if (movement.magnitude > 0.1f && currentStamina > 0)
        {
            currentStamina -= staminaDrain * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            regenTimer = 0f;

            // Move the player
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

            // Play movement animation
            animator.SetBool("swim", true);
            animator.SetBool("idle", false);

            // Play movement sound
            if (!isMoving)
            {
                isMoving = true;
                PlayMovementSound();
            }
        }
        else
        {
            // Stop movement animation
            animator.SetBool("swim", false);
            animator.SetBool("idle", true);

            // Stop movement sound
            if (isMoving)
            {
                isMoving = false;
                StopMovementSound();
            }
        }
    }

    void HandleMouseRotation()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // Rotate the player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
        cameraTransform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);
    }

    void RegenerateStamina()
    {
        if (currentStamina < maxStamina && regenTimer >= regenDelay)
        {
            currentStamina += staminaRegen * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
        else if (currentStamina < maxStamina)
        {
            regenTimer += Time.deltaTime;
        }
    }

    void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.size = currentStamina / maxStamina;
        }
    }

    void PlayMovementSound()
    {
        if (movementSound != null && !movementSound.isPlaying)
        {
            movementSound.loop = true; // Ensure the sound loops while moving
            movementSound.Play();
        }
    }

    void StopMovementSound()
    {
        if (movementSound != null && movementSound.isPlaying)
        {
            movementSound.loop = false; // Stop looping
            movementSound.Stop();

            // Play stop sound if assigned
            if (stopSound != null)
            {
                stopSound.Play();
            }
        }
    }
}
