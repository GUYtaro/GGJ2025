using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // พลังชีวิตสูงสุด
    private float currentHealth;  // พลังชีวิตปัจจุบัน

    public Animator animator;      // ตัวควบคุมแอนิเมชัน
    public GameObject deathEffect; // เอฟเฟกต์เมื่อผู้เล่นตาย

    void Start()
    {
        currentHealth = maxHealth; // ตั้งค่าพลังชีวิตเริ่มต้น
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // ลดพลังชีวิต
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // จำกัดค่าพลังชีวิตให้อยู่ในช่วง 0 ถึง maxHealth

        Debug.Log($"Player Health: {currentHealth}");

        // ตรวจสอบว่าผู้เล่นตายหรือไม่
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player is dead!");

        // เล่นแอนิเมชันตาย
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // สร้างเอฟเฟกต์การตาย
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // ปิดการใช้งานผู้เล่น
        gameObject.SetActive(false);
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // จำกัดไม่ให้เกิน maxHealth
        Debug.Log($"Player Healed: {currentHealth}");
    }
}
