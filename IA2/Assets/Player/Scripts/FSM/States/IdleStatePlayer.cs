using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStatePlayer<T> : State<T>
{
    private PlayerModel _playerModel;
    private PlayerController _playerController;
    private FSMachine<string> _weaponFSM;

    public IdleStatePlayer(PlayerModel player, FSMachine<string> weaponFSM, PlayerController playerController)
    {
        _playerModel = player;
        _weaponFSM = weaponFSM;
        _playerController = playerController;
    }

    public override void Execute()
    {
        if (_playerModel.GetCurrentLife() <= 0)
        {
            _playerController.SwapFsm("Dead");
        }
        _playerModel.Move(Vector3.zero);
        if (Input.GetKey(KeyCode.Mouse0) && _playerModel.ammoCount > 0 && !_playerModel.isReloading && !PauseManager.isPause)
        {
            _weaponFSM.Transition("Fire");
            _weaponFSM.OnUpdate();
        }

        if ((((Input.GetKeyDown(KeyCode.R) || _playerModel.ammoCount <= 0) && _playerModel.ammoCount < _playerModel.originalAmmoCount) || _playerModel.isReloading) && !PauseManager.isPause && !GameTimeManager.isGameOver)
        {
            _weaponFSM.Transition("Reload");
            _weaponFSM.OnUpdate();
        }
    }
}
