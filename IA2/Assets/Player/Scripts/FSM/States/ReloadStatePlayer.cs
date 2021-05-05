using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadStatePlayer<T> : State<T>
{
    private PlayerModel _player;
    private PlayerView _playerView;
    private float _timeToReload, _reloadTimer;

    public ReloadStatePlayer(PlayerModel player, float timeToReload, PlayerView playerView)
    {
        _player = player;
        _playerView = playerView;
        _timeToReload = timeToReload;
    }

    public override void Awake()
    {
        _reloadTimer = 0;
        _playerView.ReloadPlay();
        _player.isReloading = true;
    }

    public override void Execute()
    {
        if (_timeToReload <= _reloadTimer)
        {
            _player.ammoCount = _player.originalAmmoCount;
            EventManager.Trigger("OnAmmoChange", _player.ammoCount);
            _player.isReloading = false;
        }
        else
        {
            _reloadTimer += Time.deltaTime;
        }
    }
}
