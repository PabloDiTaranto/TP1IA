using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMachine<T>
{
    IStateFSM<T> _current;
    public FSMachine(IStateFSM<T> init)
    {
        SetInit(init);
    }

    public void SetInit(IStateFSM<T> init)
    {
        _current = init;
        _current.Awake();
    }

    public void OnUpdate()
    {
        _current.Execute();
    }

    public void Transition(T input)
    {
        IStateFSM<T> newState = _current.GetState(input);
        if (newState == null) return;
        _current.Sleep();
        _current = newState;
        _current.Awake();
    }
}
