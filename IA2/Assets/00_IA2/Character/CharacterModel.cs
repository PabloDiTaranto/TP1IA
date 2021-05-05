using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    [SerializeField]
    float _speed;
    public float Speed { get { return _speed; } }

    [SerializeField]
    GameObject _bullet;

    public GameObject Bullet { get { return _bullet; } }


    [SerializeField]
    Transform _shootPoint;
    public Transform ShootPoint { get { return _shootPoint; } }

    public float  minimumY = -60f, maximumY = 60f, sensitivityY = 15f, rotationY, speedRot;

    public Camera _playerCam;

}
