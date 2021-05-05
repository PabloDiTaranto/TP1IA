using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseStateEnemy<T> : State<T>
{
    private EnemyController _enemy;
    private EnemyView _enemyView;
    private Transform _target;
    private Rigidbody _enemyRB;
    private LineOfSight _lOS;
    private ISteering _pursuit;
    private float _speed, _speedRot;

    public ChaseStateEnemy(EnemyController enemy, Transform target, Rigidbody enemyRB, LineOfSight lOS, ISteering pursuit, float speed, float speedRot, EnemyView enemyView)
    {
        _pursuit = pursuit;
        _enemy = enemy;
        _target = target;
        _enemyRB = enemyRB;
        _lOS = lOS;
        _speed = speed;
        _speedRot = speedRot;
        _enemyView = enemyView;
    }

    public override void Awake()
    {
        _enemyView.StepsPlay();
        _enemyView.RunAnimation();
    }

    public override void Execute()
    {
        if (!_enemy.HasLife())
            _enemy.isTimeToRespawn.Execute();
        if (!_lOS.IsInSight(_target))
            _enemy.isPlayerOnSight.Execute();
        if (_enemy.CheckDistanceToFire())
            _enemy.isOnDistanceToFire.Execute();

        Move(_pursuit.GetDir(_enemy.transform.position));
    }

    public void Move(Vector3 dir)
    {
        dir.y = 0;
        _enemy.transform.position += Time.deltaTime * dir * _speed; ;
        _enemy.transform.forward = Vector3.Lerp(_enemy.transform.forward, dir, _speedRot * Time.deltaTime);
    }
}
