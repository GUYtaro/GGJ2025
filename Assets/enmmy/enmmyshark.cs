using UnityEngine;

public class EnemyAIInWater : MonoBehaviour
{
    [Header("Player Detection")]
    public Transform player;
    public float followRange = 10f;
    public float loseSightRange = 15f;
    public float speedIncreaseRange = 5f; // ระยะที่เพิ่มความเร็ว

    [Header("Wandering Behavior")]
    public float wanderRadius = 5f;
    public float wanderDelay = 3f;
    public float wanderSpeed = 2f;

    [Header("Movement Settings")]
    public float followSpeed = 3f;
    public float speedMultiplier = 1.5f; // ตัวคูณความเร็วเมื่อใกล้ Player
    public float rotationSpeed = 2f;

    private Vector3 wanderTarget;
    private float timeSinceLastWander;
    private bool isFollowing = false;
    private Vector3 currentVelocity; // ใช้สำหรับ SmoothDamp

    private void Update()
    {
        float distanceToPlayerSqr = (transform.position - player.position).sqrMagnitude;
        float followRangeSqr = followRange * followRange;
        float loseSightRangeSqr = loseSightRange * loseSightRange;

        if (distanceToPlayerSqr <= followRangeSqr)
        {
            isFollowing = true;
        }
        else if (distanceToPlayerSqr > loseSightRangeSqr)
        {
            isFollowing = false;
        }

        if (isFollowing)
        {
            FollowPlayer(distanceToPlayerSqr);
        }
        else
        {
            Wander();
        }
    }

    private void FollowPlayer(float distanceToPlayerSqr)
    {
        // คำนวณทิศทางไปยังผู้เล่น
        Vector3 direction = (player.position - transform.position).normalized;

        // เพิ่มความเร็วเมื่อเข้าใกล้ Player
        float currentSpeed = followSpeed;
        if (distanceToPlayerSqr <= speedIncreaseRange * speedIncreaseRange)
        {
            currentSpeed *= speedMultiplier;
        }

        // ใช้ SmoothDamp เพื่อให้การเคลื่อนที่นุ่มนวล
        Vector3 targetPosition = transform.position + direction * currentSpeed * Time.deltaTime;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 0.2f);

        // หมุนศัตรูให้หันหน้าหาผู้เล่นแบบ Smooth
        RotateTowards(direction);
    }

    private void Wander()
    {
        timeSinceLastWander += Time.deltaTime;

        // สุ่มเป้าหมายใหม่เมื่อถึงเวลา
        if (timeSinceLastWander >= wanderDelay || Vector3.Distance(transform.position, wanderTarget) <= 1f)
        {
            wanderTarget = GetRandomWanderPosition();
            timeSinceLastWander = 0f;
        }

        // คำนวณทิศทางไปยังเป้าหมายสุ่ม
        Vector3 direction = (wanderTarget - transform.position).normalized;

        // ใช้ SmoothDamp เพื่อให้การเคลื่อนที่ดูสมจริง
        Vector3 targetPosition = transform.position + direction * wanderSpeed * Time.deltaTime;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 0.3f);

        // หมุนศัตรูให้หันไปทางเป้าหมายสุ่มแบบ Smooth
        RotateTowards(direction);
    }

    private Vector3 GetRandomWanderPosition()
    {
        // สุ่มตำแหน่งในระยะ wanderRadius
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection.y = 0; // เคลื่อนที่เฉพาะในระนาบ XY
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