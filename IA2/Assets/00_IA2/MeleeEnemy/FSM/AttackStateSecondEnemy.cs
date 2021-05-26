using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateSecondEnemy<T> : State<T>
{
    private MeleeEnemyModel _secondEnemyModel;
    private MeleeEnemyView _secondEnemyView;
    private MeleeEnemyController _secondEnemyController;
    private Transform _target;
    private LineOfSight _lineOfSight;
    float _timer;

    public AttackStateSecondEnemy(MeleeEnemyModel secondEnemyModel, MeleeEnemyView secondEnemyView,MeleeEnemyController secondEnemyController, Transform target, LineOfSight lineOfSight)
    {
        _secondEnemyModel = secondEnemyModel;
        _secondEnemyView = secondEnemyView;
        _secondEnemyController = secondEnemyController;
        _target = target;
        _lineOfSight = lineOfSight;
    }
    public override void Awake()
    {
        _secondEnemyModel._attackTrigger.enabled = false;
    }

    public override void Execute()
    {
        if (!_secondEnemyController.HasLife())
            _secondEnemyController._isTimeToRespawn.Execute();
        if (!_lineOfSight.IsInSight(_target))
            _secondEnemyController._isPlayerOnSight.Execute();
        if (!_secondEnemyController.CheckDistanceToAttack())
            _secondEnemyController._isOnDistanceAttack.Execute();

        _secondEnemyController.transform.LookAt(_target);
        _timer += Time.deltaTime;
        _secondEnemyModel._attackTrigger.enabled = false;
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
