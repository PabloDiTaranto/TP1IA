using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadStateEnemy<T> : State<T>
{
    private EnemyController _enemy;
    private EnemyView _enemyView;

    public DeadStateEnemy(EnemyController enemy, EnemyView enemyView)
    {
        _enemy = enemy;
        _enemyView = enemyView;
    }

    public override void Awake()
    {
        _enemyView.StepsStop();
        _enemyView.Death();
        _enemyView.DeathAnimation();
    }
    public override void Execute()
    {
        _enemy.isTimeToRespawn.Execute();
    }
}
