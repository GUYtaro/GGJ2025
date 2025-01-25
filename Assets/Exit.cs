using UnityEngine;

public class Exit : MonoBehaviour
{
    // Function to exit the game
    public void QuitGame()
    {
        Debug.Log("Game is exiting..."); // Logs a message (useful during testing in the editor)
        Application.Quit(); // Closes the application
    }
}
