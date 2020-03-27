using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundState : BasicMovementState
{
    public OnGroundState(Controller c): base(c) { }

    public override void Enter() {
        controller.OnWallTimer = controller.OnWallTime;

        if (!controller.IsDashing) {
            controller.Dashable = true;
        }
    }

    public override bool Update() {
        if (!base.Update()) {
            return false;
        }

        if (controller.Jump) {
            if (controller.LiftBoost.sqrMagnitude > Vector2.kEpsilon) {
                controller.SetState(controller.stBoostJump);
                return false;
            }
            controller.SetState(controller.stJump);
            return false;
        } else if (controller.ClimbCheck()) {
            controller.SetState(controller.stClimb);
            return false; ;
        } else if (!controller.OnGround) {
            controller.SetState(controller.stFall);
            return false; ;
        }

        if (controller.Movement.x == 0) {
            controller.Speed = controller.LiftVelocity;
        } else {
            float maxVX;
            if (controller.Movement.x * controller.LiftVelocity.x > 0) {
                maxVX = Mathf.Sign(controller.Movement.x) * (controller.MaxRun + Mathf.Abs(controller.LiftVelocity.x));
            } else {
                maxVX = Mathf.Sign(controller.Movement.x) * (controller.MaxRun - Mathf.Abs(controller.LiftVelocity.x));
            }

            float vx;
            if (Mathf.Abs(controller.Speed.x) >= Mathf.Abs(maxVX)) {
                vx = Mathf.Lerp(controller.Speed.x, maxVX, controller.RunReduce * Time.fixedDeltaTime);
            }
            else {
                vx = Mathf.Lerp(controller.Speed.x, maxVX, controller.RunAccel * Time.fixedDeltaTime);
            }
            controller.Speed = new Vector2(vx, controller.Speed.y);

            if (controller.Movement.x * controller.Facing < 0) {
                controller.Flip();
            }

        }

        return true;
    }

    public override string ToString() {
        return "OnGround";
    }
}
