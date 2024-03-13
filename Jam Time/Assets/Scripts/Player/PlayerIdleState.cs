using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Hello From Idle");

        player.airJumpAvailable = true;
        player.dashRenewed = true;
        player.glideRenewed = true;
        player.isGrounded = true;

        player.sr.color = Color.cyan;

        /*
        player.ani.SetBool("isMoving", false);
        player.ani.SetBool("isAirborne", false);
        player.ani.SetBool("isFalling", false);
        */
    }

    public override void ExitState(PlayerStateManager player)
    {
        return;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) < 0.1f && player.controlsActive)
            player.rig.velocity /= new Vector2(2, 1);




        // cast a ray to check if we are on the ground and pressing space. So switch to jump state
        if (Input.GetKeyDown(KeyCode.Space) && player.CheckForGround())
            player.SwitchState(player.jumpState);

        // switch to fall when not on ground
        if (!player.CheckForGround())
            player.SwitchState(player.fallState);


        // Switch to walkstate when moving horizontally 

        if (Input.GetAxis("Horizontal") != 0)
            player.SwitchState(player.walkState);

        player.CheckWallStick();

        //switch to pivot dash
        player.CheckPivotDash();

    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        return;
    }
    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            player.wallCoyoteWindow = player.wallCoyoteTime * 1000;
    }

    public override void OnCollisionExit2D(PlayerStateManager player, Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            player.wallCoyoteWindow = player.wallCoyoteTime * 1000;
    }

    public override void OnCollisionStay2D(PlayerStateManager player, Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            player.wallCoyoteWindow = player.wallCoyoteTime * 1000;
    }
}
