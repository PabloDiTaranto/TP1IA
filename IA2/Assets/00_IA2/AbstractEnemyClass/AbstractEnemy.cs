using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    MELEE,
    RANGE
}
public abstract class AbstractEnemy: MonoBehaviour
{
    protected float _currentLife;

    [HideInInspector]
    public bool _isDead;
    public abstract void Damage();

    public string[] _items;

    public EnemyType enemyType;

}
