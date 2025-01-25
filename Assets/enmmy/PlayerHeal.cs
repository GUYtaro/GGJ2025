using UnityEngine;
using UnityEngine.UI; // สำหรับ UI Scrollbar

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;       // พลังชีวิตสูงสุด
    public float currentHealth;          // พลังชีวิตปัจจุบัน
    public Animator animator;            // ตัวควบคุมแอนิเมชัน
    public GameObject deathEffect;       // เอฟเฟกต์เมื่อผู้เล่นตาย

    [Header("UI")]
    public Scrollbar healthBar;          // Scrollbar สำหรับแสดงพลังชีวิต

    void Start()
    {
        currentHealth = maxHealth;       // ตั้งค่าเริ่มต้นให้พลังชีวิตเต็ม
        UpdateHealthBar();               // อัปเดต Scrollbar
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;         // ลดพลังชีวิต
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // จำกัดค่าพลังชีวิตให้อยู่ในช่วง 0 ถึง maxHealth

        Debug.Log($"Player Health: {currentHealth}");

        // ตรวจสอบว่าผู้เล่นตายหรือไม่
        if (currentHealth <= 0)
        {
            Die();
        }

        UpdateHealthBar(); // อัปเดต Scrollbar
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;     // ฟื้นฟูพลังชีวิต
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // จำกัดค่าพลังชีวิตให้อยู่ในช่วง 0 ถึง maxHealth

        Debug.Log($"Player Healed: {currentHealth}");

        UpdateHealthBar(); // อัปเดต Scrollbar
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

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.size = currentHealth / maxHealth; // อัปเดต Scrollbar
        }
    }
}
