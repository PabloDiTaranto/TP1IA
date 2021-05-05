using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerModel _playerModel;
    private PlayerView _playerView;
    private FSM<string> _fsm, _weaponFsm;
    [SerializeField]
    private float _rotSpeed = 180f;


    private void Awake()
    {
        //Time.timeScale = 1f;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _playerModel = GetComponent<PlayerModel>();
        _playerView = GetComponent<PlayerView>();

        var _fire = new FireStatePlayer<string>(_playerModel, _playerModel.GetFireRate(), _playerView);
        var _reload = new ReloadStatePlayer<string>(_playerModel, _playerModel.GetTimeToReload(), _playerView);

        _fire.AddTransition("Reload", _reload);
        _reload.AddTransition("Fire", _fire);

        _weaponFsm = new FSM<string>(_fire);

        var _idle = new IdleStatePlayer<string>(_playerModel, _weaponFsm, this);
        var _move = new MoveStatePlayer<string>(_playerModel, _weaponFsm, this, _playerView);
        var _dead = new DeadStatePlayer<string>(_playerModel, this, _playerView);
        var _respawn = new RespawnStatePlayer<string>(_playerModel, this);

        _idle.AddTransition("Move", _move);
        _idle.AddTransition("Dead", _dead);

        _move.AddTransition("Dead", _dead);
        _move.AddTransition("Idle", _idle);

        _dead.AddTransition("Respawn", _respawn);

        _respawn.AddTransition("Idle", _idle);
        _respawn.AddTransition("Move", _move);
        

        _fsm = new FSM<string>(_idle);        
    }

    private void Update()
    {
        _fsm.OnUpdate();

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        var rotHorX = Input.GetAxis("Mouse X");
        var rotVerY = Input.GetAxis("Mouse Y");

        if(h != 0 || v != 0)
        {
            _fsm.Transition("Move");
        }
        else
        {
            _fsm.Transition("Idle");
        }

        if (!_playerModel.isDead && !PauseManager.isPause && !GameTimeManager.isGameOver)
        {
            _playerModel.RotatePlayer(rotHorX, _rotSpeed);
            _playerModel.CamRotation(rotVerY);
        }
    }

    public void SwapFsm(string newState)
    {
        _fsm.Transition(newState);
    }

    private void OnTriggerEnter(Collider other)
    {
        var layer = other.gameObject.layer;
        if(layer == 12)
        {
            _playerModel.Damage();
            _playerView.Hurt();
        }

        if(layer == 13 && _playerModel.GetCurrentLife() < _playerModel.GetMaxLife())
        {
            _playerModel.Heal(other.GetComponent<MedKit>().GetLifeRecover());
        }
    }
}
