using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterController : MonoBehaviour
{
    private SpecialAttack _specialAttack;
    private CharacterModel _characterModel;
    private ObtainEnergy _obtainEnergy;
    private float lerpTimer;
    private bool _canShoot;
    private bool _isDead;
    public bool IsDead { get { return _isDead; } }

    private void Awake()
    {
        _characterModel = GetComponent<CharacterModel>();
        _characterModel._rb = GetComponent<Rigidbody>();
        _characterModel._playerCam = GetComponentInChildren<Camera>();
        _specialAttack = GetComponent<SpecialAttack>();
        _characterModel._playerName = PlayerNameManager._name;
        _obtainEnergy = GetComponent<ObtainEnergy>();
    }

    private void Start()
    {
        _characterModel.CurrentLife = _characterModel.MaxLife;
        EventManager.Trigger("OnPlayerLifeChange", _characterModel.CurrentLife, _characterModel.MaxLife);
        EventManager.Trigger("OnChangedEnergy", _characterModel._currentEnergy);
        _canShoot = true;
        _isDead = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        if (_isDead)
        {
            Dead();
            return;
        }

        if (PauseManager.isPause) return;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var rotHorX = Input.GetAxis("Mouse X");
        var rotVerY = Input.GetAxis("Mouse Y");
        CamRotation(rotVerY);
        RotatePlayer(rotHorX, _characterModel._speedRot);

        Move(horizontal, vertical);

        if (Input.GetKey(KeyCode.Mouse0) && _canShoot)
        {
            StartCoroutine("Timer");
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (_characterModel._currentEnergy <= 0) return;

            _specialAttack.Attack();
            _characterModel._currentEnergy--;
            EventManager.Trigger("OnChangedEnergy", _characterModel._currentEnergy);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _obtainEnergy.GetEnergy();
            EventManager.Trigger("OnChangedEnergy", _characterModel._currentEnergy);
        }

    }

    private void Move(float horizontal, float vertical)
    {
        Vector3 dir = new Vector3(horizontal, 0, vertical);
        _characterModel._myDir = dir;
        transform.position += ((dir.x * transform.right) + (dir.z * transform.forward)).normalized * _characterModel.Speed * Time.deltaTime;
    }

    private void Shoot()
    {
        Instantiate(_characterModel.Bullet, _characterModel.ShootPoint.position, _characterModel.ShootPoint.rotation);
    }

    public void CamRotation(float rotationOnY)
    {
        _characterModel._rotationY += rotationOnY * _characterModel.SensitivityY;
        _characterModel._rotationY = Mathf.Clamp(_characterModel._rotationY, _characterModel.MinimumY, _characterModel.MaximumY);
        _characterModel._playerCam.transform.localEulerAngles = new Vector3(-_characterModel._rotationY, 0, 0);
    }

    public void RotatePlayer(float rotationOnX, float turnSpeed)
    {
        transform.Rotate(0, rotationOnX * turnSpeed * Time.deltaTime, 0);
    }

    public void RestartLife()
    {
        _characterModel.CurrentLife = _characterModel.MaxLife;
        EventManager.Trigger("OnPlayerLifeChange", _characterModel.CurrentLife, _characterModel.MaxLife);
    }

    public void Damage()
    {
        if (!_isDead)
        {
            if (_characterModel.CurrentLife-- <= 0)
            {
                _isDead = true;
                EventManager.Trigger("OnPlayerDead");
            }
            EventManager.Trigger("OnPlayerLifeChange", _characterModel.CurrentLife, _characterModel.MaxLife);
        }
    }

    public void Heal(float quantityHeal)
    {
        _characterModel.CurrentLife += quantityHeal;
        if (_characterModel.CurrentLife > _characterModel.MaxLife)
            _characterModel.CurrentLife = _characterModel.MaxLife;
        EventManager.Trigger("OnPlayerLifeChange", _characterModel.CurrentLife, _characterModel.MaxLife);
    }

    private void Dead()
    {
        lerpTimer += Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 
            Mathf.Lerp(Quaternion.identity.z, 90, lerpTimer * 2));
        if (lerpTimer > 2)
        {
            lerpTimer = 0;
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.rotation = Quaternion.identity;
        transform.position = _characterModel._spawnPosition.position;
        RestartLife();
        _isDead = false;
        EventManager.Trigger("OnPlayerRespawn");
    }

    private IEnumerator Timer()
    {
        _canShoot = false;
        Shoot();
        yield return new WaitForSeconds(0.2f);
        _canShoot = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 12)
        {

            Damage();
        }

        if (other.gameObject.layer == 13 && _characterModel.CurrentLife < _characterModel.MaxLife)
        {
            Heal(other.GetComponent<MedKit>().GetLifeRecover());
        }
    }

}
