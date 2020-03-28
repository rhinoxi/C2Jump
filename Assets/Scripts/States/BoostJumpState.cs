using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostJumpState : JumpState
{
    public BoostJumpState(Controller c): base(c) { }

    private float gravityBefore;

    public override void Enter() {
        base.Enter();
        gravityBefore = controller.Gravity;
        controller.Gravity = 0;
    }

    protected override void Jump() {
        float vy = Mathf.Lerp(0, controller.JumpSpeed + liftVelocity.y +  controller.LiftBoost.y * Time.fixedDeltaTime, jumpTimer / controller.JumpTime);
        float vx = controller.Speed.x;
        if (controller.Movement.x * controller.LiftBoost.x > 0) {
            vx += controller.LiftBoost.x * Time.fixedDeltaTime;
        }
        controller.Speed = new Vector2(vx, vy);
    }

    public override void Exit() {
        base.Exit();
        controller.Gravity = gravityBefore;
    }

    public override string ToString() {
        return "BoostJump";
    }

    public override int ID => 9;
}
