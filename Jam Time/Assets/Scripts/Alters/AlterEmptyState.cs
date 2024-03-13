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
            string message;
            if (alter.type == AlterStateManager.AlterType.pivotDash)
                message = "Place Pivot Dash: E";
            else if (alter.type == AlterStateManager.AlterType.wallStick)
                message = "Place Wall Stick: E";
            else if (alter.type == AlterStateManager.AlterType.glide)
                message = "Place Glide: E";
            else
                message = "No type";
            GameUI.instance.UIPrompt(message);

            alter.playerInRange = true;
        }
    }

    public override void OnTriggerExit2D(AlterStateManager alter, Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            alter.playerInRange = false;
            GameUI.instance.RemovePrompt();
        }
    }

    public override void UpdateState(AlterStateManager alter)
    {
        if (alter.playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            string message;
            if (alter.type == AlterStateManager.AlterType.pivotDash)
                message = "Pickup Pivot Dash: E";
            else if (alter.type == AlterStateManager.AlterType.wallStick)
                message = "Pickup Wall Stick: E";
            else if (alter.type == AlterStateManager.AlterType.glide)
                message = "Pickup Glide: E";
            else
                message = "No type";
            GameUI.instance.UIPrompt(message);

            alter.AlterToggle(false);
            alter.itemVisual.enabled = true;
            alter.SwitchState(alter.holdingState);
        }
    }
}
