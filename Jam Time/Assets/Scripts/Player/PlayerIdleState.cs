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
        player.isGrounded = true;
        
        //change color to blue
        player.sr.color = new Color(0.145f, 0.584f, 0.623f, 1);
    }

    public override void ExitState(PlayerStateManager player)
    {
        return;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) < 0.1f)
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

        if (player.touchingTerrain)
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
            player.touchingTerrain = true;
    }

    public override void OnCollisionExit2D(PlayerStateManager player, Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            player.touchingTerrain = false;
    }

    public override void OnCollisionStay2D(PlayerStateManager player, Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            player.touchingTerrain = true;
    }
}
