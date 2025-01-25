using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;
    public float maxSpeed = 5f;
    public float speed;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrain = 10f;
    public float staminaRegen = 5f;
    public float regenDelay = 2f;

    private float regenTimer;
    public float minSpeedMultiplier = 0.5f;

    [Header("UI")]
    public Scrollbar staminaBar;

    [Header("Rotation Settings")]
    public float rotationSpeed = 10f;

    private bool isMoving;

    void Start()
    {
        currentStamina = maxStamina;
        speed = maxSpeed;
    }

    void Update()
    {
        // รับ Input การเคลื่อนที่
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float moveY = 0f;

        if (Input.GetKey(KeyCode.Space)) moveY = 1f;       // ลอยขึ้น
        if (Input.GetKey(KeyCode.LeftShift)) moveY = -1f;  // ลอยลง

        Vector3 movement = new Vector3(moveX, moveY, moveZ);

        if (movement.sqrMagnitude > 0.01f && currentStamina > 0)
        {
            // เคลื่อนที่
            MovePlayer(movement);
        }
        else
        {
            StopPlayer();
        }

        // ฟื้นฟู Stamina เมื่อไม่เคลื่อนที่
        RegenerateStamina();

        // อัปเดต Scrollbar
        UpdateStaminaBar();
    }

    void MovePlayer(Vector3 movement)
    {
        movement.Normalize();
        rb.linearVelocity = movement * speed;

        RotateTowards(movement);

        currentStamina -= staminaDrain * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        regenTimer = 0f;

        animator.SetBool("swim", true);
        animator.SetBool("idle", false);

        isMoving = true;
    }

    void StopPlayer()
    {
        rb.linearVelocity = Vector3.zero;
        animator.SetBool("swim", false);
        animator.SetBool("idle", true);
        isMoving = false;
    }

    void RegenerateStamina()
    {
        if (!isMoving)
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= regenDelay && currentStamina < maxStamina)
            {
                currentStamina += staminaRegen * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            }
        }
    }

    void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z), Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.size = currentStamina / maxStamina;
        }
    }
}
