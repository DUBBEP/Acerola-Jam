using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Hello From Idle");
        
        //change color to blue
        player.sr.color = new Color(0.145f, 0.584f, 0.623f, 1);
    }

    public override void ExitState(PlayerStateManager player)
    {

    }

    public override void UpdateState(PlayerStateManager player)
    {

        // cast a ray to check if we are on the ground and pressing space. So switch to jump state

        Vector2 leftRay = new Vector2(player.transform.position.x - 0.5f, player.transform.position.y - 0.55f);
        Vector2 rightRay = new Vector2(player.transform.position.x + 0.5f, player.transform.position.y - 0.55f);

        if (Input.GetKeyDown(KeyCode.Space) && (Physics2D.Raycast(leftRay, Vector2.down, 0.5f) || Physics2D.Raycast(rightRay, Vector2.down, 0.5f)))
            player.SwitchState(player.jumpState);

        // switch to fall when not on ground
        if (!Physics2D.Raycast(leftRay, Vector2.down, 0.5f) && !Physics2D.Raycast(rightRay, Vector2.down, 0.5f))
            player.SwitchState(player.fallState);


        // Switch to walkstate when moving horizontally 

        if (Input.GetAxis("Horizontal") != 0)
            player.SwitchState(player.walkState);


    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {

    }
    public override void OnCollisionEnter2D(PlayerStateManager player)
    {

    }
}
