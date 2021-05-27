using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    #region Speed Values
    [SerializeField]
    private float _speed;
    public float Speed { get { return _speed; } }
    #endregion

    #region Attack Values
    [SerializeField]
    private GameObject _bullet;
    public GameObject Bullet { get { return _bullet; } }

    public Transform _spawnPosition;

    public int _currentEnergy = 0;
    #endregion

    #region Shoot Point
    [SerializeField]
    private Transform _shootPoint;
    public Transform ShootPoint { get { return _shootPoint; } }
    #endregion

    #region Rigidbody
    //[HideInInspector]
    public Rigidbody _rb;
    #endregion

    #region Direction
    [HideInInspector]
    public Vector3 _myDir;
    #endregion

    #region Camera Values

    [HideInInspector]
    public float _rotationY;

    public float _speedRot;

    private float _minimumY = -60f, _maximumY = 60f, _sensitivityY = 3f;
    public float MinimumY { get { return _minimumY; } }
    public float MaximumY { get { return _maximumY; } }
    public float SensitivityY { get { return _sensitivityY; } }

    public Camera _playerCam;
    #endregion

    #region Life Values

    private float _currentLife, _maxLife = 50f;
    public float CurrentLife { get { return _currentLife; } set { _currentLife = value; } }
    public float MaxLife { get { return _maxLife; } }

    #endregion

    #region Ranking Values
    public int _newScore;

    public string _playerName;
    #endregion

}
