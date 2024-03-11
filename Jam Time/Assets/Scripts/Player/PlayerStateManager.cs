using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState activeState;
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerWalkState walkState = new PlayerWalkState();
    public PlayerFallState fallState = new PlayerFallState();
    public PlayerJumpState jumpState = new PlayerJumpState();
    public PlayerPivotDashState pivotDashState = new PlayerPivotDashState();
    public PlayerKnockBackState knockBackState = new PlayerKnockBackState();
    public PlayerWallStickState wallStickState = new PlayerWallStickState();
    public PlayerGlideState glideState = new PlayerGlideState();
    public PlayerDamageState damageState = new PlayerDamageState();


    [Header("Parameters")]
    public float walkSpeed = 3f;
    public float speedCap = 30f;
    public float jumpForce = 33f;
    public float jumpingAirSpeed = 3f;
    public float jumpGravityMultiplyer = 7f;
    public float jumpBuffer = 0.15f;
    public float fallingAirSpeed = 3f;
    public float glideAirSpeed = 1.5f;
    public float wallCrawlSpeed = 5f;
    public float wallCoyoteTime = 0.15f;
    

    [Header("Components")]
    public Rigidbody2D rig;
    public SpriteRenderer sr;
    public PlayerController playerController;


    [Header("Progression")]
    public bool pivotDashAquired;
    public bool wallStickAquired;
    public bool glideAquired;



    [Header("Info")]
    public bool airJumpAvailable;
    public bool glideRenewed;
    public bool dashRenewed;
    public bool isGrounded;
    public bool isFacingRight;
    public bool controlsActive;
    public float curXVel = 0f;
    public float xInput;
    public float yInput;
    public float wallCoyoteWindow = 0f;
    public float pauseControlTime = 0f;

    private bool playerIsPaused;
    





    // Start is called before the first frame update
    void Start()
    {
        pivotDashAquired = true;
        wallStickAquired = true;
        glideAquired = true;


        controlsActive = true;

        activeState = idleState;

        activeState.EnterState(this); 
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.dead)
            controlsActive = false;
        else 
            controlsActive = true;

        if (controlsActive)
        {
            activeState.UpdateState(this);
            xInput = Input.GetAxis("Horizontal");
            yInput = Input.GetAxis("Vertical");
        }

        if (pauseControlTime > 0f)
        {
            controlsActive = false;
            playerIsPaused = true; ;
            pauseControlTime -= Time.deltaTime;
            xInput = 0;
            yInput = 0;
        }
        else if (pauseControlTime < 0f && playerIsPaused)
        {
            controlsActive = true;
            playerIsPaused = false;
        }

        curXVel = rig.velocity.x;


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

        if (Mathf.Abs(rig.velocity.x) > speedCap)
        {
            if (rig.velocity.x > 0)
                rig.velocity = new Vector2(rig.velocity.x - 3, rig.velocity.y);
            else if (rig.velocity.x < 0)
                rig.velocity = new Vector2(rig.velocity.x + 3, rig.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        activeState.OnCollisionEnter2D(this, collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        activeState.OnCollisionStay2D(this, collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        activeState.OnCollisionExit2D(this, collision);
    }





    public void CheckPivotDash()
    {
        if (!pivotDashAquired)
            return;
        
        

        if (rig.velocity.x > -12f && Input.GetKey(KeyCode.I) && xInput < 0)
            SwitchState(pivotDashState);
        else if (rig.velocity.x <= 12f && Input.GetKey(KeyCode.I) && xInput > 0)
            SwitchState(pivotDashState);
    }

    public void CheckWallStick()
    {

        if (wallCoyoteWindow >= 0)
            wallCoyoteWindow -= Time.deltaTime;

        if (!wallStickAquired) 
            return;

        if (Input.GetKeyDown(KeyCode.O) && wallCoyoteWindow >= 0)
            SwitchState(wallStickState);
    }


    public void AirMovement(float speed)
    {
        float airAcceleration = xInput * speed;
        if (Mathf.Abs(rig.velocity.x) < 15f)
            rig.velocity += new Vector2(airAcceleration, 0);
          
        // Results in player being able to hop at increased speeds without use of the dash.
        //else
        //    rig.AddForce(new Vector2(Input.GetAxis("Horizontal"), 0) * (airSpeed * 5f));

        if ((rig.velocity.x > 0 && airAcceleration < 0) || (rig.velocity.x < 0 && airAcceleration > 0))
            rig.velocity += new Vector2(airAcceleration, 0);
    }

    public void CheckFacingDirection()
    {
        if (xInput > 0)
            isFacingRight = true;
        else if (xInput < 0)
            isFacingRight = false;
    }

    public bool CheckForGround()
    {
        Vector2 leftRay = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.55f);
        Vector2 rightRay = new Vector2(transform.position.x + 0.5f, transform.position.y - 0.55f);

        if ((Physics2D.Raycast(leftRay, Vector2.down, 0.5f) || Physics2D.Raycast(rightRay, Vector2.down, 0.5f)))
            return true;
        else
            return false;
    }

    public void SetDefaultgravity(float multiplyer = 1f)
    {
        rig.gravityScale = 6f * multiplyer;
    }

}
