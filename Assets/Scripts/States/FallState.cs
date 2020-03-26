using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : BasicMovementState
{
    public FallState(Controller c): base(c) { }

    public override bool Update() {
        if (!base.Update()) {
            return false;
        }

        if (controller.CanOnWall()) {
            if (controller.ClimbCheck()) {
                controller.SetState(controller.stClimb);
                return false;
            }
            else if (controller.OnWall && controller.Movement.x * controller.Facing > 0) {
                controller.SetState(controller.stWallSlide);
                return false;
            }
        }

        if (controller.OnGround) {
            controller.SetState(controller.stOnGround);
            return false;
        }
        else if (controller.WallJumpCheck()) {
            if (controller.WallBoost.sqrMagnitude > Vector2.kEpsilon) {
                controller.SetState(controller.stBoostWallJump);
            } else {
                controller.SetState(controller.stWallJump);
            }
            return false;
        }

        // Reach max falling speed
        if (controller.Speed.y < controller.MaxFall) {
            controller.Speed = new Vector2(controller.Speed.x, controller.MaxFall);
        } else if (controller.Speed.y > 0) {
            controller.Speed = new Vector2(controller.Speed.x, Mathf.Lerp(controller.Speed.y, 0, 0.6f));
        }

        controller.InAirMovement();

        return true;
    }

    public override void Exit() {
        controller.Jump = false;
    }

    public override string ToString() {
        return "Fall";
    }
}
