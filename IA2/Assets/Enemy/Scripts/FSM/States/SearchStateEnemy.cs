using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchStateEnemy<T> : State<T>
{
    private EnemyController _enemy;
    private EnemyView _enemyView;
    private Transform _target;
    private Rigidbody _enemyRB;
    private LineOfSight _lOS;
    private AgentPathfinding _agentPF;
    private List<Vector3> waypoints = new List<Vector3>();
    int _nextPoint = 0;
    private float _speed, _speedRot, _timeToCheckPlayerPos, timer;
    private bool endList;
   

    private LayerMask _mask;

    public SearchStateEnemy(EnemyController enemy,Transform target, Rigidbody enemyRB, LineOfSight lOS, AgentPathfinding agentPF, float speed, float speedRot, float timeToCheckPlayerPos, EnemyView enemyView)
    {
        _enemy = enemy;
        _target = target;
        _enemyRB = enemyRB;
        _lOS = lOS;
        _agentPF = agentPF;
        _speed = speed;
        _speedRot = speedRot;
        _timeToCheckPlayerPos = timeToCheckPlayerPos;
        _enemyView = enemyView;
    }

    public override void Awake()
    {
        _nextPoint = 0;
        timer = _timeToCheckPlayerPos;
        _enemyView.StepsPlay();
        _enemyView.RunAnimation();
    }

    public override void Execute()
    {
        if (!_enemy.HasLife())
            _enemy.isTimeToRespawn.Execute();
        if (timer >= _timeToCheckPlayerPos || endList)
        {
            waypoints = _agentPF.PathFindingAstarVector(_enemy.transform);
            _nextPoint = 0;
            endList = false;
            timer = 0f;
        }
        else
            timer += Time.deltaTime;
        if(waypoints.Count > 0)
            Run();

        if (_lOS.IsInSight(_target))
        {
            _enemy.isOnDistanceToFire.Execute();
        }
    }

    public void Run()
    {
        if (waypoints.Count < 0) return;
        var point = waypoints[_nextPoint];
        Vector3 dir = point - _enemy.transform.position;
        if (dir.magnitude < 0.2f)
        {
            if (_nextPoint + 1 < waypoints.Count)
                _nextPoint++;
            else
            {
                endList = true;
                return;
            }
        }
        Move(dir.normalized);
    }

    public void Move(Vector3 dir)
    {
        dir.y = 0;
        _enemy.transform.position += Time.deltaTime * dir * _speed; ;
        _enemy.transform.forward = Vector3.Lerp(_enemy.transform.forward, dir, _speedRot * Time.deltaTime);
    }
}


