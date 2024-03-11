using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPivotDashState : PlayerBaseState
{
    float dashForce = 50f;
    float floatTime;
    float exitTime;
    bool movingRightPressingLeft;
    bool movingleftPressingRight;

    Vector2 dashDirection;
    

    public override void EnterState(PlayerStateManager player)
    {

        if (player.dashRenewed)
            floatTime = 0.2f;
        else
            floatTime = 0f;


        exitTime = 0.3f;

        // get the horizontal direction of the player
        dashDirection = new Vector2(Input.GetAxis("Horizontal"), 0).normalized;

        if (player.dashRenewed)
            player.rig.velocity = new Vector2(0, 0);
        else
            player.rig.velocity = new Vector2(0, player.rig.velocity.y * 1/3);


        // dash in that direction and set gravity to zero
        player.rig.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);
        
        player.CheckFacingDirection();

        if (floatTime > 0)
            player.rig.gravityScale = 0f;

        player.sr.color = Color.magenta;

        if (!player.isGrounded)
            player.dashRenewed = false;
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.SetDefaultgravity();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        floatTime -= Time.deltaTime;
        exitTime -= Time.deltaTime;

        if (floatTime <= 0f)
            player.rig.gravityScale = 6f;

        if (exitTime <= 0f)
            player.SwitchState(player.fallState);

        if (Input.GetKeyDown(KeyCode.Space) && player.airJumpAvailable)
        {
            if (!player.isGrounded)
                player.airJumpAvailable = false;
            
            player.SwitchState(player.jumpState);
        }

        if (player.isGrounded)
        {
            if (player.rig.velocity.x > 0f && Input.GetAxis("Horizontal") < 0f)
                movingRightPressingLeft = true;
            else
                movingRightPressingLeft = false;

            if (player.rig.velocity.x < 0f && Input.GetAxis("Horizontal") > 0f)
                movingleftPressingRight = true;
            else
                movingleftPressingRight = false;

            if (movingleftPressingRight || movingRightPressingLeft)
                player.SwitchState(player.pivotDashState);
        }

    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        return;
    }

    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision)
    {
        Debug.Log("Checking for object ahead");

        RaycastHit2D hit1;
        RaycastHit2D hit2;

        if (player.isFacingRight)
        {
            hit1 = Physics2D.Raycast(new Vector2(player.transform.position.x + 0.55f, player.transform.position.y + 0.4f), Vector2.right, 0.5f);
            hit2 = Physics2D.Raycast(new Vector2(player.transform.position.x + 0.55f, player.transform.position.y - 0.4f), Vector2.right, 0.5f);

        }
        else
        {
            hit1 = Physics2D.Raycast(new Vector2(player.transform.position.x - 0.55f, player.transform.position.y + 0.4f), Vector2.left, 0.5f);
            hit2 = Physics2D.Raycast(new Vector2(player.transform.position.x - 0.55f, player.transform.position.y - 0.4f), Vector2.left, 0.5f);

        }

        if (hit1 || hit2)
            player.SwitchState(player.knockBackState);
    }

    public override void OnCollisionExit2D(PlayerStateManager player, Collision2D collision)
    {
        return;
    }

    public override void OnCollisionStay2D(PlayerStateManager player, Collision2D collision)
    {
        return;
    }
}
