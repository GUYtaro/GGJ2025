using UnityEngine;
using TMPro;
public class ScoreDIsplay : MonoBehaviour
{
    public ScoreManager score;
    public TMP_Text textDisplay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        textDisplay.text = $"Score: {score.Score}";
        // อัปเดตคะแนนใน UI
        textDisplay.text = $"Score: {score.Score}";

        // ตรวจสอบว่าคะแนนมากกว่า 5 หรือไม่
        if (score.Score >= 5)
        {
            EndGame();
        }
    }
    private void EndGame()
    {
        textDisplay.text="You done GG";
        Debug.Log("GG");

    }
}
