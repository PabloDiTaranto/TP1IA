using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchStateSecondEnemy<T> : State<T>
{
    private MeleeEnemyController _secondEnemyController;
    private MeleeEnemyView _secondEnemyView;
    private Transform _target;
    private Rigidbody _rb;
    private LineOfSight _lineOfSight;
    private AgentPathfinding _agentPathfinding;
    private List<Vector3> _waypoints = new List<Vector3>();
    int _nextPoint = 0;
    private float _speed, _speedRot, _timeToCheckPlayerPos, _timer;
    private bool _endList;
    private LayerMask _mask;

    public SearchStateSecondEnemy(MeleeEnemyController secondEnemyController, Transform target, Rigidbody rb, LineOfSight lineOfSight,
        AgentPathfinding agentPathfinding, float speed, float speedRot, float timeToCheckPlayerPos, MeleeEnemyView secondEnemyView)
    {
        _secondEnemyController = secondEnemyController;
        _target = target;
        _rb = rb;
        _lineOfSight = lineOfSight;
        _agentPathfinding = agentPathfinding;
        _speed = speed;
        _speedRot = speedRot;
        _timeToCheckPlayerPos = timeToCheckPlayerPos;
        _secondEnemyView = secondEnemyView;
    }
    public override void Awake()
    {
        _nextPoint = 0;
        _timer = _timeToCheckPlayerPos;
        _secondEnemyView.AttackAnim(false);
    }

    public override void Execute()
    {
        if (!_secondEnemyController.HasLife())
            _secondEnemyController._isTimeToRespawn.Execute();
        if (_timer >= _timeToCheckPlayerPos || _endList)
        {
            _waypoints = _agentPathfinding.PathFindingAstarVector(_secondEnemyController.transform);
            _nextPoint = 0;
            _endList = false;
            _timer = 0f;
        }
        else
            _timer += Time.deltaTime;
        if (_waypoints.Count > 0)
            Run();

        if (_lineOfSight.IsInSight(_target))
        {
            _secondEnemyController._isOnDistanceAttack.Execute();
        }
    }

    public void Run()
    {
        if (_waypoints.Count < 0) return;
        var point = _waypoints[_nextPoint];
        Vector3 dir = point - _secondEnemyView.transform.position;
        if (dir.magnitude < 0.2f)
        {
            if (_nextPoint + 1 < _waypoints.Count)
                _nextPoint++;
            else
            {
                _endList = true;
                return;
            }
        }
        Move(dir.normalized);
    }

    public void Move(Vector3 dir)
    {
        dir.y = 0;
        _secondEnemyView.transform.position += Time.deltaTime * dir * _speed; ;
        _secondEnemyView.transform.forward = Vector3.Lerp(_secondEnemyView.transform.forward, dir, _speedRot * Time.deltaTime);
    }
}
