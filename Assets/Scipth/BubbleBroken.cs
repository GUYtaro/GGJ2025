using UnityEngine;

public class BubbleBroken : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // แสดงชื่อ GameObject ที่ชนใน Console
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        // ตรวจสอบว่า GameObject ที่ชนมี Tag เป็น "Player" หรือไม่
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit the bubble!");
            Destroy(gameObject); // ทำลาย Bubble
        }
    }
}
