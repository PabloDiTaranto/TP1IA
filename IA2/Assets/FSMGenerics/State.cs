using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State<T> : IState<T>
{
    public virtual void Awake() { }
    public virtual void Execute() { }
    public virtual void Sleep() { }
    Dictionary<T, IState<T>> _myStates = new Dictionary<T, IState<T>>();
    public void AddTransition(T input, IState<T> state)
    {
        if (!_myStates.ContainsKey(input))
            _myStates.Add(input, state);
    }
    public void RemoveTransition(T input)
    {
        if (_myStates.ContainsKey(input))
            _myStates.Remove(input);
    }
    public IState<T> GetState(T input)
    {
        if (_myStates.ContainsKey(input))
        {
            return _myStates[input];
        }
        return null;
    }
}
