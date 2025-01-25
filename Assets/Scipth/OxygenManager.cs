using UnityEngine;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour
{
    public static OxygenManager Instance; // Singleton สำหรับเข้าถึงจากที่อื่น

    public Slider oxygenSlider;  // Slider แสดงออกซิเจน
    private float oxygen = 100f; // ค่าออกซิเจนเริ่มต้น
    private float maxOxygen = 100f; // ค่าออกซิเจนสูงสุด
    private float oxygenDepletionRate = 0.5f; // ลดออกซิเจน 0.5 ทุกวินาที

    private void Awake()
    {
        // สร้าง Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (oxygenSlider != null)
        {
            oxygenSlider.maxValue = maxOxygen;
            oxygenSlider.value = oxygen;
        }
    }

    private void Update()
    {
        // ลดค่าออกซิเจนทุกวินาที
        oxygen -= oxygenDepletionRate * Time.deltaTime;
        oxygen = Mathf.Clamp(oxygen, 0, maxOxygen); // จำกัดค่าไม่ให้น้อยกว่า 0 หรือเกิน maxOxygen

        if (oxygenSlider != null)
        {
            oxygenSlider.value = oxygen;
        }

        if (oxygen <= 0)
        {
            Debug.Log("Player has run out of oxygen!");
        }
    }

    public void AddOxygen(float amount)
    {
        oxygen += amount;
        oxygen = Mathf.Clamp(oxygen, 0, maxOxygen); // จำกัดค่าออกซิเจนไม่ให้เกิน maxOxygen

        if (oxygenSlider != null)
        {
            oxygenSlider.value = oxygen;
        }

        Debug.Log("Oxygen increased by: " + amount);
    }
}
