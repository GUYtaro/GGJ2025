using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int Score => score;
    private int score;
    public void AddScore(int score)
    {
        score += score;
        Debug.Log($"Score: {score}/15");
    }
}
