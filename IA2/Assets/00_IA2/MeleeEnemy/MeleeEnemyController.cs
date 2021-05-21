using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeleeEnemyController : AbstractEnemy, IGridEntity
{
    public event Action<IGridEntity> OnMove;

    void Update()
    {
        OnMove?.Invoke(this);
    }

    public override void Damage()
    {

    }

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
}
