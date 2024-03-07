using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    float jumpForce = 30f;
    float jumpingAirSpeed = 0f;
    public override void EnterState(PlayerStateManager player)
    {


        Debug.Log("Hello From Jump");

        // change color to yellow
        player.sr.color = new Color(0.905f, 0.945f, 0.458f, 1);


        // set vertical velocity to zero when decending so jump is consistent

        if (player.rig.velocity.y < 0)
            player.rig.velocity = new Vector2(player.rig.velocity.x, 0f);

        // Add vertical force for jump
        player.rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        player.rig.gravityScale = 7f;
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.rig.gravityScale = 1f;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        // switch to falling if y velocity is negative
        if (player.rig.velocity.y <= 0)
            player.SwitchState(player.fallState);
    }
    public override void PhysicsUpdate(PlayerStateManager player)
    {
        // Movement in the air
        jumpingAirSpeed = Input.GetAxis("Horizontal") * 10f;
        player.rig.AddForce(new Vector2(jumpingAirSpeed, 0f));
    }

    public override void OnCollisionEnter2D(PlayerStateManager player)
    {

    }
}
