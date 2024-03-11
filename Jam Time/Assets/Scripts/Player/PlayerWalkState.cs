using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    float coyoteTime;

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Hello From Walk");

        player.airJumpAvailable = true;
        player.dashRenewed = true;
        player.isGrounded = true;

        // change color
        player.sr.color = Color.green;
    }

    public override void ExitState(PlayerStateManager player)
    {
        return;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.CheckFacingDirection();

        if (!player.CheckForGround())
            coyoteTime -= Time.deltaTime;
        else
            coyoteTime = 0.15f;

        // switch to jump when pressing space and on the ground. use ray cast to check if on ground.
        if (Input.GetKeyDown(KeyCode.Space) && coyoteTime >= 0)
            player.SwitchState(player.jumpState);

        if (player.xInput == 0)
            player.SwitchState(player.idleState);


        // switch to fall when not on the ground
        if (coyoteTime < 0)
            player.SwitchState(player.fallState);

        if (player.touchingTerrain)
            player.CheckWallStick();

        // Switch to airdash
        player.CheckPivotDash();

    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        float walkAccel = player.xInput * player.walkSpeed;

        // grounded movement
        if (Mathf.Abs(player.rig.velocity.x) <= 15)
            player.rig.velocity = new Vector2(walkAccel, player.rig.velocity.y);
    }

    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            player.touchingTerrain = true;
    }


    public override void OnCollisionStay2D(PlayerStateManager player, Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            player.touchingTerrain = true;
    }

    public override void OnCollisionExit2D(PlayerStateManager player, Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            player.touchingTerrain = false;
    }
}
