using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnStatePlayer<T> : State<T>
{
    PlayerModel _playerModel;
    PlayerController _playerController;

    public RespawnStatePlayer(PlayerModel playerModel, PlayerController playerController)
    {
        _playerModel = playerModel;
        _playerController = playerController;
    }

    public override void Awake()
    {
        _playerModel.transform.rotation = Quaternion.identity;
        _playerModel.transform.position = _playerModel.spawnPosition.position;
        _playerModel.RestartLife();
        _playerModel.isDead = false;
        EventManager.Trigger("OnPlayerRespawn");
        _playerController.SwapFsm("Idle");
    }

    public override void Execute()
    {
        Debug.Log("Si estas leyendo esto, algo salio muy mal");
    }
}
