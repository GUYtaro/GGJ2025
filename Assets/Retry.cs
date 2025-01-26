using UnityEngine;
using UnityEngine.SceneManagement; // สำหรับการจัดการ Scene

public class Retry : MonoBehaviour
{
    public void RetryGame()
    {
        // โหลด Scene ปัจจุบันใหม่
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
