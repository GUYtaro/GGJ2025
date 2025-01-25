using UnityEngine;
using UnityEngine.SceneManagement; // สำหรับการเปลี่ยน Scene

public class ChangeScene : MonoBehaviour
{
    // ฟังก์ชันสำหรับเปลี่ยนไปยัง Scene ที่กำหนด
    public void LoadScene(string Mewscene)
    {
        SceneManager.LoadScene(Mewscene); // โหลด Scene ตามชื่อ
    }
}
