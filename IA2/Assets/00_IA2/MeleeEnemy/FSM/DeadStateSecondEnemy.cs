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
    }

    public override void Execute()
    {
        _secondEnemyView.AttackAnim(false);
        _secondEnemyView.DeathAnim(true);
        _secondEnemyView.OneShotSoundClip(0);
        _secondEnemyController._isTimeToRespawn.Execute();
    }
}
