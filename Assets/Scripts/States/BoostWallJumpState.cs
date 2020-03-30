using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostWallJumpState : WallJumpState
{
    public BoostWallJumpState(Controller c) : base(c) { }

    protected override void Jump() {
        controller.Velocity = new Vector2(
                controller.Facing * controller.WallJumpSpeed * Mathf.Cos(controller.WallJumpDir),
                controller.WallJumpSpeed * Mathf.Sin(controller.WallJumpDir)
                ) + controller.WallVelocity + controller.WallBoost * Time.fixedDeltaTime;
    }

    public override string ToString() {
        return "BoostWallJump";
    }

    public override int ID => 10;
}
