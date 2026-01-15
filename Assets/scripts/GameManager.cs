using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * GameManager
 *
 * This script acts as the central hub for our simple space shooter game. It is responsible for
 * keeping track of the player's score, handling the game over state, managing UI elements like
 * the score display and restart button, and playing sound effects.  Instead of using a
 * Singleton pattern (a pattern that ensures only one instance of a class exists), each script
 * that needs access to the GameManager will hold a reference to it.  This makes it explicit
 * which objects depend on the GameManager and is easier to understand for beginners.
 */
public class GameManager : MonoBehaviour
{
    // UI elements set in the Inspector
    [Header("User Interface")]         // Group related fields in the Inspector
    [SerializeField] private Text scoreText;       // Text element that shows the current score
    [SerializeField] private Text gameOverText;    // Text element that displays "Game Over"
    [SerializeField] private Button restartButton; // Button used to restart the game

    // Audio clips for sound effects set in the Inspector
    [Header("Audio Clips")]           // Another group for audio references
    [SerializeField] private AudioClip explosionClip; // Sound played when something explodes
    [SerializeField] private AudioClip laserClip;     // Sound played when the player fires a laser

    // Reference to the SpawnManager so we can stop spawning enemies when the game ends
    [Header("Game Components")]
    [SerializeField] private SpawnManager spawnManager; // Assigned in the Inspector or found at runtime

    private AudioSource audioSource; // Used to play sound effects
    private int score = 0;          // Keeps track of the player's score
    private bool isGameOver = false; // Flag to prevent multiple game over calls

    private void Start()
    {
        // Initialize the score and UI
        score = 0;
        if (scoreText != null)
        {
            // Show the score starting at 0
            scoreText.text = "Score: 0";
        }
        // Hide Game Over text and the restart button at the beginning
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);
        }

        // Get the AudioSource component attached to the same GameObject as this script
        audioSource = GetComponent<AudioSource>();

    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Restart();
        }
    }
    public void AddScore(int amount)
    {
        score += amount;
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        Debug.Log("your score is: "+score);
    }
    public void PlayLaser()
    {
        if (laserClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(laserClip);
        }
    }

    public void PlayExplosion()
    {
        if (explosionClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosionClip);
        }
    }

    public void PlayerDied()
    {
        if (isGameOver)
        {
            // Prevent multiple calls if the player somehow dies more than once
            return;
        }
        isGameOver = true;

        // Stop spawning any more enemies
        if (spawnManager != null)
        {
            spawnManager.StopSpawning();
        }

        // Show Game Over text and the restart button
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
        }
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(true);
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}