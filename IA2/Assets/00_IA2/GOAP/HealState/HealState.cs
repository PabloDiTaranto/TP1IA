using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealState : MonoBehaviour
{
    private EnemyGOAPView _enemyGOAPView;
    private EnemyGOAPController _enemyGOAPController;
    private bool _doOnce;
    void Awake()
    {
        _enemyGOAPView = FindObjectOfType<EnemyGOAPView>();
        _enemyGOAPController = FindObjectOfType<EnemyGOAPController>();
    }

    public void Healing()
    {
        if (!_doOnce)
        {
            _enemyGOAPController.AbortPlan();
            _enemyGOAPView.MeleeHitAnim(false);
            _enemyGOAPView.ShootAnim(false);
            _enemyGOAPView.GrabWeaponAnim(false);
            _enemyGOAPView.HealAnim(false);
            _enemyGOAPView.DeathAnim(true);
            StartCoroutine("Timer");
            _doOnce = true;
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(6f);
        _enemyGOAPView.DeathAnim(false);
        _enemyGOAPView.HealAnim(true);
        yield return new WaitForSeconds(0.1f);
        _enemyGOAPView.HealAnim(false);
        Debug.Log("ONHEALSTATE");
        _enemyGOAPController.RestartPlanCoroutine();
        _enemyGOAPController.ResetValues();
        _doOnce = false;
    }


}
