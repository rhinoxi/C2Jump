using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicMovementState: IMovementState
{
    protected readonly Controller controller;
    public BasicMovementState(Controller c) {
        controller = c;
    }

    public virtual void Enter() {
    }

    public virtual bool Update() {
        if (controller.Dash) {
            if (controller.Dashable) {
                controller.SetState(controller.stDash);
                return false;
            }
            controller.Dash = false;
        }
        return true;
    }

    public virtual void Exit() {
    }
}
