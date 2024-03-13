using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Components")]
    private PlayerStateManager playerState;
    private PlayerController playerController;
    public Transform spawnPoint;
    public EndDoor endDoor;

    [Header("GameStatus")]
    public int playerAbilitiesHeld;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        playerState = FindObjectOfType<PlayerStateManager>();
        playerController = FindObjectOfType<PlayerController>();

        playerAbilitiesHeld = 3;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckGameProgress(AlterStateManager alter, bool itemRemoved)
    {
        if (itemRemoved)
            playerAbilitiesHeld++;
        else if (!itemRemoved)
            playerAbilitiesHeld--;

        if (playerAbilitiesHeld <= 0)
            EndGameActive(true);
        else if (playerAbilitiesHeld > 0)
            EndGameActive(false);

        GameUI.instance.UpdateProgressionUI(alter, itemRemoved);
    }

    void EndGameActive(bool toggle)
    {
        // set endgame text
        GameUI.instance.endGamePrompt.SetActive(toggle);

        // set door 
        endDoor.doorActive = toggle;
        

    }

    public void EndGame()
    {
        // load the Title Screen

        // Save time
    }

}
