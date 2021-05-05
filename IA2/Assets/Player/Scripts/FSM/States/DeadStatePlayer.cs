using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadStatePlayer<T> : State<T>
{
    PlayerModel _playerModel;
    PlayerView _playerView;
    PlayerController _playerController;
    private float lerpTimer;

    public DeadStatePlayer(PlayerModel playerModel, PlayerController playerController, PlayerView playerView)
    {
        _playerModel = playerModel;
        _playerController = playerController;
        _playerView = playerView;
    }

    public override void Awake()
    {
        _playerView.Death();
        EventManager.Trigger("OnPlayerDead");
    }
    public override void Execute()
    {
        lerpTimer += Time.deltaTime;
        _playerModel.isDead = true;
        _playerModel.transform.rotation = Quaternion.Euler(_playerModel.transform.rotation.x, _playerModel.transform.rotation.y, Mathf.Lerp(Quaternion.identity.z , 90, lerpTimer * 2));
        if(lerpTimer > 2)
        {
            lerpTimer = 0;
            _playerController.SwapFsm("Respawn");
        }
    }
}
