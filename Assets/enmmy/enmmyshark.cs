using UnityEngine;

public class enemyShark : MonoBehaviour
{
    [Header("Player Detection")]
    public Transform player;               // ตำแหน่งของผู้เล่น
    public float followRange = 10f;        // ระยะที่ศัตรูเริ่มตามผู้เล่น
    public float loseSightRange = 15f;     // ระยะที่ศัตรูหยุดตามผู้เล่น
    public float speedIncreaseRange = 5f; // ระยะที่เพิ่มความเร็ว

    [Header("Wandering Behavior")]
    public float wanderRadius = 5f;        // ระยะการเดินสุ่ม
    public float wanderDelay = 3f;         // เวลาระหว่างการสุ่มเป้าหมาย
    public float wanderSpeed = 2f;         // ความเร็วการเดินสุ่ม

    [Header("Movement Settings")]
    public float followSpeed = 3f;         // ความเร็วตามผู้เล่น
    public float speedMultiplier = 1.5f;   // ตัวคูณความเร็วเมื่อใกล้ Player
    public float rotationSpeed = 2f;       // ความเร็วการหมุนตัว

    [Header("Animation")]
    public Animator Sharkanim;             // ตัวควบคุมแอนิเมชัน

    private Vector3 wanderTarget;          // ตำแหน่งเป้าหมายสุ่มเดิน
    private float timeSinceLastWander;     // เวลาเช็คการสุ่มเป้าหมาย
    private bool isFollowing = false;      // กำลังตามผู้เล่นหรือไม่
    private Vector3 currentVelocity;       // ใช้สำหรับ SmoothDamp

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // ตรวจสอบระยะห่างระหว่างศัตรูกับผู้เล่น
        if (distanceToPlayer <= followRange)
        {
            isFollowing = true;
        }
        else if (distanceToPlayer > loseSightRange)
        {
            isFollowing = false;
        }

        // การกระทำของศัตรูตามสถานะ
        if (isFollowing)
        {
            FollowPlayer(distanceToPlayer);
            Sharkanim.SetBool("fast", true); // เล่นแอนิเมชัน "fast"
        }
        else
        {
            Wander();
            Sharkanim.SetBool("fast", false); // หยุดเล่นแอนิเมชัน "fast"
        }
    }

    private void FollowPlayer(float distanceToPlayer)
    {
        // คำนวณทิศทางไปยังผู้เล่น
        Vector3 direction = (player.position - transform.position).normalized;

        // เพิ่มความเร็วเมื่อเข้าใกล้ Player
        float currentSpeed = followSpeed;
        if (distanceToPlayer <= speedIncreaseRange)
        {
            currentSpeed *= speedMultiplier;
        }

        // เคลื่อนที่ไปยังผู้เล่น
        transform.position += direction * currentSpeed * Time.deltaTime;

        // หมุนศัตรูให้หันหน้าหาผู้เล่น
        RotateTowards(direction);
    }

    private void Wander()
    {
        timeSinceLastWander += Time.deltaTime;

        // สุ่มเป้าหมายใหม่เมื่อถึงเวลา หรือศัตรูถึงเป้าหมายแล้ว
        if (timeSinceLastWander >= wanderDelay || Vector3.Distance(transform.position, wanderTarget) <= 1f)
        {
            wanderTarget = GetRandomWanderPosition();
            timeSinceLastWander = 0f;
        }

        // คำนวณทิศทางไปยังเป้าหมายสุ่ม
        Vector3 direction = (wanderTarget - transform.position).normalized;

        // เคลื่อนที่ไปยังเป้าหมายสุ่ม
        transform.position += direction * wanderSpeed * Time.deltaTime;

        // หมุนศัตรูให้หันหน้าไปทางเป้าหมายสุ่ม
        RotateTowards(direction);
    }

    private Vector3 GetRandomWanderPosition()
    {
        // สุ่มตำแหน่งในระยะ wanderRadius
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection.y = 0; // ให้เคลื่อนที่เฉพาะบนระนาบ XY (พื้นผิวน้ำ)
        return transform.position + randomDirection;
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction.magnitude == 0) return;

        // หมุนตัวศัตรูให้หันไปทางเป้าหมายอย่างลื่นไหล
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        // วาดระยะใน Scene View เพื่อช่วยในการตรวจสอบ
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, followRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, loseSightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, speedIncreaseRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
