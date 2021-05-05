using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T>
{
    IState<T> _current;
    public FSM(IState<T> init)
    {
        SetInit(init);
    }

    public void SetInit(IState<T> init)
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
        IState<T> newState = _current.GetState(input);
        if (newState == null) return;
        _current.Sleep();
        _current = newState;
        _current.Awake();
    }
}
