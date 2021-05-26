using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseStateSecondEnemy<T> : State<T>
{
    private MeleeEnemyController _secondEnemyController;
    private MeleeEnemyView _secondEnemyView;
    private Transform _target;
    private Rigidbody _rb;
    private LineOfSight _lineOfSight;
    private ISteering _pursuit;
    private float _speed, _speedRot;
    public ChaseStateSecondEnemy(MeleeEnemyController secondEnemyController, Transform target, Rigidbody rb, LineOfSight lineOfSight, ISteering pursuit, float speed, float speedRot, MeleeEnemyView secondEnemyView)
    {
        _pursuit = pursuit;
        _secondEnemyController = secondEnemyController;
        _target = target;
        _rb = rb;
        _lineOfSight = lineOfSight;
        _speed = speed;
        _speedRot = speedRot;
        _secondEnemyView = secondEnemyView;
    }
    public override void Awake()
    {
        _secondEnemyView.AttackAnim(false);
    }

    public override void Execute()
    {
        Debug.Log("ChaseExecute");
        if (!_secondEnemyController.HasLife())
            _secondEnemyController._isTimeToRespawn.Execute();
        if (!_lineOfSight.IsInSight(_target))
            _secondEnemyController._isPlayerOnSight.Execute();
        if (_secondEnemyController.CheckDistanceToAttack())
            _secondEnemyController._isOnDistanceAttack.Execute();

        Move(_pursuit.GetDir(_secondEnemyController.transform.position));
    }

    public void Move(Vector3 dir)
    {
        Debug.Log("MoveChase");
        dir.y = 0;
        _secondEnemyController.transform.position += Time.deltaTime * dir * _speed; ;
        _secondEnemyController.transform.forward = Vector3.Lerp(_secondEnemyController.transform.forward, dir, _speedRot * Time.deltaTime);
    }
}
