using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Collect : MonoBehaviour
{
    public int score;
    public Camera mainCamera; // กล้องหลักสำหรับ Raycast
    public float raycastDistance = 2f; // ระยะของ Raycast

    void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // ค้นหา Camera ที่มี Tag เป็น "MainCamera" ถ้ายังไม่ได้กำหนด
        }

        // สร้าง Ray จากกึ่งกลางหน้าจอ
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // ตรวจจับ Raycast
        if (Physics.Raycast(ray, out RaycastHit hitInfo, raycastDistance))
        {
            Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red);
            Debug.Log("Hit: " + hitInfo.collider.name);

            // ตรวจจับการกดปุ่ม E
            if (Input.GetKeyDown(KeyCode.E))
            {
                // ปิดการใช้งานวัตถุที่โดน Ray
                hitInfo.collider.gameObject.SetActive(false);
                Debug.Log("Collected: " + hitInfo.collider.name);
                score +=1;
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.green);
        }
        if(score >=15)
        {
            Debug.Log("You win");
        }
    }
}
