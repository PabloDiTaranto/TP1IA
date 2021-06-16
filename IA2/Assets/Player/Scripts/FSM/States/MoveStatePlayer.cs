using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStatePlayer<T> : State<T>
{
    PlayerModel _playerModel;
    PlayerView _playerView;
    PlayerController _playerController;
    FSMachine<string> _weaponFSM;

    public MoveStatePlayer(PlayerModel player, FSMachine<string> weaponFSM, PlayerController playerController, PlayerView playerView)
    {
        _playerModel = player;
        _playerController = playerController;
        _weaponFSM = weaponFSM;
        _playerView = playerView;
    }

    public override void Awake()
    {
        _playerView.StepsPlay();
    }

    public override void Execute()
    {
        if (_playerModel.GetCurrentLife() <= 0)
        {
            _playerController.SwapFsm("Dead");
        }
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        _playerModel.Move(new Vector3(h, 0, v));
        if (Input.GetKey(KeyCode.Mouse0) && _playerModel.ammoCount > 0 && !_playerModel.isReloading && !PauseManager.isPause && !GameTimeManager.isGameOver)
        {
            _weaponFSM.Transition("Fire");
            _weaponFSM.OnUpdate();
        }

        if ((((Input.GetKeyDown(KeyCode.R) || _playerModel.ammoCount <= 0) && _playerModel.ammoCount < _playerModel.originalAmmoCount) || _playerModel.isReloading) && !PauseManager.isPause)
        {
            _weaponFSM.Transition("Reload");
            _weaponFSM.OnUpdate();
        }
    }

    public override void Sleep()
    {
        _playerView.StepsStop();
    }
}
