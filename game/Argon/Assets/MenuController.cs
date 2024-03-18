using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject instructionsPanel;
    public GameObject settingsPanel;
        
    public GameObject MenuUI; // Drag your MenuUI GameObject here in the inspector
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        MenuUI.SetActive(isPaused); // Show or hide the pause menu
        instructionsPanel.SetActive(false);
        settingsPanel.SetActive(false);

        if (isPaused)
        {
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
        }
    }
}