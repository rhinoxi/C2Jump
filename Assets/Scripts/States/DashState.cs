using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : BasicMovementState
{
    public DashState(Controller c): base(c) { }
    private float gravityBefore;
    private float dashTimer;
    private float dashLockTimer;
    private Vector2 dashDir;
    private float currentDashSpeed;

    public override void Enter() {
        controller.Dash = false;
        controller.Dashable = false;
        controller.IsDashing = true;
        gravityBefore = controller.Gravity;
        controller.Gravity = 0;
        dashTimer = controller.DashTime;
        dashLockTimer = controller.DashLockTime;
        currentDashSpeed = controller.DashStartSpeed;

        controller.StartCoroutine(Dashing());

        if (controller.Movement.y == 0 && controller.Movement.x == 0) {
            dashDir = Vector2.right * controller.Facing;
        } else {
            dashDir = controller.OctoDir(Mathf.Atan2(controller.Movement.y, controller.Movement.x));
        }
        
        if (dashDir.x * controller.Facing < 0) {
            controller.Flip();
        }
    }

    public override bool Update() {
        if (controller.OnGround) {
            controller.Dashable = true;
        } else {
            controller.Dashable = false;
        }

        if (!controller.IsDashing) {
            if (controller.OnGround) {
                controller.SetState(controller.stIdle);
            } else {
                controller.SetState(controller.stFall);
            }
            return false;
        }

        if (dashLockTimer <= 0) {
            if (controller.Jump) {
                // TODO: Maybe DashJump?
                controller.SetState(controller.stJump);
                return false;
            }
        }

        currentDashSpeed = Mathf.Lerp(controller.DashEndSpeed, currentDashSpeed, dashTimer / controller.DashTime);
        controller.Speed = dashDir * currentDashSpeed;
        return true;
    }

    public IEnumerator Dashing() {
        while (dashTimer > 0) {
            yield return new WaitForFixedUpdate();
            dashTimer -= Time.fixedDeltaTime;
            dashLockTimer -= Time.fixedDeltaTime;
        }
        controller.IsDashing = false;
    }

    public override void Exit() {
        controller.Dash = false;
        controller.Gravity = gravityBefore;
    }

    public override string ToString() {
        return "Dash";
    }

    public override int ID => 8;
}
