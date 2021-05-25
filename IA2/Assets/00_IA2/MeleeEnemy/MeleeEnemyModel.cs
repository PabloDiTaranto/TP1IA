using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyModel : MonoBehaviour
{
    public LayerMask _maskAvoidance;
    public Rigidbody _rb;
    public Transform _target;
    public float _radius, _avoidWeight, _distanceToAttack, _speed, _speedRot, _timeToCheckPlayerPos, _timeToRespawn, _timePredictionChase, _attackRate;
    public Collider _attackTrigger;
    //public Vector3 _playerDir;
    private void Awake()
    {
        var player = FindObjectOfType<CharacterModel>();
        _target = player.transform;
        _rb = player._rb;
        //_playerDir = player._myDir;
    }
}
