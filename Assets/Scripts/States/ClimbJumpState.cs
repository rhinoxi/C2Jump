using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbJumpState : BasicMovementState
{
    public ClimbJumpState(Controller c): base(c) { }
    private float jumpTimer;

    public override void Enter() {
        jumpTimer = controller.ClimbJumpTime;
        controller.Speed = Vector2.up * controller.ClimbJumpSpeed + controller.WallVelocity;
        controller.OnWallTimer -= controller.OnWallTime / 4f;
    }

    public override bool Update() {
        if (!base.Update()) {
            return false;
        }

        jumpTimer -= Time.fixedDeltaTime;
        if (jumpTimer <= 0) {
            controller.SetState(controller.stFall);
            return false;
        }
        return true;
    }

    public override void Exit() {
        jumpTimer = 0;
        controller.Jump = false;
    }

    public override int ID => 6;
}
