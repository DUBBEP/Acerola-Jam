using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject mapScreen;
    public GameObject controlsScreen;
    public GameObject dashControlsScreen;
    public GameObject wallStickControlsScreen;
    public GameObject glideControlsScreen;

    [Header("info")]
    bool gameIsPaused = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameIsPaused)
        {
            // Set pause screen active
            SetScreen(pauseScreen);

            gameIsPaused = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && gameIsPaused)
        {
            // remove pause screen
            SetScreen();

            gameIsPaused = false;
        }
    }

    void SetScreen(GameObject screen)
    {

        pauseScreen.SetActive(false);
        mapScreen.SetActive(false);
        controlsScreen.SetActive(false);
        dashControlsScreen.SetActive(false);
        wallStickControlsScreen.SetActive(false);
        glideControlsScreen.SetActive(false);

        screen.SetActive(true);
    }

    void SetScreen()
    {
        pauseScreen.SetActive(false);
        mapScreen.SetActive(false);
        controlsScreen.SetActive(false);
        dashControlsScreen.SetActive(false);
        wallStickControlsScreen.SetActive(false);
        glideControlsScreen.SetActive(false);
    }

    public void OnPauseScreenButton()
    {
        SetScreen(pauseScreen);
    }

    public void OnControlsButton()
    {
        SetScreen(controlsScreen);
    }

    public void OnResumeButton()
    {
        SetScreen();
        
        gameIsPaused = false;
    }

    public void OnMapButton()
    {
        SetScreen(mapScreen);
    }

    public void OnTitleScreenButton()
    {
        // change unity screen to title screen
    }

    public void OnDashControlsButton()
    {
        SetScreen(dashControlsScreen);
    }

    public void OnWallStickButton()
    {
        SetScreen(wallStickControlsScreen);
    }

    public void OnGlideButton()
    {
        SetScreen(glideControlsScreen);
    }
}
