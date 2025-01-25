using UnityEngine;
using UnityEngine.UI; // สำหรับ UI Scrollbar

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;         // ตัวควบคุมแอนิเมชัน
    public Rigidbody rb;              // Rigidbody ของตัวละคร
    public float maxSpeed = 5f;       // ความเร็วสูงสุดของตัวละคร
    public float speed;               // ความเร็วปัจจุบัน (ขึ้นอยู่กับ Stamina)

    [Header("Stamina Settings")]
    public float maxStamina = 100f;   // Stamina สูงสุด
    public float currentStamina;      // Stamina ปัจจุบัน
    public float staminaDrain = 10f;  // ลด Stamina ต่อวินาทีเมื่อเคลื่อนที่
    public float staminaRegen = 5f;   // ฟื้นฟู Stamina ต่อวินาทีเมื่อไม่เคลื่อนที่
    public float regenDelay = 2f;     // เวลาที่ต้องรอก่อนฟื้นฟู Stamina

    private float regenTimer;         // ตัวจับเวลาฟื้นฟู Stamina
    public float minSpeedMultiplier = 0.5f; // คูณความเร็วขั้นต่ำเมื่อ Stamina ต่ำ

    [Header("UI")]
    public Scrollbar staminaBar;      // Scrollbar สำหรับแสดง Stamina

    void Start()
    {
        currentStamina = maxStamina; // ตั้งค่าเริ่มต้นให้ Stamina เต็ม
        speed = maxSpeed;            // ตั้งค่าเริ่มต้นความเร็ว
    }

    void Update()
    {
        // รับ Input การเคลื่อนที่
        float moveX = Input.GetAxis("Horizontal");
        float moveY = 0f;
        float moveZ = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space)) moveY = 1f;       // ลอยขึ้น
        if (Input.GetKey(KeyCode.LeftShift)) moveY = -1f;  // ลอยลง

        Vector3 movement = new Vector3(moveX, moveY, moveZ).normalized;

        // ลด Stamina เมื่อเคลื่อนที่
        if (movement != Vector3.zero && currentStamina > 0)
        {
            currentStamina -= staminaDrain * Time.deltaTime;
            regenTimer = 0f; // รีเซ็ตตัวจับเวลาฟื้นฟู Stamina
            rb.linearVelocity = movement * speed; // เคลื่อนที่
            animator.SetBool("swim", true);  // เปิดแอนิเมชันว่ายน้ำ
            animator.SetBool("idle", false); // ปิดแอนิเมชัน Idle
        }
        else
        {
            rb.linearVelocity = Vector3.zero;     // หยุดเคลื่อนที่
            regenTimer += Time.deltaTime;

            // ฟื้นฟู Stamina หลังจาก regenDelay
            if (regenTimer >= regenDelay && currentStamina < maxStamina)
            {
                currentStamina += staminaRegen * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); // จำกัดไม่ให้เกิน maxStamina
            }

            animator.SetBool("swim", false); // ปิดแอนิเมชันว่ายน้ำ
            animator.SetBool("idle", true);  // เปิดแอนิเมชัน Idle
        }

        // จำกัดค่า Stamina ให้อยู่ในช่วง 0 ถึง maxStamina
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        // คำนวณความเร็วตาม Stamina
        float staminaPercent = currentStamina / maxStamina;
        speed = Mathf.Lerp(maxSpeed * minSpeedMultiplier, maxSpeed, staminaPercent);

        // อัปเดต Scrollbar
        UpdateStaminaBar();
    }

    void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.size = currentStamina / maxStamina; // อัปเดต Scrollbar
        }
    }
}
