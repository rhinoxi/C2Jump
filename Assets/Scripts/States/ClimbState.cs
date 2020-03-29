using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbState : BasicMovementState
{
    public ClimbState(Controller c): base(c) { }

    private float gravityBefore;
    public override void Enter() {
        gravityBefore = controller.Gravity;
        controller.Gravity = 0;
    }

    public override bool Update() {
        if (!base.Update()) {
            return false;
        }

        if (!controller.Grab) {
            controller.SetState(controller.stFall);
            return false;
        }

        if (!controller.OnWall) {
            // TODO: Climb Over
            if (controller.Speed.y > 0) {
                controller.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
            }
            controller.SetState(controller.stFall);
            return false;
        }

        if (controller.Jump) {
            if (Mathf.Abs(controller.Movement.x) > 0.5) {
                if (controller.WallBoost.sqrMagnitude > Vector2.kEpsilon) {
                    controller.SetState(controller.stBoostWallJump);
                } else {
                    controller.SetState(controller.stWallJump);
                    return false;
                }
            } else {
                controller.SetState(controller.stClimbJump);
            }
            return false;
        }


        if (controller.Movement.y != 0) {
            if (controller.OnWallTimer <= 0) {
                controller.SetState(controller.stFall);
                return false;
            }
            controller.Speed = controller.WallVelocity + Vector2.up * Mathf.Sign(controller.Movement.y) * controller.ClimbSpeed;
            controller.OnWallTimer -= Time.fixedDeltaTime;
            controller.SetAnimSpeed(1);
            // if (controller.Movement.y > 0) {
            //     controller.SetAnimSpeed(1);
            // } else {
            //     controller.SetAnimSpeed(-1);
            // }
        } else {
            controller.Speed = controller.WallVelocity;
            controller.SetAnimSpeed(0f);
        }

        return true;
    }

    public override void Exit() {
        controller.Gravity = gravityBefore;
        controller.SetAnimSpeed(1f);
    }

    public override string ToString() {
        return "Climb";
    }

    public override int ID => 2;
}
