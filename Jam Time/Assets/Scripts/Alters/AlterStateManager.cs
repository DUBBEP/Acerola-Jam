using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterStateManager : MonoBehaviour
{
    public enum AlterType
    {
        pivotDash,
        wallStick,
        glide
    }

    public AlterType type;
    
    AlterBaseState activeState;
    public AlterEmptyState emptyState = new AlterEmptyState();
    public AlterHoldingState holdingState = new AlterHoldingState();

    public SpriteRenderer itemVisual;
    public Transform alterRespawnPoint;

    GameObject targetPlayer;


    public bool playerInRange;




    private void Start()
    {
        activeState = emptyState;
        activeState.EnterState(this);
        targetPlayer = FindObjectOfType<PlayerStateManager>().gameObject;
    }

    private void Update()
    {
        activeState.UpdateState(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        activeState.OnTriggerEnter2D(this, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        activeState.OnTriggerExit2D(this, collision);
    }

    public void SwitchState(AlterBaseState newState)
    {
        activeState.ExitState(this);
        activeState = newState;
        activeState.EnterState(this);
    }

    public void AlterToggle(bool toggle)
    {
        PlayerStateManager player = targetPlayer.GetComponent<PlayerStateManager>();

        if (type == AlterType.pivotDash)
            player.pivotDashAquired = toggle;
        else if ( type == AlterType.wallStick)
            player.wallStickAquired = toggle;
        else if ( type == AlterType.glide)
            player.glideAquired = toggle;

        GameManager.instance.CheckGameProgress(this, toggle);
        GameManager.instance.spawnPoint = alterRespawnPoint;
    }
}
