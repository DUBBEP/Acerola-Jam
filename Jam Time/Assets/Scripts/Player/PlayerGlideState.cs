using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlideState : PlayerBaseState
{
    float lastJumpTime;

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Hello from glide state");

        player.sr.color = Color.white;

        if (player.glideRenewed)
            player.rig.velocity = new Vector2(player.rig.velocity.x, 0);
        else
            player.rig.velocity = new Vector2(player.rig.velocity.x, player.rig.velocity.y * 1/6);

        player.rig.gravityScale = 1f;

        player.glideRenewed = false;
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.SetDefaultgravity();
    }

    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            player.wallCoyoteWindow = player.wallCoyoteTime;
    }

    public override void OnCollisionExit2D(PlayerStateManager player, Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            player.wallCoyoteWindow = player.wallCoyoteTime;
    }

    public override void OnCollisionStay2D(PlayerStateManager player, Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            player.wallCoyoteWindow = player.wallCoyoteTime;
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        player.AirMovement(player.glideAirSpeed);

    }

    public override void UpdateState(PlayerStateManager player)
    {

        player.CheckPivotDash();

        player.CheckWallStick();

        if (Input.GetKeyUp(KeyCode.Space) || !player.glideAquired)
            player.SwitchState(player.fallState);

        if (Input.GetKeyDown(KeyCode.Space))
            lastJumpTime = player.jumpBuffer;

        lastJumpTime -= Time.deltaTime;

        Vector2 leftRay = new Vector2(player.transform.position.x - 0.5f, player.transform.position.y - 0.55f);
        Vector2 rightRay = new Vector2(player.transform.position.x + 0.5f, player.transform.position.y - 0.55f);

        RaycastHit2D leftHit = Physics2D.Raycast(leftRay, Vector2.down, 0.5f);
        RaycastHit2D rightHit = Physics2D.Raycast(rightRay, Vector2.down, 0.5f);

        if (leftHit || rightHit)
        {
            if (lastJumpTime >= 0)
            {
                player.SwitchState(player.jumpState);
                player.airJumpAvailable = true;
                player.glideRenewed = true;
            }
            else if (player.xInput != 0)
                player.SwitchState(player.walkState);
            else
                player.SwitchState(player.idleState);
        }

    }
}
