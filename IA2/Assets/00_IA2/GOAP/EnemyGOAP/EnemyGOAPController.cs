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

    private bool canBeDamage;

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
        canBeDamage = false;
        yield return new WaitForSeconds(3f);
        var watchDog = 5000;
        Debug.Log("RestartPlan");

        while (_currentEnemy == null && watchDog>0)
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
            Debug.LogError("Fail");

    }

    List<GOAPAction> GOAPActionList()
    {
        /*Action<GOAPAction, string, object> SetEffect = (action, key, value) => action.effects[key] = value;
        Action<GOAPAction, string, object> SetPrecondition = (action, key, value) => action.preconditions[key] = value;
        
        GOAPAction chaseEnemy = new GOAPAction("ChaseEnemy");
        SetEffect(chaseEnemy, "isEnemyNear", "true");

        GOAPAction meleeWeapon = new GOAPAction("MeleeWeapon");
        SetEffect(meleeWeapon, "hasMeleeWeapon", true);
        SetPrecondition(meleeWeapon, "isEnemyNear", "true");
        SetPrecondition(meleeWeapon, "wantMeleeWeapon", 1);

        GOAPAction distanceWeapon = new GOAPAction("DistanceWeapon");
        SetEffect(distanceWeapon, "hasDistanceWeapon", true);
        SetPrecondition(distanceWeapon, "isEnemyNear", "true");
        SetPrecondition(distanceWeapon, "wantDistanceWeapon", 1f);

        GOAPAction distanceAttack = new GOAPAction("DistanceAttack");
        SetEffect(distanceAttack, "isPlayerAlive", false);
        SetPrecondition(distanceAttack, "hasDistanceWeapon", true);
        
        GOAPAction meleeAttack = new GOAPAction("MeleeAttack");
        SetEffect(meleeAttack, "isPlayerAlive", false);
        SetPrecondition(meleeAttack, "hasMeleeWeapon", true);*/

        return new List<GOAPAction>{
                                                  new GOAPAction("ChaseEnemy")
                                                 .Effect("isEnemyNear", x => x.SetValue(true))
                                                 .LinkedState(chaseEnemy),


                                                  new GOAPAction("MeleeWeapon")
                                                 .Pre("isEnemyNear", x => x.GetValue<float>() < (2 * 2))
                                                 .Pre("wantMeleeWeapon", x => x.GetValue<EnemyGOAPController>().enemyType == EnemyType.MELEE&& x.GetValue<EnemyGOAPController>()!=null)
                                                 .Effect("hasMeleeWeapon", x => x.SetValue(true))
                                                 .LinkedState(meleeWeapon),

                                                  new GOAPAction("DistanceWeapon")
                                                 .Pre("isEnemyNear",   x => x.GetValue<float>() < (2 * 2))
                                                 .Pre("wantDistanceWeapon", x => (x.GetValue<EnemyGOAPController>().enemyType == EnemyType.RANGE&& x.GetValue<EnemyGOAPController>()!=null))
                                                 .Effect("hasDistanceWeapon",    x => x.SetValue(true))
                                                 .LinkedState(distanceWeapon),

                                                 new GOAPAction("DistanceAttack")
                                                 .Pre("hasDistanceWeapon",   x => x.GetValue<bool>() == true)
                                                 .Effect("isPlayerAlive",  x => x.SetValue(_character.GetLife()==0))
                                                 .LinkedState(distanceAttack),


                                                 new GOAPAction("MeleeAttack")
                                                 .Pre("hasMeleeWeapon",   x => x.GetValue<bool>() == true)
                                                 .Effect("isPlayerAlive", x => x.SetValue(_character.GetLife()==0))
                                                 .LinkedState(meleeAttack)
                                          };


    }
    GOAPState GetGOAPState()
    {
        var from = new GOAPState();
        from.values["isEnemyNear"] = new Element((transform.position - _currentEnemy.transform.position).sqrMagnitude/* < (2 * 2)*/);
        from.values["wantMeleeWeapon"] = new Element(_currentEnemy);
        from.values["wantDistanceWeapon"] = new Element(_currentEnemy);
        from.values["hasMeleeWeapon"] = new Element(_hasMeleeWeapon);
        from.values["hasDistanceWeapon"] = new Element(_hasDistanceWeapon);


        return from;
        

        /*Func<string> getEnemyNear = () => (transform.position - _currentEnemy.transform.position).sqrMagnitude < 2 * 2 ? "true" : "false";
        from.values["isEnemyNear"] = getEnemyNear();

        Func<int> getWantMeleeWeapon = () => _currentEnemy != null && _currentEnemy.enemyType == EnemyType.MELEE ? 1 : 0;
        from.values["wantMeleeWeapon"] = getWantMeleeWeapon();
        
        Func<float> getWantRangeWeapon = () => _currentEnemy != null && _currentEnemy.enemyType == EnemyType.RANGE ? 1.0f : 0.0f;
        from.values["wantDistanceWeapon"] = getWantRangeWeapon();
        
        Func<bool> getHasMeleeWeapon = () => _hasMeleeWeapon;
        from.values["hasMeleeWeapon"] = getHasMeleeWeapon();

        Func<bool> getHasDistanceWeapon = () => hasDistanceWeapon;
        from.values["hasDistanceWeapon"] = getHasDistanceWeapon();*/
    }

    public void PlanAndExecute()
    {
        var actions = GOAPActionList();

        var from = GetGOAPState();

        var to = new GOAPState();
        
        to.values["isPlayerAlive"] = new Element(_character.IsDead);

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
        to.values["isPlayerAlive"] = new Element(_character.IsDead);

        var planner = new GoapPlanner(this);

        planner.Run(from, to, actions, StartCoroutine);

        //ConfigureFsm(plan);
    }

    private void ConfigureFsm(IEnumerable<GOAPAction> plan)
    {
        Debug.LogWarning("Completed Plan");
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
        canBeDamage = true;
    }

    public override void Damage()
    {
        _currentLife--;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 14 && canBeDamage)
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


