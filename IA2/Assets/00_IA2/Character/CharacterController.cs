using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterController : MonoBehaviour//NO PUEDE USAR LA INTERFAZ IGRIDENTITY PORQUE LA GRILLA TIRA ERROR 
{
    public CharacterModel _characterModel;
    public delegate void PlayerFunctions();
    bool _canShoot;
    float _currentLife;
    public float CurrentLifePlayer { get { return _currentLife; } }
    bool _isDead;
    float lerpTimer;
    void Awake()
    {
        _characterModel = GetComponent<CharacterModel>();
        _characterModel._rb = GetComponent<Rigidbody>();
        _characterModel._playerCam = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        _currentLife = _characterModel.MaxLife;
        EventManager.Trigger("OnPlayerLifeChange", _currentLife, _characterModel.MaxLife);
        _canShoot = true;
        _isDead = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (_isDead)
        {
            Dead();
            return;
        }
        Debug.Log(lerpTimer);
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var rotHorX = Input.GetAxis("Mouse X");
        var rotVerY = Input.GetAxis("Mouse Y");
        CamRotation(rotVerY);
        RotatePlayer(rotHorX, _characterModel.speedRot);

        Move(horizontal, vertical);

        if (Input.GetKey(KeyCode.Mouse0) && _canShoot)
        {
            StartCoroutine("Timer");
        }
        
    }

    void Move(float horizontal, float vertical)
    {
        Vector3 dir = new Vector3(horizontal, 0, vertical);
        _characterModel._myDir = dir;
        transform.position += ((dir.x * transform.right) + (dir.z * transform.forward)).normalized * _characterModel.Speed * Time.deltaTime;
    }

    void Shoot()
    {
        Instantiate(_characterModel.Bullet, _characterModel.ShootPoint.position, _characterModel.ShootPoint.rotation);
    }

    public void CamRotation(float rotationOnY)
    {
        _characterModel.rotationY += rotationOnY * _characterModel.sensitivityY;
        _characterModel.rotationY = Mathf.Clamp(_characterModel.rotationY, _characterModel.minimumY, _characterModel.maximumY);
        _characterModel._playerCam.transform.localEulerAngles = new Vector3(-_characterModel.rotationY, 0, 0);
    }

    public void RotatePlayer(float rotationOnX, float turnSpeed)
    {
        transform.Rotate(0, rotationOnX * turnSpeed * Time.deltaTime, 0);
    }

    public void RestartLife()
    {
        _currentLife = _characterModel.MaxLife;
        EventManager.Trigger("OnPlayerLifeChange", _currentLife, _characterModel.MaxLife);
    }

    public void Damage()
    {
        if (!_isDead)
        {
            if (_currentLife-- <= 0)
            {
                Debug.Log("damage menor a 0");
                _isDead = true;
                EventManager.Trigger("OnPlayerDead");
            }
            EventManager.Trigger("OnPlayerLifeChange", _currentLife, _characterModel.MaxLife);
        }
    }

    public void Heal(float quantityHeal)
    {
        _currentLife += quantityHeal;
        if (_currentLife > _characterModel.MaxLife)
            _currentLife = _characterModel.MaxLife;
        EventManager.Trigger("OnPlayerLifeChange", _currentLife, _characterModel.MaxLife);
    }

    void Dead()
    {
        lerpTimer += Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 
            Mathf.Lerp(Quaternion.identity.z, 90, lerpTimer * 2));
        Debug.Log("Dead");
        if (lerpTimer > 2)
        {
            lerpTimer = 0;
            Respawn();
        }
    }

    void Respawn()
    {
        Debug.Log("respawn");
        transform.rotation = Quaternion.identity;
        transform.position = _characterModel._spawnPosition.position;
        RestartLife();
        _isDead = false;
        EventManager.Trigger("OnPlayerRespawn");
    }

    IEnumerator Timer()
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
