using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerFallState : PlayerBaseState
{
    float lastJumpTime = 0f;
    float groundContactPoint;
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Hello from fall state");

        player.isGrounded = false;
        
        // increase fall speed when decending
        player.SetDefaultgravity();

        player.ani.SetBool("isFalling", true);
        player.ani.SetBool("isAirborne", true);
        player.ani.SetBool("isJumping", false);
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.SetDefaultgravity();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.CheckFacingDirection();

        player.SetDefaultgravity();


        // If player tries to jump in the air then start buffer timer
        if (Input.GetKeyDown(KeyCode.Space))
            lastJumpTime = player.jumpBuffer;

        lastJumpTime -= Time.deltaTime;



        // Switch to grounded state

        Vector2 leftRay = new Vector2(player.transform.position.x - 0.5f, player.transform.position.y - 0.55f);
        Vector2 rightRay = new Vector2(player.transform.position.x + 0.5f, player.transform.position.y - 0.55f);

        RaycastHit2D leftHit = Physics2D.Raycast(leftRay, Vector2.down, 0.5f);
        RaycastHit2D rightHit = Physics2D.Raycast(rightRay, Vector2.down, 0.5f);

        if (leftHit || rightHit)
        {

            // find the vertical of the contact point
            if (leftHit)
                groundContactPoint = leftHit.point.y;
            else if (rightHit)
                groundContactPoint = rightHit.point.y;

            // set player velocity to zero and move them to the contact points vertical
            player.rig.velocity = new Vector2(player.rig.velocity.x, 0);
            player.transform.position = new Vector2(player.transform.position.x, groundContactPoint + 0.5f);

            // switch to walk if horizontal detected else switch to walk
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

        // switch to wall stick
        player.CheckWallStick();

        // switch to pivot dash
        player.CheckPivotDash();

        // switch to glide
        if (player.glideAquired && Input.GetKey(KeyCode.Space) && lastJumpTime < 0)
            player.SwitchState(player.glideState);

    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        // falling movement
        player.AirMovement(player.fallingAirSpeed);
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
