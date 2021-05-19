using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour,IGridEntity
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private EnemyModel enemy;
    [SerializeField] private EnemyView enemyView;
    [SerializeField] private AgentPathfinding _agentPF;
    private float currentLife, timerRespawn;
    [SerializeField] private LineOfSight lineOfSight;
    private FSM<string> fsmEnemy;

    private ISteering pursuitObsAvoidance, seekObsAvoidance, seekBullet, pursuitBullet;
    
    public QuestionNode isTimeToRespawn, isSearching, isPlayerOnSight, isOnDistanceToFire;
    private ActionNode actionDead, actionSearch, actionChase, actionFire, actionRespawn;
    private INode init;
    private Roulette roulette;

    private bool isPlayerAlive = true;

    public event Action<IGridEntity> OnMove;


    public bool _isDead = false;

    private void Awake()
    {        
        currentLife = enemy.life;
        pursuitObsAvoidance = new PursuitObstacleAvoidance(enemy.rbPlayer, enemy.radius, enemy.avoidWeight, enemy.maskAvoidance, enemy.timePredictionChase);
        seekBullet = new BulletSeek(enemy.target);
        pursuitBullet = new BulletPursuit(enemy.rbPlayer, enemy.timePredictionBullet, enemy.playerDir);
        EventManager.Subscribe("OnPlayerDead", DestroyMe);

        OnMove += SpatialGrid._instance.UpdateEntity;
        SpatialGrid._instance.UpdateEntity(this);
    }

    private void Start()
    {
#region FSM
        var deadTransition = new DeadStateEnemy<string>(this, enemyView);
        var searchTransition = new SearchStateEnemy<string>(this, enemy.target, rb, lineOfSight, _agentPF, enemy.speed, enemy.speedRot, enemy.timeToCheckPlayerPos, enemyView);
        var chaseTransition = new ChaseStateEnemy<string>(this, enemy.target, rb, lineOfSight, pursuitObsAvoidance, enemy.speed, enemy.speedRot, enemyView);
        var fireTransition = new FiringStateEnemy<string>(this, enemy.target, rb, lineOfSight, enemy.fireRate, enemy.bulletPrefab, enemy.spawnBullets, seekBullet, pursuitBullet, enemyView);
        var respawnTransition = new RespawnStateEnemy<string>(this, enemyView);
        fsmEnemy = new FSM<string>(searchTransition);

        searchTransition.AddTransition("Dead", deadTransition);
        searchTransition.AddTransition("Chase", chaseTransition);
        searchTransition.AddTransition("Fire", fireTransition);

        chaseTransition.AddTransition("Dead", deadTransition);
        chaseTransition.AddTransition("Search", searchTransition);
        chaseTransition.AddTransition("Fire", fireTransition);

        fireTransition.AddTransition("Dead", deadTransition);
        fireTransition.AddTransition("Chase", chaseTransition);
        fireTransition.AddTransition("Search", searchTransition);

        deadTransition.AddTransition("Respawn", respawnTransition);

        respawnTransition.AddTransition("Chase", chaseTransition);
        respawnTransition.AddTransition("Search", searchTransition);
        respawnTransition.AddTransition("Fire", fireTransition);
        #endregion

#region Decision Tree
        actionDead = new ActionNode(ToDead);
        actionSearch = new ActionNode(ToSearch);
        actionChase = new ActionNode(ToChase);
        actionFire = new ActionNode(ToFire);
        actionRespawn = new ActionNode(ToRespawn);

        isPlayerOnSight = new QuestionNode(PlayerInSight, actionChase, actionSearch);
        isOnDistanceToFire = new QuestionNode(CheckDistanceToFire, actionFire, actionChase);
        isTimeToRespawn = new QuestionNode(RespawnTime, actionRespawn, actionDead);

        init = isPlayerOnSight;
        init.Execute();
#endregion
    }

    private void Update()
    {
        if (isPlayerAlive)
        {
            fsmEnemy.OnUpdate();
        }
        OnMove?.Invoke(this);
    }


    void ToDead()
    {
        fsmEnemy.Transition("Dead");
    }
    void ToRespawn()
    {
        fsmEnemy.Transition("Respawn");
    }
    void ToSearch()
    {
        fsmEnemy.Transition("Search");
    }
    void ToFire()
    {
        fsmEnemy.Transition("Fire");
    }
    void ToChase()
    {
        fsmEnemy.Transition("Chase");
    }

    private bool PlayerInSight()
    {
        return lineOfSight.IsInSight(enemy.target);
    }

    bool RespawnTime()
    {
        if(timerRespawn >= enemy.timeToRespawn)
        {
            timerRespawn = 0;
            return true;
        }
        else
        {
            timerRespawn += Time.deltaTime;
            return false;
        }
    }
    public bool HasLife()
    {
        return (currentLife > 0);
    }
    public bool CheckDistanceToFire()
    {
        Vector3 diff = (enemy.target.position - transform.position);
        float distance = diff.magnitude;
        if (distance < enemy.distanceToFire) return true;
        else return false;
    }

    public void DestroyMe(params object[] parameters)
    {
        isPlayerAlive = false;
        EventManager.Unsubscribe("OnPlayerDead", DestroyMe);
        Destroy(gameObject);
    }
    public void Damage()
    {
        _isDead = true;
        currentLife--;
         //OnMove -= SpatialGrid._instance.UpdateEntity;
    }
   

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 14)
        {
            Damage();
        }
    }

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
}
