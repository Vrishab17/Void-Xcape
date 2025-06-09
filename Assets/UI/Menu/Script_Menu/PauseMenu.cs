using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuUI;
    
    [Header("Settings")]
    public KeyCode pauseKey = KeyCode.P;
    public KeyCode alternatePauseKey = KeyCode.Escape; // Common alternative
    
    public static bool GameIsPaused = false;
    
    // Events for other scripts to listen to
    public static System.Action<bool> OnPauseStateChanged;
    
    void Start()
    {
        // Ensure pause menu starts inactive
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
            
        // Reset pause state on scene start
        GameIsPaused = false;
        Time.timeScale = 1f;
    }
    
    void Update()
    {
        // Check for pause input
        if (Input.GetKeyDown(pauseKey) || Input.GetKeyDown(alternatePauseKey))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    
    public void Resume()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
            
        Time.timeScale = 1f;
        GameIsPaused = false;
        
        // Lock cursor back for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Notify other scripts
        OnPauseStateChanged?.Invoke(false);
        
        Debug.Log("Game Resumed");
    }
    
    public void Pause()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
        else
        {
            Debug.LogError("Pause Menu UI is not assigned!");
            return;
        }
            
        Time.timeScale = 0f;
        GameIsPaused = true;
        
        // Unlock and show cursor for menu interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Notify other scripts
        OnPauseStateChanged?.Invoke(true);
        
        Debug.Log("Game Paused");
    }
    
    public void MainMenu()
    {
        Debug.Log("Loading Main Menu...");
        Time.timeScale = 1f;
        GameIsPaused = false;
        
        // Make sure to reset cursor state
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        SceneManager.LoadScene("Menu");
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Time.timeScale = 1f;
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    // Optional: Method to check if game should accept input
    public static bool ShouldAcceptInput()
    {
        return !GameIsPaused;
    }
}