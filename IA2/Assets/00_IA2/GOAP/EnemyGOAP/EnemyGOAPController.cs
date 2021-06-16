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

    void Start()
    {
        chaseEnemy.OnNeedsReplan += OnReplan;
        meleeWeapon.OnNeedsReplan += OnReplan;
        distanceWeapon.OnNeedsReplan += OnReplan;
        distanceAttack.OnNeedsReplan += OnReplan;
        meleeAttack.OnNeedsReplan += OnReplan;

        PlanAndExecute();
    }

    private void PlanAndExecute()
    {
        var actions = new List<GOAPAction>{
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

        var from = new GOAPState();
        from.values["isEnemyNear"] = false;//
        from.values["needMeleeWeapon"] = false;//
        from.values["hasMeleeWeapon"] = true;//
        from.values["needDistanceWeapon"] = true;//
        from.values["hasDistanceWeapon"] = true;//

        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;

        var planner = new GoapPlanner();

        var plan = planner.Run(from, to, actions);

        ConfigureFsm(plan);
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

        var actions = new List<GOAPAction>{
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

        var from = new GOAPState();
        from.values["isEnemyNear"] = false;//
        from.values["needMeleeWeapon"] = false;//
        from.values["hasMeleeWeapon"] = true;//
        from.values["needDistanceWeapon"] = true;//
        from.values["hasDistanceWeapon"] = true;//


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


