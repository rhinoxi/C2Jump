using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideState : BasicMovementState
{
    public WallSlideState(Controller c) : base(c) { }

    public override bool Update() {
        if (!base.Update()) {
            return false;
        }

        if (controller.CanOnWall()) {
            if (controller.ClimbCheck()) {
                controller.SetState(controller.stClimb);
                return false;
            }
            else if (!controller.OnWall || controller.Movement.x * controller.Facing <= 0) {
                controller.SetState(controller.stFall);
                return false;
            }
        }

        if (controller.OnGround) {
            controller.SetState(controller.stIdle);
            return false;
        }
        else if (controller.WallJumpCheck()) {

            if (controller.WallBoost.sqrMagnitude > Vector2.kEpsilon) {
                controller.SetState(controller.stBoostWallJump);
            } else {
                controller.SetState(controller.stWallJump);
            }
            return false;
        } else if (controller.OnWallTimer <= 0) {
            controller.SetState(controller.stFall);
            return false;
        }

        controller.Speed = Vector2.down * controller.WallSlideSpeed + controller.WallVelocity;
        controller.OnWallTimer -= Time.fixedDeltaTime;
        return true;
    }

    public override string ToString() {
        return "WallSlide";
    }

    public override int ID => 7;
}
