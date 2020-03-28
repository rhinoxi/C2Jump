using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : OnGroundState
{
    public IdleState(Controller c): base(c) { }

    public override bool Update() {
        if (!base.Update()) {
            return false;
        }

        if (controller.Movement.x != 0) {
            controller.SetState(controller.stRun);
            return false;
        }

        controller.Speed = controller.LiftVelocity;

        return true;
    }

    public override string ToString() {
        return "Idle";
    }

    public override int ID => 0;
}
