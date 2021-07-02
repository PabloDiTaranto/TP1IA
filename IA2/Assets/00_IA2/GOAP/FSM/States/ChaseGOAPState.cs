using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class ChaseGOAPState : MonoBaseState
{
    private bool executeOnce;
    public float speed = 2f;
    public float distanceToStealWeapon = 1.5f;
    private EnemyGOAPController _enemyGOAPController;
    private void Awake()
    {
        _enemyGOAPController = FindObjectOfType<EnemyGOAPController>();
    }

    public override void UpdateLoop()
    {
        if (_enemyGOAPController.CurrentEnemy == null)
        {
            _enemyGOAPController.AbortPlan();
            _enemyGOAPController.ResetValues();
            _enemyGOAPController.RestartPlanCoroutine();
            return;
        }
        _enemyGOAPController.transform.LookAt(_enemyGOAPController.CurrentEnemy.transform);

        if (!executeOnce)
        {
            _enemyGOAPController._enemyGOAPView.HealAnim(false);
            _enemyGOAPController._enemyGOAPView.MeleeHitAnim(false);
            _enemyGOAPController._enemyGOAPView.ShootAnim(false);
            _enemyGOAPController._enemyGOAPView.GrabWeaponAnim(false);
            executeOnce = true;
        }

        var dir = (_enemyGOAPController.CurrentEnemy.transform.position - transform.position).normalized;

        transform.position += dir * (speed * Time.deltaTime);

    }

    public override IState ProcessInput()
    {
        if(_enemyGOAPController.CurrentEnemy == null) return this;
        
        var sqrDistance = (_enemyGOAPController.CurrentEnemy.transform.position - transform.position).sqrMagnitude;
        if(sqrDistance < distanceToStealWeapon)
        {
            if (_enemyGOAPController.CurrentEnemy.enemyType == EnemyType.MELEE && Transitions.ContainsKey("OnMeleeWeaponGOAP"))
                return Transitions["OnMeleeWeaponGOAP"];
            if (_enemyGOAPController.CurrentEnemy.enemyType == EnemyType.RANGE && Transitions.ContainsKey("OnDistanceWeaponGOAP"))
                return Transitions["OnDistanceWeaponGOAP"];
        }

        return this;
    }
}
