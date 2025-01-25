using UnityEngine;
using TMPro; // ใช้ TextMesh Pro

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton สำหรับเข้าถึงจากที่อื่น
    private int score = 0;               // ตัวแปรเก็บคะแนน
    public int maxScore = 15;            // คะแนนสูงสุด
    public TextMeshProUGUI scoreText;    // อ้างอิงถึง TextMesh Pro UI

    private void Awake()
    {
        // ทำให้ ScoreManager เป็น Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ทำให้ ScoreManager ไม่ถูกทำลายเมื่อเปลี่ยน Scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreText(); // อัปเดตคะแนนเริ่มต้น
    }

    // เพิ่มคะแนน
    public void AddScore(int amount)
    {
        score += amount;
        if (score > maxScore)
        {
            score = maxScore; // จำกัดไม่ให้คะแนนเกิน maxScore
        }
        UpdateScoreText(); // อัปเดตข้อความคะแนน
        Debug.Log("Score: " + score);
    }

    // รับค่าคะแนนปัจจุบัน
    public int GetScore()
    {
        return score;
    }

    // ฟังก์ชันอัปเดตข้อความคะแนน
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}/{maxScore}";
        }
        else
        {
            Debug.LogWarning("Score Text (TMP) is not assigned!");
        }
    }
}
