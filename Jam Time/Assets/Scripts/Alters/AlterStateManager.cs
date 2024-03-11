using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterStateManager : MonoBehaviour
{
    AlterBaseState activeState;
    public AlterEmptyState emptyState = new AlterEmptyState();
    public AlterHoldingState holdingState = new AlterHoldingState();

    public SpriteRenderer itemVisual;
    public GameObject targetPlayer;

    public bool playerInRange;


    private void Start()
    {
        activeState = emptyState;
        activeState.EnterState(this);
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
}
