using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // For scene management

public class OxygenManager : MonoBehaviour
{
    public static OxygenManager Instance; // Singleton for global access

    [Header("Oxygen Settings")]
    public Scrollbar oxygenScrollbar;  // Scrollbar to display oxygen level
    private float oxygen = 100f;       // Starting oxygen value
    private float maxOxygen = 100f;    // Maximum oxygen value
    private float oxygenDepletionRate = 3f; // Oxygen depletion rate per second

    [Header("Scene Settings")]
    public string gameOverScene = "GameOver"; // Scene to load when oxygen runs out

    private void Awake()
    {
        // Create Singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (oxygenScrollbar != null)
        {
            UpdateScrollbar(); // Initialize the scrollbar
        }
    }

    private void Update()
    {
        // Decrease oxygen every second
        oxygen -= oxygenDepletionRate * Time.deltaTime;
        oxygen = Mathf.Clamp(oxygen, 0, maxOxygen); // Ensure oxygen stays within valid range

        if (oxygenScrollbar != null)
        {
            UpdateScrollbar(); // Update scrollbar value
        }

        if (oxygen <= 0)
        {
            Debug.Log("Player has run out of oxygen!");
            ChangeScene(); // Trigger scene change when oxygen reaches zero
        }
    }

    public void AddOxygen(float amount)
    {
        oxygen += amount; // Increase oxygen
        oxygen = Mathf.Clamp(oxygen, 0, maxOxygen); // Ensure oxygen doesn't exceed the maximum

        if (oxygenScrollbar != null)
        {
            UpdateScrollbar();
        }

        Debug.Log("Oxygen increased by: " + amount);
    }

    private void UpdateScrollbar()
    {
        // Update scrollbar to reflect current oxygen level
        oxygenScrollbar.size = oxygen / maxOxygen;
    }

    private void ChangeScene()
    {
        // Load the game over scene
        SceneManager.LoadScene(gameOverScene);
    }
}
