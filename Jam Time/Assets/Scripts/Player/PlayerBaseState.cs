using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateManager player);
    public abstract void UpdateState(PlayerStateManager player);
    public abstract void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision);
    public abstract void OnCollisionStay2D(PlayerStateManager player, Collision2D collision);
    public abstract void OnCollisionExit2D(PlayerStateManager player, Collision2D collision);
    public abstract void PhysicsUpdate(PlayerStateManager player);
    public abstract void ExitState(PlayerStateManager player);
}