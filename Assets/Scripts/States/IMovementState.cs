using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementState
{
    void Enter();
    bool Update();
    void Exit();
}
