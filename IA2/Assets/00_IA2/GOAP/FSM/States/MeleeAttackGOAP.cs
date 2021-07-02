using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class MeleeAttackGOAP : MonoBaseState
{
    private bool executeOnce;
    [SerializeField] private Collider _attackTrigger;
    [SerializeField] private float distanceBetweenTarget = 1.5f;
    [SerializeField] private float _timer;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float _attackRate;
    private EnemyGOAPController _enemyGOAPController;
    private CharacterController _player;
    private void Awake()
    {
        _attackTrigger.enabled = false;
        _enemyGOAPController = FindObjectOfType<EnemyGOAPController>();
        _player = FindObjectOfType<CharacterController>();
    }

    public override void UpdateLoop()
    {
        if (_player == null) return;
        _enemyGOAPController.transform.LookAt(_player.transform);
        if (!executeOnce)
        {
            _enemyGOAPController._enemyGOAPView.HealAnim(false);
            _enemyGOAPController._enemyGOAPView.ShootAnim(false);
            _enemyGOAPController._enemyGOAPView.GrabWeaponAnim(false);
            _enemyGOAPController._enemyGOAPView.MeleeHitAnim(false);
            executeOnce = true;
        }

        var sqrDistance = (_player.transform.position - transform.position).sqrMagnitude;
        if (sqrDistance > distanceBetweenTarget)
        {
            var dir = (_player.transform.position - transform.position).normalized;
            dir.y = 0;
            transform.position += dir * (speed * Time.deltaTime);
            _enemyGOAPController._enemyGOAPView.SetSpeed(dir);
        }

        else
        {
            _enemyGOAPController._enemyGOAPView.SetSpeed(Vector3.zero);
            _timer += Time.deltaTime;
            if (_timer < _attackRate)
            {
                _enemyGOAPController._enemyGOAPView.MeleeHitAnim(false);
                _attackTrigger.enabled = false;
                return;
            }
            _attackTrigger.enabled = true;
            _enemyGOAPController._enemyGOAPView.MeleeHitAnim(true);
            _timer = 0;
        }


    }

    public override IState ProcessInput()
    {
        executeOnce = false;
        return this;
    }
}
