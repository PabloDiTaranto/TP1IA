using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadStateSecondEnemy<T>:  State<T>
{
    private MeleeEnemyController _secondEnemyController;
    private MeleeEnemyView _secondEnemyView;
    public DeadStateSecondEnemy(MeleeEnemyController enemy, MeleeEnemyView enemyView)
    {
        _secondEnemyController = enemy;
        _secondEnemyView = enemyView;
    }

    public override void Awake()
    {
        _secondEnemyView.DeathAnim();
        _secondEnemyView.OneShotSoundClip(0);
    }

    public override void Execute()
    {
        _secondEnemyController._isTimeToRespawn.Execute();
    }
}
