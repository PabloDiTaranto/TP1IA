using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateFSM<T>
{
    void Awake();
    void Execute();
    void Sleep();
    void AddTransition(T input, IStateFSM<T> state);
    void RemoveTransition(T input);
    IStateFSM<T> GetState(T input);
}
