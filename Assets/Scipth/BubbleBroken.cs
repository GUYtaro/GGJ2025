using UnityEngine;

public class BubbleBroken : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource bubblePopSound; // AudioSource for the bubble pop sound

    private void OnCollisionEnter(Collision collision)
    {
        // Log the name of the object that collided
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        // Check if the object that collided has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit the bubble!");

            // Play the pop sound
            if (bubblePopSound != null)
            {
                bubblePopSound.Play();
            }
            else
            {
                Debug.LogWarning("No AudioSource assigned for bubble pop sound!");
            }

            // Destroy the bubble after the sound finishes
            Destroy(gameObject, bubblePopSound != null ? bubblePopSound.clip.length : 0f);
        }
    }
}
