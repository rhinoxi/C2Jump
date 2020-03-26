using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostWallJumpState : WallJumpState
{
    public BoostWallJumpState(Controller c) : base(c) { }

    public override void Enter() {
        wallJumpTimer = controller.WallJumpTime;
        controller.Speed = new Vector2(
                controller.Facing * -1 * controller.WallJumpSpeed * Mathf.Cos(controller.WallJumpDir),
                controller.WallJumpSpeed * Mathf.Sin(controller.WallJumpDir)
                ) + controller.WallVelocity + controller.WallBoost * Time.fixedDeltaTime;
        Debug.LogFormat("{0}, {1}", controller.WallBoost.x, controller.WallBoost.y);
        controller.Flip();
        controller.OnWallTimer -= controller.OnWallTime / 4f;
    }

    public override string ToString() {
        return "BoostWallJump";
    }
}
