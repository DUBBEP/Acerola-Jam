using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockBackState : PlayerBaseState
{
    float stunTimer;
    float bounceForce = 15f;
    public override void EnterState(PlayerStateManager player)
    {
        stunTimer = 0.5f;

        player.sr.color = Color.grey;

        player.rig.velocity = Vector2.zero;

        if (player.isFacingRight)
            player.rig.AddForce(new Vector2(-0.85f, 1.2f) * bounceForce, ForceMode2D.Impulse);
        else
            player.rig.AddForce(new Vector2(0.85f, 1.2f) * bounceForce, ForceMode2D.Impulse);
        
        player.SetDefaultgravity();
    }

    public override void ExitState(PlayerStateManager player)
    {
        return;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        stunTimer -= Time.deltaTime;

        if (stunTimer < 0)
        {
            if (player.CheckForGround())
                player.SwitchState(player.idleState);
            else
                player.SwitchState(player.fallState);
        }

    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        return;
    }

    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision)
    {
        return;
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
