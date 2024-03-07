using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    float moveSpeed = 0f;


    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Hello From Walk");

        // change color
        player.sr.color = Color.green;
    }

    public override void ExitState(PlayerStateManager player)
    {

    }

    public override void UpdateState(PlayerStateManager player)
    {
        // switch to jump when pressing space and on the ground. use ray cast to check if on ground.
        Vector2 rayStart = new Vector2(player.transform.position.x, player.transform.position.y - 0.55f);
        if (Input.GetKeyDown(KeyCode.Space) && Physics2D.Raycast(rayStart, Vector2.down, 0.5f))
            player.SwitchState(player.jumpState);

        if (Input.GetAxis("Horizontal") == 0)
            player.SwitchState(player.idleState);

    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        // grounded movement
        moveSpeed = Input.GetAxis("Horizontal") * 10;
        player.rig.velocity = new Vector2(moveSpeed, player.rig.velocity.y);
    }

    public override void OnCollisionEnter2D(PlayerStateManager player)
    {

    }
}
