using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class MeleeWeaponGOAP : MonoBaseState
{
    private EnemyGOAPController _enemyGOAPController;
    private bool executeOnce;
    private void Awake()
    {
        _enemyGOAPController = FindObjectOfType<EnemyGOAPController>();
    }

    public override void UpdateLoop()
    {
        if (!executeOnce) 
        {
            _enemyGOAPController.MeleeWeapon.SetActive(true);
            _enemyGOAPController._hasDistanceWeapon = false;
            _enemyGOAPController._hasMeleeWeapon = true;
            executeOnce = true;
        }
    }

    public override IState ProcessInput()
    {
        executeOnce = false;
        if (_enemyGOAPController._hasMeleeWeapon&&Transitions.ContainsKey("OnMeleeAttackGOAP"))
            return Transitions["OnMeleeAttackGOAP"];

        return this;
    }
}
