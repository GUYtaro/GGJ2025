using UnityEngine;

public class attack : MonoBehaviour
{
    public float damage = 20f; // ความเสียหายที่ศัตรูทำได้
    public float attackCooldown = 1.5f; // ระยะเวลาระหว่างการโจมตีแต่ละครั้ง

    private float lastAttackTime; // เวลาโจมตีครั้งล่าสุด

    public GameObject currentObjec;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        currentObjec = other.gameObject;
        if (other.CompareTag("Player"))
        {
            TryAttack(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TryAttack(other);
        }
    }

    private void TryAttack(Collider player)
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Enemy attacked Player!");
                lastAttackTime = Time.time; // บันทึกเวลาโจมตี
            }
        }
    }
}
