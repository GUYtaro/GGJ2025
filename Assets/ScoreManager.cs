using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int Score => _score;
    private int _score;
    public void AddScore(int score)
    {
        _score += score;
        Debug.Log($"Score: {_score}");
    }
}
