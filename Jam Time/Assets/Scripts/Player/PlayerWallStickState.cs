using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWallStickState : PlayerBaseState
{
    bool onLedge;
    bool exitingState;

    Vector2 ledgePos;

    float rayOffSet = 0.52f;
    float rayCastDistance = 0.02f;
    float crawlAccelMult = 4f;

    int surfacesInContact;

    string previousSurfaceType;

    RaycastHit2D currentSurface;

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Hello from wallstick");

        player.sr.color = Color.black;

        player.rig.gravityScale = 0f;

        exitingState = false;

    }

    public override void ExitState(PlayerStateManager player)
    {
        player.SetDefaultgravity();

        exitingState = true;
    }

    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision)
    {
        return;
    }

    public override void OnCollisionStay2D(PlayerStateManager player, Collision2D collision)
    {
        return;
    }

    public override void OnCollisionExit2D(PlayerStateManager player, Collision2D collision)
    {
        return;
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        return;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (Input.GetKeyDown(KeyCode.O))
            player.SwitchState(player.fallState);

        if (Input.GetKeyDown(KeyCode.Space))
            player.SwitchState(player.jumpState);

        player.CheckPivotDash();

        // create rays for each side of the character
        Ray2D leftRay = new Ray2D(new Vector2(player.transform.position.x - rayOffSet, player.transform.position.y), Vector2.left);
        Ray2D rightRay = new Ray2D(new Vector2(player.transform.position.x + rayOffSet, player.transform.position.y), Vector2.right);
        Ray2D upRay = new Ray2D(new Vector2(player.transform.position.x, player.transform.position.y + rayOffSet), Vector2.up);
        Ray2D downRay = new Ray2D(new Vector2(player.transform.position.x, player.transform.position.y - rayOffSet), Vector2.down);

        // cast rays and capture the hits
        RaycastHit2D leftWallDetect = Physics2D.Raycast(leftRay.origin, leftRay.direction, rayCastDistance);
        RaycastHit2D rightWallDetect = Physics2D.Raycast(rightRay.origin, rightRay.direction, rayCastDistance);
        RaycastHit2D ceilingDetect = Physics2D.Raycast(upRay.origin, upRay.direction, rayCastDistance);
        RaycastHit2D floorDetect = Physics2D.Raycast(downRay.origin, downRay.direction, rayCastDistance);

        Debug.DrawRay(leftRay.origin, leftRay.direction, Color.red, rayCastDistance);
        Debug.DrawRay(rightRay.origin, rightRay.direction, Color.red, rayCastDistance);
        Debug.DrawRay(upRay.origin, upRay.direction, Color.red, rayCastDistance);
        Debug.DrawRay(downRay.origin, downRay.direction, Color.red, rayCastDistance);

        surfacesInContact = FindCurrentWall(leftWallDetect, rightWallDetect, ceilingDetect, floorDetect);

        // find how many walls are in contact.
        if (surfacesInContact > 0 || (surfacesInContact == 0 && FindOuterCorner(player.transform.position) == 0))
            onLedge = false;

        // If we detect a wall and ceiling then we must be on a corner
        if (surfacesInContact > 1)
            CornerLogic(FindInnerCorner(leftWallDetect, rightWallDetect, ceilingDetect, floorDetect), player);

        // allow vertical movement on walls
        else if (surfacesInContact > 0 && (currentSurface == leftWallDetect || currentSurface == rightWallDetect))
        {
            float crawlSpeed = player.yInput * player.wallCrawlSpeed;

            if (player.yInput != 0 && !exitingState)
                player.rig.velocity = new Vector2(0, crawlSpeed);



            CheckPlayerDistance("horizontal", player);
        }

        // allow horizontal movement on ceilings
        else if (surfacesInContact > 0 && (currentSurface == ceilingDetect || currentSurface == floorDetect))
        {
            float crawlSpeed = player.xInput * player.wallCrawlSpeed;

            if (player.xInput != 0 && !exitingState)
                player.rig.velocity = new Vector2(crawlSpeed, 0);

            CheckPlayerDistance("vertical", player);

        }

        // If the wall we're on dissapears then assume its a ledge
        else if (surfacesInContact == 0)
        {
            // if we find a ledge then run the ledge logic
            if (FindOuterCorner(player.transform.position) > 0)
            {
                if (!onLedge)
                {
                    ledgePos = player.transform.position;
                    onLedge = true;
                }


                // add ledge logic
                EdgeLogic(FindOuterCorner(player.transform.position), player);
            }

        }
        
        if (surfacesInContact == 0 && !onLedge)
            player.SwitchState(player.fallState);



        // If we're not moving then transition to idle.
    }


    void CheckPlayerDistance(string wallType, PlayerStateManager player)
    {
        float distanceFromWall = 0f;

        if (wallType == "vertical")
            distanceFromWall = player.transform.position.y - currentSurface.point.y;
        else if (wallType == "horizontal")
            distanceFromWall = player.transform.position.x - currentSurface.point.x;

        if (Mathf.Abs(distanceFromWall) > 0.51f)
            CorrectPlayerDistance(distanceFromWall, wallType, player);
    }

    void CorrectPlayerDistance(float distanceFromWall, string wallType, PlayerStateManager player)
    {
        float correctionValue = Mathf.Abs(distanceFromWall) - 0.52f;

        if (wallType == "vertical")
        {
            if (distanceFromWall > 0)
                player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y - correctionValue);
            else
                player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + correctionValue);
        }
        else if (wallType == "horizontal")
        {
            if (distanceFromWall > 0)
                player.transform.position = new Vector2(player.transform.position.x - correctionValue, player.transform.position.y);
            else
                player.transform.position = new Vector2(player.transform.position.x + correctionValue, player.transform.position.y);
        }

    }


    // function will find the number of walls in contact and return that value.
    int FindCurrentWall(RaycastHit2D left, RaycastHit2D right, RaycastHit2D up, RaycastHit2D down)
    {
        int numberOfWallsInContact = 0;

        if (left)
        {
            numberOfWallsInContact++;
            currentSurface = left;
            previousSurfaceType = "left";
        }
        if (right)
        {
            numberOfWallsInContact++;
            currentSurface = right;
            previousSurfaceType = "right";
        }
        if (up)
        {
            numberOfWallsInContact++;
            currentSurface = up;
            previousSurfaceType = "up";
        }
        if (down)
        {
            numberOfWallsInContact++;
            currentSurface = down;
            previousSurfaceType = "down";
        }

        return numberOfWallsInContact;
    }

    // this function determines how the player recognizes which corner they are in
    int FindInnerCorner(RaycastHit2D left, RaycastHit2D right, RaycastHit2D up, RaycastHit2D down)
    {
        int curCorner = 0;

        if (up && left)
            curCorner = 1;
        else if (up && right)
            curCorner = 2;
        else if (down && left)
            curCorner = 3;
        else if (down && right)
            curCorner = 4;

        return curCorner;
    }

    // this function determines how the player should move in each corner
    void CornerLogic(int curCorner, PlayerStateManager player)
    {
        Vector2 movement = new Vector2(player.xInput * player.wallCrawlSpeed, player.yInput * player.wallCrawlSpeed);


        switch (curCorner)
        {
            case 0:
                return;
            case 1: // A top left corner
                if (player.xInput > 0 && player.yInput < 0)
                    return;
                else
                    player.rig.velocity = movement;
                break;
            case 2: // a top right corner
                if (player.xInput < 0 && player.yInput < 0)
                    return;
                else
                    player.rig.velocity = movement;
                break;
            case 3: // a bottom left corner
                if (player.xInput > 0 && player.yInput > 0)
                    return;
                else
                    player.rig.velocity = movement;
                break;
            case 4: // a bottom right corner
                if (player.xInput < 0 && player.yInput > 0)
                    return;
                else
                    player.rig.velocity = movement;
                break;
        }
    }


    // This function finds the corner that the player is on as well as what direction the player is heading in using rays and a reference from the most previous surface interaction
    int FindOuterCorner(Vector2 playerPos)
    {
        rayCastDistance = 0.6f;


        if (previousSurfaceType == "left")
        {
            RaycastHit2D topHit = Physics2D.Raycast(playerPos + new Vector2(-0.52f, 0.52f), new Vector2(-1, 1), rayCastDistance);
            RaycastHit2D bottomHit = Physics2D.Raycast(playerPos + new Vector2(-0.52f, -0.52f), new Vector2(-1, -1), rayCastDistance);

            Debug.DrawRay(playerPos + new Vector2(-0.52f, 0.52f), new Vector2(-1, 1), Color.red, 0.2f);
            Debug.DrawRay(playerPos + new Vector2(-0.52f, -0.52f), new Vector2(-1, -1), Color.red, 0.2f);



            if (topHit)
                return 1; // bottom right corner moving down
            else if (bottomHit)
                return 2; // top right corner moving up
        }
        else if (previousSurfaceType == "right")
        {
            RaycastHit2D topHit = Physics2D.Raycast(playerPos + new Vector2(0.52f, 0.52f), new Vector2(1, 1), rayCastDistance);
            RaycastHit2D bottomHit = Physics2D.Raycast(playerPos + new Vector2(0.52f, -0.52f), new Vector2(1, -1), rayCastDistance);

            Debug.DrawRay(playerPos + new Vector2(0.52f, 0.52f), new Vector2(1, 1), Color.red, 0.2f);
            Debug.DrawRay(playerPos + new Vector2(0.52f, -0.52f), new Vector2(1, -1), Color.red, 0.2f);


            if (topHit)
                return 3; // bottom left corner moving down
            else if (bottomHit)
                return 4; // top left corner moving up
        }
        else if (previousSurfaceType == "up")
        {
            RaycastHit2D leftHit = Physics2D.Raycast(playerPos + new Vector2(-0.52f, 0.52f), new Vector2(-1, 1), rayCastDistance);
            RaycastHit2D rightHit = Physics2D.Raycast(playerPos + new Vector2(0.52f, 0.52f), new Vector2(1, 1), rayCastDistance);

            Debug.DrawRay(playerPos + new Vector2(-0.52f, 0.52f), new Vector2(-1, 1), Color.red, 0.2f);
            Debug.DrawRay(playerPos + new Vector2(0.52f, 0.52f), new Vector2(1, 1), Color.red, 0.2f);


            if (leftHit)
                return 5; // bottom right corner moving right
            else if (rightHit)
                return 6; // bottom left corner moving left
        }
        else if (previousSurfaceType == "down")
        {
            RaycastHit2D topHit = Physics2D.Raycast(playerPos + new Vector2(0.52f, -0.52f), new Vector2(1, -1), rayCastDistance);
            RaycastHit2D bottomHit = Physics2D.Raycast(playerPos + new Vector2(-0.52f, -0.52f), new Vector2(-1, -1), rayCastDistance);

            Debug.DrawRay(playerPos + new Vector2(0.52f, -0.52f), new Vector2(1, -1), Color.red, 0.2f);
            Debug.DrawRay(playerPos + new Vector2(-0.52f, -0.52f), new Vector2(-1, -1), Color.red, 0.2f);


            if (topHit)
                return 8; // top right corner moving right
            else if (bottomHit)
                return 7; // top left corner moving left
        }
        
        return 0;
    }

    // this function determinse organizes how the player should act in each corner
    void EdgeLogic(int curCorner, PlayerStateManager player)
    {

        switch (curCorner)
        {
            case 0:
                return;
            case 1: // bottom right corner moving down
                EdgeMoveLogic(false, false, "down", "bottomRight", player);
                break;
            case 2: // top right corner moving up
                EdgeMoveLogic(true, false, "up", "topRight", player);
                break;
            case 3: // bottom left corner moving down
                EdgeMoveLogic(false, true, "down", "bottomLeft", player);
                break;
            case 4: // top left corner moving up
                EdgeMoveLogic(true, true, "up", "topLeft", player);
                break;
            case 5: // bottom right corner moving right
                EdgeMoveLogic(false, false, "right", "bottomRight", player);
                break;
            case 6: // bottom left corner moving left
                EdgeMoveLogic(false, true, "left", "bottomLeft", player);
                break;
            case 7: // top right corner moving right
                EdgeMoveLogic(true, false, "right", "topRight", player);
                break;
            case 8: // top left corner moving left
                EdgeMoveLogic(true, true, "left", "topLeft", player);
                break;
        }
    }

    // this function determines how the player should move given a certain corner configuration
    void EdgeMoveLogic(bool topCorner, bool leftCorner, string direction, string actualCorner, PlayerStateManager player)
    {
        float xPosLimit = 0;
        float yPosLimit = 0;

        Vector2 playerPos = player.transform.position;
        Vector2 movement = new Vector2(player.xInput * player.wallCrawlSpeed, player.yInput * player.wallCrawlSpeed);
        
        // figure out our limits
        switch (direction)
        {
            case "up": // going up
                xPosLimit = ledgePos.x;
                yPosLimit = ledgePos.y + 0.51f;
                break;
            case "down": // going down
                xPosLimit = ledgePos.x;
                yPosLimit = ledgePos.y - 0.51f;
                break;
            case "left": // going left
                xPosLimit = ledgePos.x - 0.51f;
                yPosLimit = ledgePos.y;
                break;
            case "right": // going right
                xPosLimit = ledgePos.x + 0.51f;
                yPosLimit = ledgePos.y ;
                break;
        }

        // figuring out when to call the player back into the limits
        switch (topCorner)
        {
            case true: // top corner
                if (playerPos.y > yPosLimit)
                    player.transform.position = new Vector2(player.transform.position.x, yPosLimit);
                break;
            case false: // bottom corner
                if (playerPos.y < yPosLimit)
                    player.transform.position = new Vector2(player.transform.position.x, yPosLimit);
                break;
        }
                
                
        switch (leftCorner)
        {
            case true: // left corner
                if (playerPos.x < xPosLimit)
                    player.transform.position = new Vector2(xPosLimit, player.transform.position.y);
                break;
            case false: // right corner
                if (playerPos.x > xPosLimit)
                    player.transform.position = new Vector2(xPosLimit, player.transform.position.y);
                break;
        }

        // giving player movement if they are within the bounds of the corner
        switch (actualCorner)
        {
            case "topLeft": // top left corner
                if (playerPos.x >= xPosLimit && playerPos.y <= yPosLimit)
                    player.rig.velocity = movement;
                else
                    player.rig.velocity = Vector2.zero;
                break;
            case "topRight": // top right corner
                if (playerPos.x <= xPosLimit && playerPos.y <= yPosLimit)
                    player.rig.velocity = movement;
                else
                    player.rig.velocity = Vector2.zero;
                break;
            case "bottomLeft": // bottom left corner
                if (playerPos.x >= xPosLimit && playerPos.y >= yPosLimit)
                    player.rig.velocity = movement;
                else
                    player.rig.velocity = Vector2.zero;
                break;
            case "bottomRight": // bottom right corner
                if (playerPos.x <= xPosLimit && playerPos.y >= yPosLimit)
                    player.rig.velocity = movement;
                else
                    player.rig.velocity = Vector2.zero;
                break;
        }

    }
}