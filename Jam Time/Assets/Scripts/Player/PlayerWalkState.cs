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
        Vector2 leftRay = new Vector2(player.transform.position.x - 0.5f, player.transform.position.y - 0.55f);
        Vector2 rightRay = new Vector2(player.transform.position.x + 0.5f, player.transform.position.y - 0.55f);

        // switch to jump when pressing space and on the ground. use ray cast to check if on ground.
        if (Input.GetKeyDown(KeyCode.Space) && (Physics2D.Raycast(leftRay, Vector2.down, 0.5f) || Physics2D.Raycast(rightRay, Vector2.down, 0.5f)))
            player.SwitchState(player.jumpState);

        if (Input.GetAxis("Horizontal") == 0)
            player.SwitchState(player.idleState);


        // switch to fall when not on the ground
        if (!Physics2D.Raycast(leftRay, Vector2.down, 0.5f) && !Physics2D.Raycast(rightRay, Vector2.down, 0.5f))
            player.SwitchState(player.fallState);


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
