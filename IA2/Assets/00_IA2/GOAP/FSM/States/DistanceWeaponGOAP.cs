using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class DistanceWeaponGOAP : MonoBaseState
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
            _enemyGOAPController.DistanceWeapon.SetActive(true);
            _enemyGOAPController._hasMeleeWeapon = false;
            _enemyGOAPController._hasDistanceWeapon = true;

            _enemyGOAPController._enemyGOAPView.HealAnim(false);
            _enemyGOAPController._enemyGOAPView.MeleeHitAnim(false);
            _enemyGOAPController._enemyGOAPView.ShootAnim(false);
            _enemyGOAPController._enemyGOAPView.GrabWeaponAnim(true);
            //_enemyGOAPController._enemyGOAPView.OneShotSoundClip(0);
            executeOnce = true;
        }
    }

    public override IState ProcessInput()
    {
        executeOnce = false;
        if (_enemyGOAPController._hasDistanceWeapon && Transitions.ContainsKey("OnDistanceAttackGOAP"))
            return Transitions["OnDistanceAttackGOAP"];

        return this;
    }
}
