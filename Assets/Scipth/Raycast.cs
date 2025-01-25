using UnityEngine;

public class Raycast : MonoBehaviour
{
    public Camera mainCamera; // กล้องหลัก

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
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 10f, ~0, QueryTriggerInteraction.Ignore))
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
                    OxygenManager.Instance.AddOxygen(10f);
                }
                else
                {
                    Debug.Log("Object does not have the required tag: GGJ or Bubble");
                }
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green);
        }
    }
}
