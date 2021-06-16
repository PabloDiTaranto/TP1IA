//IA2-P1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeleeEnemyController : AbstractEnemy, IGridEntity
{
    public event Action<IGridEntity> OnMove;
    [SerializeField] private LineOfSight _lineOfSight;
    [SerializeField] private MeleeEnemyModel _secondEnemyModel;
    [SerializeField] private MeleeEnemyView _secondEnemyView;
    [SerializeField] private AgentPathfinding _agentPathfinding;
    private ActionNode _actionDead, _actionSearch, _actionChase, _actionAttack, _actionRespawn;
    public QuestionNode _isTimeToRespawn, _isSearching, _isPlayerOnSight, _isOnDistanceAttack;
    private ISteering _pursuitObsAvoidance, _seekObsAvoidance, _seekBullet, _pursuitBullet;
    private FSMachine<string> _fsmEnemy;
    private float _timerRespawn;
    private INode _init;

    private void Awake()
    {
        _secondEnemyModel = GetComponent<MeleeEnemyModel>();
        _secondEnemyView = GetComponent<MeleeEnemyView>();

        EventManager.Subscribe("OnPlayerDead", DestroyObj);

        for (int i = 0; i < _items.Length; i++)
        {
            float randomValue = UnityEngine.Random.Range(0f, 1f);
            if (randomValue <= 0.15f)
                _items[i] = "Energy";
            else
                _items[i] = "Empty";
        }
    }
    private void Start()
    {
        _currentLife = 2;
        _pursuitObsAvoidance = new PursuitObstacleAvoidance(_secondEnemyModel._rbTarget, _secondEnemyModel._radius,
            _secondEnemyModel._avoidWeight, _secondEnemyModel._maskAvoidance, _secondEnemyModel._timePredictionChase);

        var deadTransition = new DeadStateSecondEnemy<string>(this, _secondEnemyView);
        var searchTransition = new SearchStateSecondEnemy<string>(this, _secondEnemyModel._target, _secondEnemyModel._rbTarget, _lineOfSight, _agentPathfinding, 
            _secondEnemyModel._speed, _secondEnemyModel._speedRot, _secondEnemyModel._timeToCheckPlayerPos, _secondEnemyView);
        var chaseTransition = new ChaseStateSecondEnemy<string>(this, _secondEnemyModel._target, _secondEnemyModel._rbTarget, _lineOfSight, _pursuitObsAvoidance, _secondEnemyModel._speed, _secondEnemyModel._speedRot, _secondEnemyView);
        var respawnTransition = new RespawnStateSecondEnemy<string>(this,_secondEnemyView);
        var attackTransition = new AttackStateSecondEnemy<string>(_secondEnemyModel, _secondEnemyView,this,_secondEnemyModel._target,_lineOfSight);
        _fsmEnemy = new FSMachine<string>(searchTransition);

        searchTransition.AddTransition("Dead", deadTransition);
        searchTransition.AddTransition("Chase", chaseTransition);
        searchTransition.AddTransition("Attack", attackTransition);

        chaseTransition.AddTransition("Dead", deadTransition);
        chaseTransition.AddTransition("Search", searchTransition);
        chaseTransition.AddTransition("Attack", attackTransition);

        attackTransition.AddTransition("Dead", deadTransition);
        attackTransition.AddTransition("Chase", chaseTransition);
        attackTransition.AddTransition("Search", searchTransition);

        deadTransition.AddTransition("Respawn", respawnTransition);

        respawnTransition.AddTransition("Chase", chaseTransition);
        respawnTransition.AddTransition("Search", searchTransition);
        respawnTransition.AddTransition("Attack", attackTransition);

        _actionDead = new ActionNode(ToDead);
        _actionSearch = new ActionNode(ToSearch);
        _actionChase = new ActionNode(ToChase);
        _actionAttack = new ActionNode(ToAttack);
        _actionRespawn = new ActionNode(ToRespawn);

        _isPlayerOnSight = new QuestionNode(PlayerInSight, _actionChase, _actionSearch);
        _isOnDistanceAttack = new QuestionNode(CheckDistanceToAttack, _actionAttack, _actionChase);
        _isTimeToRespawn = new QuestionNode(RespawnTime, _actionRespawn, _actionDead);

        _init = _isPlayerOnSight;
        _init.Execute();

        OnMove += SpatialGrid._instance.UpdateEntity;
        SpatialGrid._instance.UpdateEntity(this);
    }
    void Update()
    {
        OnMove?.Invoke(this);
        _fsmEnemy.OnUpdate();
    }

    void ToDead()
    {
        _fsmEnemy.Transition("Dead");
    }
    void ToRespawn()
    {
        _fsmEnemy.Transition("Respawn");
    }
    void ToSearch()
    {
        _fsmEnemy.Transition("Search");
    }
    void ToAttack()
    {
        _fsmEnemy.Transition("Attack");
    }
    void ToChase()
    {
        _fsmEnemy.Transition("Chase");
    }
    private bool PlayerInSight()
    {
        return _lineOfSight.IsInSight(_secondEnemyModel._target);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            Damage();
        }
    }
    private bool RespawnTime()
    {
        if (_timerRespawn >= _secondEnemyModel._timeToRespawn)
        {
            _timerRespawn = 0;
            return true;
        }
        else
        {
            _timerRespawn += Time.deltaTime;
            return false;
        }
    }
    public bool HasLife()
    {
        return (_currentLife > 0);
    }

    public bool CheckDistanceToAttack()
    {
        Vector3 diff = (_secondEnemyModel._target.position - transform.position);
        float distance = diff.magnitude;
        if (distance < _secondEnemyModel._distanceToAttack) return true;
        else return false;
    }

    public void DestroyObj(params object[] parameters)
    {
        Destroy(gameObject);
    }

    public override void Damage()
    {
        _currentLife--;
    }

    private void OnDestroy()
    {
        SpatialGrid._instance.Remove(this);
        EventManager.Unsubscribe("OnPlayerDead", DestroyObj);
    }

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
}
