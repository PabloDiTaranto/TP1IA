using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class EnemyGOAPController : MonoBehaviour
{
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

    private void Awake()
    {
        _character = FindObjectOfType<CharacterController>();
        _meleeWeapon.SetActive(false);
        _distanceWeapon.SetActive(false);
        _enemyGOAPView = GetComponent<EnemyGOAPView>();

    }
    void Start()
    {
        chaseEnemy.OnNeedsReplan += OnReplan;
        meleeWeapon.OnNeedsReplan += OnReplan;
        distanceWeapon.OnNeedsReplan += OnReplan;
        distanceAttack.OnNeedsReplan += OnReplan;
        meleeAttack.OnNeedsReplan += OnReplan;
        StartCoroutine("Test");
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(3f);
        var watchDog = 5000;
        while (_currentEnemy==null&&watchDog>0)
        {
            watchDog -= 1;
            ObtainedEnemy();
        }
        if (_currentEnemy)
            PlanAndExecute();
        else
            Debug.Log(_currentEnemy);

    }

    List<GOAPAction> GOAPActionList()
    {
        return new List<GOAPAction>{
                                              new GOAPAction("ChaseEnemy")
                                                 .Effect("isEnemyNear", true)
                                                 .LinkedState(chaseEnemy),


                                              new GOAPAction("MeleeWeapon")
                                                 .Pre("isEnemyNear", true)
                                                 .Pre("wantMeleeWeapon", true)
                                                 .Effect("hasMeleeWeapon",    true)
                                                 .LinkedState(meleeWeapon),

                                              new GOAPAction("DistanceWeapon")
                                                 .Pre("isEnemyNear",   true)
                                                 .Pre("wantDistanceWeapon", true)
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
        from.values["isEnemyNear"] = UtilitiesGOAP.IsEnemyNear(transform.position, _currentEnemy.transform.position);
        from.values["wantMeleeWeapon"] = UtilitiesGOAP.IsNeededWeapon(EnemyType.MELEE, _currentEnemy);
        from.values["wantDistanceWeapon"] = UtilitiesGOAP.IsNeededWeapon(EnemyType.RANGE, _currentEnemy);
        from.values["hasMeleeWeapon"] = _hasMeleeWeapon;
        from.values["hasDistanceWeapon"] = _hasDistanceWeapon;
        return from;
    }

    private void PlanAndExecute()
    {
        var actions = GOAPActionList();

        var from = GetGOAPState();

        var to = new GOAPState();
        to.values["isPlayerAlive"] = _character.IsDead;

        var planner = new GoapPlanner(this);

        planner.Run(from, to, actions,StartCoroutine);

        //ConfigureFsm(plan);
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
        Debug.Log("Completed Plan");
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }
}


