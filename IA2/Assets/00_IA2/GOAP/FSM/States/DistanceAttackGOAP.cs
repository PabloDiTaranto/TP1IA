﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class DistanceAttackGOAP : MonoBaseState
{
    private bool executeOnce;
    private float _timerRate;
    [SerializeField] private float _fireRate;
    private EnemyGOAPController _enemyGOAPController;
    private CharacterController _player;
    [SerializeField] private float distanceBetweenTarget;
    [SerializeField] private float _speed;
    private void Awake()
    {
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
            _enemyGOAPController._enemyGOAPView.GrabWeaponAnim(false);
            _enemyGOAPController._enemyGOAPView.MeleeHitAnim(false);
            _enemyGOAPController._enemyGOAPView.ShootAnim(false);
            executeOnce = true;
        }

        var sqrDistance = (_player.transform.position - transform.position).sqrMagnitude;
        if (sqrDistance > distanceBetweenTarget)
        {
            var dir = (_player.transform.position - transform.position).normalized;
            dir.y = 0;
            transform.position += dir * (_speed * Time.deltaTime);
            _enemyGOAPController._enemyGOAPView.SetSpeed(dir);
        }

        else
        {
            _enemyGOAPController._enemyGOAPView.SetSpeed(Vector3.zero);
            if (_timerRate >= _fireRate)
            {
                _enemyGOAPController._enemyGOAPView.ShootAnim(true);
                Shoot();
            }
            else
            {
                _enemyGOAPController._enemyGOAPView.ShootAnim(false);
                _timerRate += Time.deltaTime;
            }
        }
    }

    public override IState ProcessInput()
    {
        executeOnce = false;
        return this;
    }

    private void Shoot()
    {
       // _enemyGOAPController._enemyGOAPModel._bulletPrefab.transform.position = _enemyGOAPController._enemyGOAPModel._spawnPoint.position;
        var bullet = Object.Instantiate(_enemyGOAPController._enemyGOAPModel._bulletPrefab, _enemyGOAPController._enemyGOAPModel._spawnPoint.position, _enemyGOAPController._enemyGOAPModel._spawnPoint.rotation);
        var dir = (_player.transform.position - bullet.transform.position);
        dir.y = 0;
        bullet.transform.forward = dir;

        _enemyGOAPController._enemyGOAPView.OneShotSoundClip(0);
        _timerRate = 0f;
    }
}
