using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpState : BasicMovementState
{
    public WallJumpState(Controller c): base(c) { }
    protected float wallJumpTimer;
    public override void Enter() {
        wallJumpTimer = controller.WallJumpTime;
        controller.OnWallTimer -= controller.OnWallTime / 4f;
        controller.Flip();
    }

    public override bool Update() {
        if (wallJumpTimer == controller.WallJumpTime) {
            // There's some bug when putting this in Enter().
            // Have no idea.
            Jump();
        }

        if (!base.Update()) {
            return false;
        }

        wallJumpTimer -= Time.fixedDeltaTime;
        if (wallJumpTimer <= 0) {
            controller.SetState(controller.stFall);
            return false;
        }
        return true;
    }

    protected virtual void Jump() {
        controller.Velocity = new Vector2(
                controller.Facing * controller.WallJumpSpeed * Mathf.Cos(controller.WallJumpDir),
                controller.WallJumpSpeed * Mathf.Sin(controller.WallJumpDir)
                ) + controller.WallVelocity / 2f;
    }

    public override void Exit() {
        controller.Jump = false;
    }

    public override int ID => 5;
}
