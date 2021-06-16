using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class DistanceAttackGOAP : MonoBaseState
{
    private void Awake()
    {
    }

    public override void UpdateLoop()
    {
    }

    public override IState ProcessInput()
    {
        return this;
    }
}
