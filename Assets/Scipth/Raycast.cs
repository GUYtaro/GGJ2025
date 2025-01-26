using UnityEngine;
using UnityEngine.SceneManagement; // สำหรับเปลี่ยนฉาก
using UnityEngine.UI; // สำหรับการปรับ UI (จอมืด)
using System.Collections; // สำหรับ IEnumerator

public class Raycast : MonoBehaviour
{
    public Camera mainCamera;    // กล้องหลัก
    public float rayDistance = 10f; // ระยะของ Ray (ปรับได้ใน Inspector)
    public Image fadeImage;      // UI Image สำหรับทำจอให้มืด
    public float fadeDuration = 2f; // ระยะเวลาในการเฟดจอมืด
    public string targetSceneName = "YourSceneName"; // ชื่อฉากที่ต้องการเปลี่ยนไป
    private bool isFading = false;  // สถานะว่ากำลังเฟดหรือไม่

    public int requiredScore = 10; // คะแนนที่ต้องการเพื่อเปลี่ยนฉาก

    void Update()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Please assign a Camera to this script!");
            return;
        }

        // สร้าง Ray จากจุดกึ่งกลางหน้าจอ
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // ตรวจจับ Raycast โดยไม่สนใจ Trigger
        if (Physics.Raycast(ray, out RaycastHit hitInfo, rayDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red);

            // ตรวจสอบว่าผู้เล่นกดปุ่ม E
            if (Input.GetKeyDown(KeyCode.E))
            {
                // ตรวจสอบว่า Object มีแท็ก "GGJ"
                if (hitInfo.collider.CompareTag("GGJ"))
                {
                    // ลบ Object ที่มีแท็ก "GGJ"
                    Destroy(hitInfo.collider.gameObject);

                    // เพิ่มคะแนนใน ScoreManager
                    ScoreManager.Instance.AddScore(1);
                }
                // ตรวจสอบว่า Object มีแท็ก "Bubble"
                else if (hitInfo.collider.CompareTag("Bubble"))
                {
                    // ลบ Bubble ออก
                    Destroy(hitInfo.collider.gameObject);

                    // เพิ่มค่าออกซิเจน
                    OxygenManager.Instance.AddOxygen(30f);
                }
                // ตรวจสอบว่า Object มีแท็ก "End"
                else if (hitInfo.collider.CompareTag("End") && !isFading)
                {
                    // ตรวจสอบคะแนนก่อนเปลี่ยนฉาก
                    if (ScoreManager.Instance.GetScore() >= requiredScore)
                    {
                        StartCoroutine(FadeToBlackAndChangeScene());
                    }
                    else
                    {
                        Debug.Log("Not enough score to change scene!");
                    }
                }
                else
                {
                    Debug.Log("Object does not have the required tag: GGJ, Bubble, or End");
                }
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);
        }
    }

    private IEnumerator FadeToBlackAndChangeScene()
    {
        isFading = true; // เริ่มเฟด
        Color fadeColor = fadeImage.color;
        float fadeSpeed = 1f / fadeDuration;

        // เฟดจอให้มืด
        for (float t = 0; t <= 1; t += Time.deltaTime * fadeSpeed)
        {
            fadeColor.a = t; // เพิ่มความทึบของสี
            fadeImage.color = fadeColor;
            yield return null;
        }

        // เปลี่ยนไปยังฉากเฉพาะ
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("Target scene name is not set!");
        }
    }
}
