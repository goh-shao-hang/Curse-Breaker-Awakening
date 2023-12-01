using GameCells.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_Grabbed : Condition
{
    private GrabbableObject _grabbableObject;

    private bool _grabbed;

    public Condition_Grabbed(GrabbableObject grabbableObject)
    {
        this._grabbableObject = grabbableObject;
    }

    public override void Enter()
    {
        base.Enter();

        _grabbableObject.OnGrabbed.AddListener(OnGrabbed);

        _grabbed = false;
    }

    public override void Exit()
    {
        base.Exit();

        _grabbableObject.OnGrabbed.RemoveListener(OnGrabbed);

        _grabbed = false;
    }

    private void OnGrabbed()
    {
        _grabbed = true;
    }

    public override bool Evaluate()
    {
        return _grabbed;
    }


}
