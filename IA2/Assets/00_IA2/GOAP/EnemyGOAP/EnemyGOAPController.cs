using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using System;

public class EnemyGOAPController : AbstractEnemy, IGridEntity, IGOAP
{
    public event Action<IGridEntity> OnMove;
    private float _lastReplanTime;
    private float _replanRate = .5f;
    private FiniteStateMachine _fsm;

    public ChaseGOAPState chaseEnemy;
    public MeleeWeaponGOAP meleeWeapon;
    public DistanceWeaponGOAP distanceWeapon;
    public DistanceAttackGOAP distanceAttack;
    public MeleeAttackGOAP meleeAttack;

    [SerializeField] LayerMask layerMask;

    private AbstractEnemy _currentEnemy;
    public AbstractEnemy CurrentEnemy { get { return _currentEnemy; } }

    public bool _hasMeleeWeapon;
    public bool _hasDistanceWeapon;

    private CharacterController _character;

    [SerializeField]private GameObject _meleeWeapon;
    [SerializeField]private GameObject _distanceWeapon;

    public GameObject MeleeWeapon { get { return _meleeWeapon; } }
    public GameObject DistanceWeapon { get { return _distanceWeapon; } }

    public EnemyGOAPView _enemyGOAPView;
    public EnemyGOAPModel _enemyGOAPModel;

    private HealState _healState;

    private void Awake()
    {
        _character = FindObjectOfType<CharacterController>();
        _meleeWeapon.SetActive(false);
        _distanceWeapon.SetActive(false);
        _enemyGOAPView = GetComponent<EnemyGOAPView>();
        _healState = FindObjectOfType<HealState>();
        _currentLife = 10;

    }
    void Start()
    {
        chaseEnemy.OnNeedsReplan += OnReplan;
        meleeWeapon.OnNeedsReplan += OnReplan;
        distanceWeapon.OnNeedsReplan += OnReplan;
        distanceAttack.OnNeedsReplan += OnReplan;
        meleeAttack.OnNeedsReplan += OnReplan;
        RestartPlanCoroutine();
    }

    void Update()
    {
        OnMove?.Invoke(this);
        if(_currentLife<=0)
            _healState.Healing();
    }

    public void RestartPlanCoroutine()
    {
        StartCoroutine("RestartPlan");
    }

    public void ResetValues()
    {
        _currentLife = 10;
        _hasDistanceWeapon = false;
        _hasMeleeWeapon = false;
        MeleeWeapon.SetActive(false);
        DistanceWeapon.SetActive(false);
    }

    private IEnumerator RestartPlan()
    {
        yield return new WaitForSeconds(3f);
        var watchDog = 5000;
        Debug.Log("RestartPlan");

        while (_currentEnemy==null&&watchDog>0)
        {
            watchDog -= 1;
            ObtainedEnemy();
        }

        if (_currentEnemy)
        {
            PlanAndExecute();
            Debug.Log("PlanAndExecute");
        }
            
        else
            Debug.Log("FALLO TODO");

    }

    List<GOAPAction> GOAPActionList()
    {
        return new List<GOAPAction>{
                                              new GOAPAction("ChaseEnemy")
                                                 .Effect("isEnemyNear", "true")
                                                 .LinkedState(chaseEnemy),


                                              new GOAPAction("MeleeWeapon")
                                                 .Pre("isEnemyNear", "true")
                                                 .Pre("wantMeleeWeapon", 1)
                                                 .Effect("hasMeleeWeapon", true)
                                                 .LinkedState(meleeWeapon),

                                              new GOAPAction("DistanceWeapon")
                                                 .Pre("isEnemyNear",   "true")
                                                 .Pre("wantDistanceWeapon", 1f)
                                                 .Effect("hasDistanceWeapon",    true)
                                                 .LinkedState(distanceWeapon),

                                                 new GOAPAction("DistanceAttack")
                                                 .Pre("hasDistanceWeapon",   true)
                                                 .Effect("isPlayerAlive", false)
                                                 .LinkedState(distanceAttack),

                                                 new GOAPAction("MeleeAttack")
                                                 .Pre("hasMeleeWeapon",   true)
                                                 .Effect("isPlayerAlive", false)
                                                 .LinkedState(meleeAttack)
                                          };

    }
    GOAPState GetGOAPState()
    {
        var from = new GOAPState();
        //Action<Vector3, Vector3> getEnemy = (Vector3 init, Vector3 finit) => { if ((finit - init).sqrMagnitude < 2 * 2) return "true"; return else "false"; }
        //from.values["isEnemyNear"] = getEnemy;


        from.values["isEnemyNear"] = UtilitiesGOAP.IsEnemyNear(transform.position, _currentEnemy.transform.position);//(Vector3 init, Vector3 finit) => { if ((finit - init).sqrMagnitude < 2 * 2) return "true"; else return "false"; }
        from.values["wantMeleeWeapon"] = UtilitiesGOAP.IsNeededWeaponMelee(EnemyType.MELEE, _currentEnemy); //(AbstractEnemy currentEnemy) => { if (currentEnemy != null && currentEnemy.enemyType == type) return 1; else return 0; }
        from.values["wantDistanceWeapon"] = UtilitiesGOAP.IsNeededWeaponRange(EnemyType.RANGE, _currentEnemy);//(AbstractEnemy currentEnemy) => { if(currentEnemy != null && currentEnemy.enemyType == type) return 1f;else return 0f;}
        from.values["hasMeleeWeapon"] = _hasMeleeWeapon;
        from.values["hasDistanceWeapon"] = _hasDistanceWeapon;
        return from;
    }

    public void PlanAndExecute()
    {
        var actions = GOAPActionList();

        var from = GetGOAPState();

        var to = new GOAPState();
        to.values["isPlayerAlive"] = _character.IsDead;

        var planner = new GoapPlanner(this);

        planner.Run(from, to, actions,StartCoroutine);

        //ConfigureFsm(plan);
    }
    public void AbortPlan()
    {
        _enemyGOAPView.SetSpeed(Vector3.zero);
        _fsm.Active = false;
    }
    public void SetPlan(IEnumerable<GOAPAction> plan)
    {
        ConfigureFsm(plan);
    }

    private bool ObtainedEnemy()
    {
        _currentEnemy = UtilitiesGOAP.GetNearestEnemy(transform.position, layerMask);

        return _currentEnemy == null;
    }

    private void OnReplan()
    {
        if (Time.time >= _lastReplanTime + _replanRate)
        {
            _lastReplanTime = Time.time;
        }
        else
        {
            return;
        }

        var actions = GOAPActionList();

        var from = GetGOAPState();

        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;

        var planner = new GoapPlanner(this);

        planner.Run(from, to, actions, StartCoroutine);

        //ConfigureFsm(plan);
    }

    private void ConfigureFsm(IEnumerable<GOAPAction> plan)
    {
        Debug.LogWarning("Completed Plan");
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }

    public override void Damage()
    {
        _currentLife--;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 14)
        {
            Damage();
        }
    }

    public void DestroyObj(params object[] parameters)
    {
        Destroy(gameObject);
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


