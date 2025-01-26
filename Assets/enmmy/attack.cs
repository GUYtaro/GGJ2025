using UnityEngine;

public class attack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float damage = 20f; // Damage dealt by the shark
    public float attackCooldown = 1.5f; // Time between attacks

    private float lastAttackTime; // Last attack timestamp
    private PlayerHealth playerHealth; // Reference to the PlayerHealth script

    [Header("Audio Settings")]
    public AudioSource attackSound; // AudioSource for the attack sound

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered attack range");
            playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                TryAttack(); // Attack immediately upon entering range
            }
            else
            {
                Debug.LogWarning("PlayerHealth not found on Player!");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && playerHealth != null)
        {
            TryAttack(); // Attack while staying in range
        }
    }

    private void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("TryAttack called");

            // Apply damage
            playerHealth.TakeDamage(damage);
            Debug.Log($"Damage applied to player: {damage}");

            // Play attack sound
            PlayAttackSound();

            // Record the time of this attack
            lastAttackTime = Time.time;
        }
        else
        {
            Debug.Log("Attack on cooldown");
        }
    }

    private void PlayAttackSound()
    {
        if (attackSound != null)
        {
            attackSound.Play(); // Play the attack sound
        }
        else
        {
            Debug.LogWarning("No attack sound assigned!");
        }
    }
}
