using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showMenu : MonoBehaviour
{
    public GameObject instructionsPanel;
    public GameObject settingsPanel;
    // Start is called before the first frame update
    void Start()
    {
        // Make sure only the main menu is visible at start
        instructionsPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void Hide()
    {
        instructionsPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
        instructionsPanel.SetActive(false);
    }

    // Add more functions here as needed for other menu interactions
}