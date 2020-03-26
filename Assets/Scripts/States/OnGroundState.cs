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
            if (controller.Speed.x != controller.LiftVelocity.x) {
                OnGroundSlowDown();
            }
        } else if (Mathf.Abs(controller.Speed.x) >= controller.MaxRun + Mathf.Abs(controller.LiftVelocity.x)
            && controller.Movement.x * controller.Speed.x > 0) {
            // Decrease speed to max;
            controller.Speed = new Vector2((controller.MaxRun + Mathf.Abs(controller.LiftVelocity.x)) * Mathf.Sign(controller.Speed.x), controller.Speed.y);
        } else {
            // Increase speed to max;
            float vx = Mathf.Lerp(controller.Speed.x, Mathf.Sign(controller.Movement.x) * (controller.MaxRun + Mathf.Abs(controller.LiftVelocity.x)), controller.RunAccel * Time.fixedDeltaTime);
            controller.Speed = new Vector2(vx, controller.Speed.y);
            if (controller.Movement.x * controller.Facing < 0) {
                controller.Flip();
            }
        }

        return true;
    }

    private void OnGroundSlowDown() {
        float vx = Mathf.Lerp(controller.Speed.x, controller.LiftVelocity.x, controller.RunReduce * Time.fixedDeltaTime);
        if (Mathf.Abs(vx - controller.LiftVelocity.x) < 0.01) {
            controller.Speed = new Vector2(controller.LiftVelocity.x, controller.Speed.y);
        } else {
            controller.Speed = new Vector2(vx, controller.Speed.y);
        }
    }


    public override string ToString() {
        return "OnGround";
    }
}
