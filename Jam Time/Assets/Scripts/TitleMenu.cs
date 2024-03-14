using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
        public GameObject titleScreen;
        public GameObject premiseScreen;


    public void OnStart()
    {
        premiseScreen.SetActive(true);
    }

    public void OnStartGame()
    {
        SceneManager.LoadScene("GameWorld");
    }

    public void OnQuit()
    {
        Application.Quit();
    } 
}
