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

    private bool _hasMeleeWeapon;
    private bool _hasDistanceWeapon;

    private CharacterController _character;

    private void Awake()
    {
        _character = FindObjectOfType<CharacterController>();
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
                                                 .Pre("needMeleeWeapon", true)
                                                 .Effect("hasMeleeWeapon",    true)
                                                 .LinkedState(meleeWeapon),

                                              new GOAPAction("DistanceWeapon")
                                                 .Pre("isEnemyNear",   true)
                                                 .Pre("needDistanceWeapon", true)
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
        from.values["isEnemyNear"] = _currentEnemy!=null;
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

        var planner = new GoapPlanner();

        var plan = planner.Run(from, to, actions);

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

        var planner = new GoapPlanner();

        var plan = planner.Run(from, to, actions);

        ConfigureFsm(plan);
    }

    private void ConfigureFsm(IEnumerable<GOAPAction> plan)
    {
        Debug.Log("Completed Plan");
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }
}


