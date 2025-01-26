using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        // Unlock and make the cursor visible when the scene starts
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
    }
}
