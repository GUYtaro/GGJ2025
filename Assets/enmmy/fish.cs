using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishNPC : MonoBehaviour
{
    public float swimSpeed = 2f; // ความเร็วในการว่าย
    public float fleeSpeed = 5f; // ความเร็วในการว่ายหนี
    public float detectionRadius = 5f; // ระยะที่ตรวจจับผู้เล่น
    public float swimAreaRadius = 10f; // ขอบเขตที่ปลาสามารถว่ายได้

    private Vector3 targetPosition; // ตำแหน่งเป้าหมายที่ปลาจะว่ายไป
    private bool isFleeing = false; // สถานะว่ากำลังว่ายหนีหรือไม่
    private Transform player; // ตัวผู้เล่น

    private void Start()
    {
        SetRandomTargetPosition();

        // ค้นหา GameObject ของผู้เล่นโดยใช้ชื่อ
        GameObject playerObject = GameObject.Find("PlayerController");
        if (playerObject != null)
        {
            player = playerObject.transform; // กำหนด Transform ของผู้เล่น
        }
        else
        {
            Debug.LogWarning("PlayerController not found in the scene!");
        }
    }

    private void Update()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            // ว่ายหนีผู้เล่น
            isFleeing = true;
            FleeFromPlayer();
        }
        else
        {
            // ว่ายน้ำตามปกติ
            isFleeing = false;
            SwimNormally();
        }
    }

    private void SwimNormally()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            SetRandomTargetPosition(); // ตั้งเป้าหมายใหม่เมื่อถึงจุด
        }

        MoveTowards(targetPosition, swimSpeed);
    }

    private void FleeFromPlayer()
    {
        if (player == null) return;

        Vector3 fleeDirection = (transform.position - player.position).normalized;
        Vector3 fleeTarget = transform.position + fleeDirection * fleeSpeed;

        Debug.DrawLine(transform.position, fleeTarget, Color.red, 0.5f); // แสดงทิศทางการหนี
        MoveTowards(fleeTarget, fleeSpeed);
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.LookAt(target); // หันหน้าไปยังเป้าหมาย
    }

    private void SetRandomTargetPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * swimAreaRadius;
        randomDirection.y = 0; // ล็อกแกน Y ไว้ที่เดิมเพื่อไม่ให้ปลาว่ายขึ้นหรือลง
        targetPosition = transform.position + randomDirection;

        // ตรวจสอบขอบเขต
        targetPosition = ClampPositionToSwimArea(targetPosition);
    }

    private Vector3 ClampPositionToSwimArea(Vector3 position)
    {
        Vector3 center = transform.position;
        Vector3 direction = (position - center).normalized;
        float distance = Mathf.Min(Vector3.Distance(center, position), swimAreaRadius);
        return center + direction * distance;
    }

    private void OnDrawGizmosSelected()
    {
        // แสดงระยะการว่ายและตรวจจับผู้เล่น
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, swimAreaRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
