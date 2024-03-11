using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterEmptyState : AlterBaseState
{
    public override void EnterState(AlterStateManager alter)
    {
        alter.itemVisual.enabled = false;
    }

    public override void ExitState(AlterStateManager alter)
    {
        return;
    }

    public override void OnTriggerEnter2D(AlterStateManager alter, Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            alter.playerInRange = true;
            GameUI.instance.PromptItemPlacement(false);
        }
    }

    public override void OnTriggerExit2D(AlterStateManager alter, Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            alter.playerInRange = false;
            GameUI.instance.RemoveItemPrompt();
        }
    }

    public override void UpdateState(AlterStateManager alter)
    {
        if (alter.playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            GameUI.instance.PromptItemPlacement(true);
            alter.targetPlayer.GetComponent<PlayerStateManager>().pivotDashAquired = false;
            alter.itemVisual.enabled = true;
            alter.SwitchState(alter.holdingState);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
