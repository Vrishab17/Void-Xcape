using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Reference to the pause menu GameObject
    public GameObject pauseMenu;

    // Flag to check if the game is paused
    private bool isPaused;

    void Start()
    {
        // Hide the pause menu when the game starts
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        else
        {
            Debug.LogError("PauseMenu GameObject is not assigned in the Inspector!");
        }
        isPaused = false;
    }

    void Update()
    {
        // Debug log to check if Update() is called
        Debug.Log("Update is being called!");

        // Check if Escape key is pressed and toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        // Show the pause menu and stop time
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f; // Pause the game
            isPaused = true;
            Debug.Log("Game Paused");
        }
        else
        {
            Debug.LogError("PauseMenu GameObject is not assigned in the Inspector!");
        }
    }

    public void ResumeGame()
    {
        // Hide the pause menu and resume time
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f; // Resume the game
            isPaused = false;
            Debug.Log("Game Resumed");
        }
        else
        {
            Debug.LogError("PauseMenu GameObject is not assigned in the Inspector!");
        }
    }
}
