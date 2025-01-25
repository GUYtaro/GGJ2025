using UnityEngine;

public class UpAndDown : MonoBehaviour
{
    public float speed = 2f;  // ความเร็วในการเคลื่อนที่
    public float amplitude = 0.5f; // ระยะสูง-ต่ำของการเคลื่อนที่ (ความสูงของคลื่น)

    private Vector3 startPos; // ตำแหน่งเริ่มต้นของ GameObject

    void Start()
    {
        // บันทึกตำแหน่งเริ่มต้นของ GameObject
        startPos = transform.position;
    }

    void Update()
    {
        // คำนวณตำแหน่งใหม่แบบขึ้นลงด้วย Sin
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitude;

        // อัปเดตตำแหน่งของ GameObject
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
