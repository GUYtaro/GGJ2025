using UnityEngine;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour
{
    public Slider oxygenSlider; // Slider สำหรับแสดงค่าอากาศหายใจ
    public float oxygen = 100f; // ค่าอากาศหายใจเริ่มต้น
    public float maxOxygen = 100f; // ค่าอากาศหายใจสูงสุด
    public float oxygenDecreaseRate = 0.5f; // อัตราการลดลงของอากาศหายใจต่อวินาที

    void Start()
    {
        // ตั้งค่าเริ่มต้นของ Slider
        if (oxygenSlider != null)
        {
            oxygenSlider.maxValue = maxOxygen;
            oxygenSlider.value = oxygen;
        }
    }

    void Update()
    {
        // ลดค่าอากาศหายใจทุกวินาที
        oxygen -= oxygenDecreaseRate * Time.deltaTime;
        oxygen = Mathf.Clamp(oxygen, 0, maxOxygen); // จำกัดค่าให้อยู่ระหว่าง 0 และ maxOxygen

        // อัปเดต Slider
        if (oxygenSlider != null)
        {
            oxygenSlider.value = oxygen;
        }
    }

    // ฟังก์ชันเพิ่มค่าอากาศหายใจ
    public void AddOxygen(float amount)
    {
        oxygen += amount;
        oxygen = Mathf.Clamp(oxygen, 0, maxOxygen); // จำกัดไม่ให้เกิน maxOxygen
    }

    // ฟังก์ชันตรวจสอบว่าอากาศหมดหรือไม่
    public bool IsOxygenDepleted()
    {
        return oxygen <= 0;
    }
}
