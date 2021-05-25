using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateSecondEnemy<T> : State<T>
{
    private MeleeEnemyModel _secondEnemyModel;
    private MeleeEnemyView _secondEnemyView;
    float _timer;

    public AttackStateSecondEnemy(MeleeEnemyModel enemy, MeleeEnemyView enemyView)
    {
        _secondEnemyModel = enemy;
        _secondEnemyView = enemyView;
    }
    public override void Awake()
    {
    }

    public override void Execute()
    {
        _timer += Time.deltaTime;
        if (_timer < _secondEnemyModel._attackRate) return;

        _secondEnemyView.OneShotSoundClip(1);
        _secondEnemyView.AttackAnim(true);
        _secondEnemyModel._attackTrigger.enabled = true;
        _timer = 0;
    }

    public override void Sleep()
    {
        _secondEnemyModel._attackTrigger.enabled = false;
    }
}
