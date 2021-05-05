using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    CharacterModel _characterModel;
    public delegate void PlayerFunctions();
    bool _canShoot;
    void Awake()
    {
        _characterModel = GetComponent<CharacterModel>();
        _canShoot = true;

        _characterModel._playerCam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
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

    


    IEnumerator Timer()
    {
        _canShoot = false;
        Shoot();
        yield return new WaitForSeconds(0.2f);
        _canShoot = true;
    }
}
