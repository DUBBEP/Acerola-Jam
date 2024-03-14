using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoor : MonoBehaviour
{
    public bool doorActive;

    bool playerInRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Ending Game");
            GameManager.instance.EndGame();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && doorActive)
        {
            playerInRange = true;
            GameUI.instance.UIPrompt("Leave: E");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && doorActive)
        {
            playerInRange = false;
            GameUI.instance.RemovePrompt();
        }
    }
}
