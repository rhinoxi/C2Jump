using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnGroundState : BasicMovementState
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
        }
        else if (controller.ClimbCheck()) {
            controller.SetState(controller.stClimb);
            return false; ;
        }
        else if (!controller.OnGround) {
            controller.SetState(controller.stFall);
            return false; ;
        }
        return true;
    }
}
