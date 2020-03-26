using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpState : BasicMovementState
{
    public WallJumpState(Controller c): base(c) { }
    protected float wallJumpTimer;
    public override void Enter() {
        wallJumpTimer = controller.WallJumpTime;
        controller.Speed = new Vector2(
                controller.Facing * -1 * controller.WallJumpSpeed * Mathf.Cos(controller.WallJumpDir),
                controller.WallJumpSpeed * Mathf.Sin(controller.WallJumpDir)
                ) + controller.WallVelocity / 2f;
        controller.Flip();
        controller.OnWallTimer -= controller.OnWallTime / 4f;
    }

    public override bool Update() {
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

    public override void Exit() {
        controller.Jump = false;
    }
}
