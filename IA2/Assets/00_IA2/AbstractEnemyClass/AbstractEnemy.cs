using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemy: MonoBehaviour
{
    protected float _currentLife;

    [HideInInspector]
    public bool _isDead;
    public abstract void Damage();
}
