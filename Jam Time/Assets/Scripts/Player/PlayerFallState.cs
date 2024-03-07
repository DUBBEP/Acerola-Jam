using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    float fallingAirSpeed = 0f;
    float lastJumpTime = 0f;
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Hello from fall state");

        player.sr.color = Color.red;

        // increase fall speed when decending
        player.rig.gravityScale = 6f;
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.rig.gravityScale = 1f;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        // If player tries to jump in the air then start buffer timer
        if (Input.GetKeyDown(KeyCode.Space))
            lastJumpTime = 0.15f;

        lastJumpTime -= Time.deltaTime;

        // Switch to grounded state
        Vector2 rayStart = new Vector2(player.transform.position.x, player.transform.position.y - 0.55f);
        if (Physics2D.Raycast(rayStart, Vector2.down, 0.5f))
        {
            // switch to walk if horizontal detected else switch to walk
            if (lastJumpTime >= 0)
                player.SwitchState(player.jumpState);
            else if (Input.GetAxis("Horizontal") != 0)
                player.SwitchState(player.walkState);
            else
                player.SwitchState(player.idleState);
        }
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        // falling movement
        fallingAirSpeed = Input.GetAxis("Horizontal") * 10f;
        player.rig.AddForce(new Vector2(fallingAirSpeed, 0f));
    }

    public override void OnCollisionEnter2D(PlayerStateManager player)
    {

    }
}
