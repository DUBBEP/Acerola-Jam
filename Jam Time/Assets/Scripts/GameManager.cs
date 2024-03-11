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

    [Header("GameStatus")]
    int playerAbilitiesHeld;


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
            playerAbilitiesHeld--;
        else if (!itemRemoved)
            playerAbilitiesHeld++;

        GameUI.instance.UpdateProgressionUI(alter, itemRemoved);
    }



}
