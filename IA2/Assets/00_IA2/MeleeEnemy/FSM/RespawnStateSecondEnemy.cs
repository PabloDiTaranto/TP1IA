using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnStateSecondEnemy<T> : State<T>
{
    private MeleeEnemyController _secondEnemyController;
    private MeleeEnemyView _secondEnemyView;
    public RespawnStateSecondEnemy(MeleeEnemyController enemy, MeleeEnemyView enemyView)
    {
        _secondEnemyController = enemy;
        _secondEnemyView = enemyView;
    }
    public override void Awake()
    {

    }

    public override void Execute()
    {
        EventManager.Trigger("OnEnemyDestroy");
        _secondEnemyController.DestroyObj();
    }
}
