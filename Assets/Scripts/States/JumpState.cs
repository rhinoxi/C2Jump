using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BasicMovementState
{
    public JumpState(Controller c): base(c) { }
    protected float jumpTimer;
    protected Vector2 liftVelocity;

    public override void Enter() {
        jumpTimer = controller.JumpTime;
        liftVelocity = controller.LiftVelocity;
    }

    public override bool Update() {
        if (!base.Update()) {
            return false;
        }

        if (controller.ClimbCheck()) {
            controller.SetState(controller.stClimb);
            return false;
        } else if (jumpTimer <= 0 && controller.OnGround) {
            controller.SetState(controller.stOnGround);
            return false;
        }

        if (controller.Jump && jumpTimer > 0) {
            Jump();
        } else {
            controller.SetState(controller.stFall);
            return false;
        } 

        jumpTimer -= Time.fixedDeltaTime;

        return true;
    }

    protected virtual void Jump() {
        float vy = Mathf.Lerp(0, controller.JumpSpeed + liftVelocity.y, jumpTimer / controller.JumpTime);
        controller.Speed = new Vector2(controller.Speed.x, vy);
    }

    public override void Exit() {
        jumpTimer = 0;
        controller.Jump = false;
    }

    public override string ToString() {
        return "Jump";
    }
}
