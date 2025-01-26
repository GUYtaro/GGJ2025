using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;  // Dialogue text
    public TextMeshProUGUI promptText;    // TMP for "Press E" message
    public string[] lines;               // Dialogue lines
    public float textSpeed;              // Speed of text typing
    public string nextSceneName = "NextScene"; // Name of the next scene
    public AudioSource typingSound;      // Looping sound during typing
    public AudioSource endSound;         // Sound to play before scene transition
    public Image fadeImage;              // Image used for fading the screen

    private int index;
    private bool dialogueFinished = false; // Track if dialogue is finished
    private bool canPressE = false;        // Track if "E" can be pressed

    void Start()
    {
        textComponent.text = string.Empty;
        promptText.text = string.Empty; // Ensure the prompt text is empty at start
        StartDialogue();

        // Ensure the fade image starts as invisible
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0); // Fully transparent
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
                StopTypingSound(); // Stop typing sound if skipped
            }
        }

        if (dialogueFinished && canPressE && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FadeToBlackAndChangeScene());
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        PlayTypingSound(); // Start typing sound

        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        StopTypingSound(); // Stop typing sound when the line is done

        if (index == lines.Length - 1) // If it's the last line
        {
            dialogueFinished = true;
            promptText.text = "Press E to continue..."; // Show the "Press E" text
            canPressE = true;
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueFinished = true;
            promptText.text = "Press E to continue...";
            canPressE = true;
        }
    }

    IEnumerator FadeToBlackAndChangeScene()
    {
        canPressE = false; // Prevent pressing E multiple times
        promptText.text = string.Empty; // Remove the prompt text

        // Gradually fade the screen using the Image
        if (fadeImage != null)
        {
            float fadeDuration = 2f; // Duration of fade effect
            float timeElapsed = 0f;
            while (timeElapsed < fadeDuration)
            {
                timeElapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(0, 1, timeElapsed / fadeDuration);
                fadeImage.color = new Color(0, 0, 0, alpha); // Gradually increase alpha
                yield return null;
            }
        }

        // Play the end sound
        if (endSound != null)
        {
            endSound.Play();
            yield return new WaitForSeconds(endSound.clip.length); // Wait until the sound finishes
        }

        // Change to the next scene
        SceneManager.LoadScene(nextSceneName);
    }

    void PlayTypingSound()
    {
        if (typingSound != null && !typingSound.isPlaying)
        {
            typingSound.loop = true; // Set the typing sound to loop
            typingSound.Play();
        }
    }

    void StopTypingSound()
    {
        if (typingSound != null && typingSound.isPlaying)
        {
            typingSound.loop = false; // Stop the loop
            typingSound.Stop();
        }
    }
}
