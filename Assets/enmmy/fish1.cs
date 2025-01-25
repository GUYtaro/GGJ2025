using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish1 : MonoBehaviour
{
    public float swimSpeed = 2f; // ความเร็วพื้นฐานในการว่าย
    public float fleeSpeed = 5f; // ความเร็วในการว่ายหนี
    public float acceleration = 2f; // อัตราเร่ง
    public float rotationSpeed = 2f; // ความเร็วในการหมุน
    public float detectionRadius = 5f; // ระยะตรวจจับผู้เล่น
    public float swimAreaRadius = 10f; // ขอบเขตว่ายน้ำ

    private Vector3 targetPosition; // ตำแหน่งเป้าหมายที่ปลาจะว่ายไป
    private bool isFleeing = false; // สถานะว่ายหนี
    private Transform player; // ตัวผู้เล่น
    private Vector3 currentVelocity; // ความเร็วปัจจุบัน

    private void Start()
    {
        SetRandomTargetPosition();

        // ค้นหาผู้เล่น
        GameObject playerObject = GameObject.Find("PlayerController");
        if (playerObject != null)
        {
            player = playerObject.transform;
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
            // หากอยู่ในระยะตรวจจับ ให้ว่ายหนี
            isFleeing = true;
            FleeFromPlayer();
        }
        else
        {
            // ว่ายแบบปกติ
            isFleeing = false;
            SwimNormally();
        }
    }

    private void SwimNormally()
    {
        // หากถึงเป้าหมายแล้ว ให้ตั้งเป้าหมายใหม่
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            SetRandomTargetPosition();
        }

        SmoothMoveTowards(targetPosition, swimSpeed);
    }

    private void FleeFromPlayer()
    {
        if (player == null) return;

        Vector3 fleeDirection = (transform.position - player.position).normalized;
        Vector3 fleeTarget = transform.position + fleeDirection * fleeSpeed;

        SmoothMoveTowards(fleeTarget, fleeSpeed);
    }

    private void SmoothMoveTowards(Vector3 target, float targetSpeed)
    {
        // คำนวณทิศทางการเคลื่อนที่
        Vector3 direction = (target - transform.position).normalized;

        // อัพเดตความเร็วด้วยอัตราเร่ง
        currentVelocity = Vector3.Lerp(currentVelocity, direction * targetSpeed, acceleration * Time.deltaTime);

        // เคลื่อนที่
        transform.position += currentVelocity * Time.deltaTime;

        // หมุนตัวไปทางเป้าหมายอย่างนุ่มนวล
        if (currentVelocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentVelocity, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void SetRandomTargetPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * swimAreaRadius;
        randomDirection.y = 0; // ล็อกแกน Y
        targetPosition = transform.position + randomDirection;

        // ตรวจสอบให้เป้าหมายไม่อยู่นอกขอบเขต
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
        // แสดงขอบเขตการว่ายและการตรวจจับ
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, swimAreaRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
