using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnStateEnemy<T> : State<T>
{
    private EnemyController _enemy;
    private EnemyView _enemyView;

    public RespawnStateEnemy(EnemyController enemy, EnemyView enemyView)
    {
        _enemy = enemy;
        _enemyView = enemyView;
    }

    public override void Awake()
    {
        _enemyView.StepsStop();
    }

    public override void Execute()
    {
        EventManager.Trigger("OnEnemyDestroy");
        _enemy.DestroyMe();
    }
}
