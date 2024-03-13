using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{   
    public override void EnterState(PlayerStateManager player)
    {


        Debug.Log("Hello From Jump");

        player.isGrounded = false;

        // set vertical velocity to zero when decending so jump is consistent

        if (player.rig.velocity.y < 0)
            player.rig.velocity = new Vector2(player.rig.velocity.x, 0f);

        // Add vertical force for jump
        player.rig.AddForce(Vector2.up * player.jumpForce, ForceMode2D.Impulse);

        player.rig.gravityScale = player.jumpGravityMultiplyer;

        player.sr.color = Color.yellow;


        /*            
        player.ani.SetBool("isAirborne", true);
        player.ani.SetBool("isJumping", true);
        player.ani.SetBool("isFalling", false);
        */
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.SetDefaultgravity();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.CheckFacingDirection();

        // switch to falling if y velocity is negative
        if (player.rig.velocity.y <= 0)
            player.SwitchState(player.fallState);

        if (Input.GetKey(KeyCode.Space))
            player.rig.gravityScale = player.jumpGravityMultiplyer;
        else
            player.rig.gravityScale = player.jumpGravityMultiplyer * 2f;

        player.CheckWallStick();

        //Switch to pivot dash
        player.CheckPivotDash();
    }
    public override void PhysicsUpdate(PlayerStateManager player)
    {
        // Movement in the air
        player.AirMovement(player.jumpingAirSpeed);
    }

    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            player.wallCoyoteWindow = player.wallCoyoteTime;
    }


    public override void OnCollisionStay2D(PlayerStateManager player, Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            player.wallCoyoteWindow = player.wallCoyoteTime;
    }

    public override void OnCollisionExit2D(PlayerStateManager player, Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            player.wallCoyoteWindow = player.wallCoyoteTime;
    }
}
