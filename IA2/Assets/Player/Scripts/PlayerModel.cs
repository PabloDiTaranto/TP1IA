using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public Rigidbody _rb;
    private Camera _playerCam;
    private float rotationY, currentLife;

    [SerializeField]
    private float speed, minimumY = -60f, maximumY = 60f, sensitivityY = 15f, fireRate = 0.2f, timeToReload = 2f, maxLife;
    public Transform bulletOrigin, spawnPosition;
    public GameObject bulletPrefab;
    public int ammoCount;
    [HideInInspector]
    public int originalAmmoCount;
    public bool isReloading;
    [HideInInspector]
    public bool isDead;
    public Vector3 _myDir;

    private void Awake()
    {
        currentLife = maxLife;        
        _rb = GetComponent<Rigidbody>();
        _playerCam = GetComponentInChildren<Camera>();
        originalAmmoCount = ammoCount;
    }

    private void Start()
    {
        EventManager.Trigger("OnAmmoChange", ammoCount);
        EventManager.Trigger("OnPlayerLifeChange", currentLife, maxLife);
    }

    public void Move(Vector3 dir)
    {
        _myDir = dir;
        transform.position += ((dir.x * transform.right) + (dir.z * transform.forward)).normalized * speed * Time.deltaTime;
    }

    public void CamRotation(float rotationOnY)
    {
        rotationY += rotationOnY * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        _playerCam.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
    }

    public void RotatePlayer(float rotationOnX, float turnSpeed)
    {
        transform.Rotate(0, rotationOnX * turnSpeed * Time.deltaTime, 0);
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    public float GetTimeToReload()
    {
        return timeToReload;
    }

    public void RestartLife()
    {
        currentLife = maxLife;
        EventManager.Trigger("OnPlayerLifeChange", currentLife, maxLife);
    }

    public void Damage()
    {
        if (!isDead)
        {
            currentLife--;
            EventManager.Trigger("OnPlayerLifeChange", currentLife, maxLife);
        }
    }

    public float GetCurrentLife()
    {
        return currentLife;
    }

    public float GetMaxLife()
    {
        return maxLife;
    }

    public void Heal(float qtyHeal)
    {
        currentLife += qtyHeal;
        if (currentLife > maxLife)
            currentLife = maxLife;
        EventManager.Trigger("OnPlayerLifeChange", currentLife, maxLife);
    }
}
