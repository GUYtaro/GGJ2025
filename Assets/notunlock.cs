using UnityEngine;
using TMPro; // Import สำหรับ TextMeshPro

public class notunlock : MonoBehaviour
{
    public TMP_Text messageText;       // อ้างอิงถึงข้อความที่จะแสดงผล
    public string lockedMessage = "Lock"; // ข้อความที่จะแสดงเมื่อกด
    public float displayDuration = 2f; // ระยะเวลาที่ข้อความแสดงบนหน้าจอ

    private float timer;               // ตัวจับเวลา

    private void Update()
    {
        // ตรวจสอบว่าข้อความกำลังแสดงอยู่หรือไม่
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            // เมื่อหมดเวลา ให้ซ่อนข้อความ
            if (timer <= 0)
            {
                messageText.text = ""; // ลบข้อความ
            }
        }
    }

    public void OnTextClick()
    {
        messageText.text = lockedMessage; // แสดงข้อความ "Lock"
        timer = displayDuration;          // ตั้งเวลาการแสดงผล
    }
}
