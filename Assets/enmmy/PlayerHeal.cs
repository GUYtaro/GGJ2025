using UnityEngine;
using UnityEngine.UI; // สำหรับ Scrollbar
using UnityEngine.SceneManagement; // สำหรับการเปลี่ยน Scene
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f; // พลังชีวิตสูงสุด
    public float currentHealth;  // พลังชีวิตปัจจุบัน

    [Header("UI")]
    public string GameOver = "GameOver";
    public Scrollbar healthBar; // Scrollbar แสดงพลังชีวิต

    void Start()
    {
        currentHealth = maxHealth; // ตั้งค่าพลังชีวิตเริ่มต้น
        UpdateHealthBar(); // อัปเดต Scrollbar
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // ลดพลังชีวิต
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // จำกัดค่าพลังชีวิตให้อยู่ในช่วง 0 - maxHealth

        Debug.Log($"Player health: {currentHealth}");

        UpdateHealthBar(); // อัปเดต Scrollbar

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount; // ฟื้นฟูพลังชีวิต
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // จำกัดค่าพลังชีวิตให้อยู่ในช่วง 0 - maxHealth

        Debug.Log($"Player healed: {currentHealth}");

        UpdateHealthBar(); // อัปเดต Scrollbar
    }           
    private void Die()
    {
        Debug.Log("Player is dead!");
        SceneManager.LoadScene(GameOver);
        // เพิ่มระบบตาย เช่น Restart เกมหรือแสดงหน้าจอ Game Over
        gameObject.SetActive(false); // ปิดการใช้งานผู้เล่น
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.size = currentHealth / maxHealth; // อัปเดต Scrollbar ให้ตรงกับพลังชีวิต
        }
    }
}
