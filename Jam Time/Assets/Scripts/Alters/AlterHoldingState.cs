using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterHoldingState : AlterBaseState
{
    public override void EnterState(AlterStateManager alter)
    {
        alter.itemVisual.enabled = true;
    }

    public override void ExitState(AlterStateManager alter)
    {

    }

    public override void OnTriggerEnter2D(AlterStateManager alter, Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            alter.playerInRange = true;
            GameUI.instance.PromptItemPlacement(true);
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
            GameUI.instance.PromptItemPlacement(false);
            alter.AlterToggle(true);
            alter.itemVisual.enabled = false;
            alter.SwitchState(alter.emptyState);
        }
    }
}
