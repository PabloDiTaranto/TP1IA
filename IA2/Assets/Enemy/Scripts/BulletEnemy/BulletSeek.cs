using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSeek : ISteering
{
    Transform _target;

    public BulletSeek(Transform target)
    {
        _target = target;
    }

    public Vector3 GetDir(Vector3 _from)
    {
        return (_target.position - _from).normalized;
    }
}
