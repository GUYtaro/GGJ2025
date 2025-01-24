using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;  // ตัวควบคุมแอนิเมชัน
    public Rigidbody rb;       // Rigidbody ของตัวละคร
    public float speed = 5f;   // ความเร็วการเคลื่อนที่

    void Update()
    {
        // รับ Input การเคลื่อนที่
        float moveX = Input.GetAxis("Horizontal"); // ซ้าย/ขวา
        float moveY = 0f;                          // ขึ้น/ลง
        float moveZ = Input.GetAxis("Vertical");   // เดินหน้า/ถอยหลัง

        if (Input.GetKey(KeyCode.Space)) moveY = 1f;       // ลอยขึ้น
        if (Input.GetKey(KeyCode.LeftShift)) moveY = -1f;  // ลอยลง

        // สร้างเวกเตอร์การเคลื่อนที่
        Vector3 movement = new Vector3(moveX, moveY, moveZ);

        // ตั้งค่าแอนิเมชัน
        if (movement != Vector3.zero)
        {
            animator.SetBool("swim", true); // เปิดแอนิเมชันว่ายน้ำ
        }
        else
        {
            animator.SetBool("swim", false); // เปิดแอนิเมชัน Idle
        }

        // ใช้ Rigidbody เคลื่อนที่
        rb.linearVelocity = movement * speed;
    }
}

//rb.linearVelocity