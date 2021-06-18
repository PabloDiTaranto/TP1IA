using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class ChaseGOAPState : MonoBaseState
{
    public float speed = 2f;
    public float distanceToStealWeapon = 1.5f;
    private EnemyGOAPController enemyGOAPController;
    private void Awake()
    {
        enemyGOAPController = FindObjectOfType<EnemyGOAPController>();
    }

    public override void UpdateLoop()
    {
        var dir = (enemyGOAPController.CurrentEnemy.transform.position - transform.position).normalized;

        transform.position += dir * (speed * Time.deltaTime);
    }

    public override IState ProcessInput()
    {
        var sqrDistance = (enemyGOAPController.CurrentEnemy.transform.position - transform.position).sqrMagnitude;
        if(sqrDistance < distanceToStealWeapon)
        {
            if (enemyGOAPController.CurrentEnemy.enemyType == EnemyType.MELEE && Transitions.ContainsKey("OnMeleeWeaponGOAP"))
                return Transitions["OnMeleeWeaponGOAP"];
            if (enemyGOAPController.CurrentEnemy.enemyType == EnemyType.RANGE && Transitions.ContainsKey("OnDistanceWeaponGOAP"))
                return Transitions["OnDistanceWeaponGOAP"];
        }

        return this;
    }
}
