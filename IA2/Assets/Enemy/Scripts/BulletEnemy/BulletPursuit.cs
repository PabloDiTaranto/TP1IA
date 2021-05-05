using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPursuit : ISteering
{
    Rigidbody _rbTarget;
    Vector3 _playerDir;
    float _timePrediction;

    public BulletPursuit(Rigidbody rbTarget, float timePrediction, Vector3 playerDir)
    {
        _timePrediction = timePrediction;
        _rbTarget = rbTarget;
        _playerDir = playerDir;
    }

    public Vector3 GetDir(Vector3 _from)
    {
        Vector3 posPredicition = _rbTarget.transform.position + _playerDir * _timePrediction;
        return (posPredicition - _from).normalized;
    }
}
