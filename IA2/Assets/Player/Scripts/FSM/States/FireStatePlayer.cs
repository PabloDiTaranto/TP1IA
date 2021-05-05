using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStatePlayer<T> : State<T>
{
    PlayerModel _player;
    PlayerView _playerView;
    float _fireRate, _fireTimer;

    public FireStatePlayer(PlayerModel player, float fireRate, PlayerView playerView)
    {
        _player = player;
        _fireRate = fireRate;
        _playerView = playerView;
    }

    public override void Awake()
    {
        _fireTimer = _fireRate;
    }

    public override void Execute()
    {
        if(_fireTimer >= _fireRate)
        {
            var bullet = Object.Instantiate(_player.bulletPrefab, _player.bulletOrigin.transform.position, _player.bulletOrigin.transform.rotation);
            _player.ammoCount--;
            _playerView.ShootPlay();
            EventManager.Trigger("OnAmmoChange", _player.ammoCount);
            _fireTimer = 0f;
        }
        else
        {
            _fireTimer += Time.deltaTime;
        }
    }
}
