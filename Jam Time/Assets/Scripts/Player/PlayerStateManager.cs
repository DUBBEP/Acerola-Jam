using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState activeState;
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerWalkState walkState = new PlayerWalkState();
    public PlayerFallState fallState = new PlayerFallState();
    public PlayerJumpState jumpState = new PlayerJumpState();

    public Rigidbody2D rig;
    public SpriteRenderer sr;

    public float horizontalVelocity = 0f;

    private float speedCap = 30f;
    


    // Start is called before the first frame update
    void Start()
    {

        activeState = idleState;

        activeState.EnterState(this); 
    }

    // Update is called once per frame
    void Update()
    {
        activeState.UpdateState(this);

        horizontalVelocity = rig.velocity.x;

        if (rig.velocity.x > speedCap)
            rig.velocity = new Vector2 (30, rig.velocity.y);
    }

    public void SwitchState(PlayerBaseState state)
    {
        activeState.ExitState(this);
        activeState = state;
        state.EnterState(this);
    }

    private void FixedUpdate()
    {
        activeState.PhysicsUpdate(this);
    }
}
