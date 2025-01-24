using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;      // ความเร็วในการเคลื่อนที่
    public float rotationSpeed = 5f; // ความเร็วในการหมุน
    public float buoyancy = 2f;      // แรงลอยตัวในน้ำ

    [Header("Input Settings")]
    public string horizontalInput = "Horizontal"; // ปุ่มเคลื่อนที่แนวนอน (ค่าเริ่มต้นคือ A/D หรือลูกศรซ้าย/ขวา)
    public string verticalInput = "Vertical";     // ปุ่มเคลื่อนที่แนวตั้ง (ค่าเริ่มต้นคือ W/S หรือลูกศรขึ้น/ลง)
    public KeyCode ascendKey = KeyCode.Space;     // ปุ่มสำหรับขึ้นด้านบน
    public KeyCode descendKey = KeyCode.LeftShift; // ปุ่มสำหรับลงด้านล่าง

    private Rigidbody rb;

    void Start()
    {
        // ดึง Rigidbody มาจาก GameObject
        rb = GetComponent<Rigidbody>();

        // ตั้งค่าความหนืดในน้ำ
        rb.linearDamping = 2f;
        rb.angularDamping = 2f;
    }

    void Update()
    {
        // รับค่าการเคลื่อนที่จากคีย์บอร์ด
        float horizontal = Input.GetAxis(horizontalInput);
        float vertical = Input.GetAxis(verticalInput);

        // คำนวณทิศทางการเคลื่อนที่
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

        // เคลื่อนที่ไปในทิศทางที่ผู้เล่นกำหนด
        Move(movement);

        // เคลื่อนที่ขึ้นหรือลงในน้ำ
        if (Input.GetKey(ascendKey))
        {
            Ascend();
        }
        else if (Input.GetKey(descendKey))
        {
            Descend();
        }
    }

    void Move(Vector3 direction)
    {
        if (direction.magnitude > 0)
        {
            // คำนวณการเคลื่อนที่
            Vector3 moveDirection = transform.forward * direction.z + transform.right * direction.x;
            rb.AddForce(moveDirection * moveSpeed, ForceMode.Acceleration);

            // หมุนตัวผู้เล่นไปในทิศทางการเคลื่อนที่
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void Ascend()
    {
        // เพิ่มแรงเพื่อให้ตัวละครลอยขึ้น
        rb.AddForce(Vector3.up * buoyancy, ForceMode.Acceleration);
    }

    void Descend()
    {
        // ลดแรงเพื่อให้ตัวละครจมลง
        rb.AddForce(Vector3.down * buoyancy, ForceMode.Acceleration);
    }
}
