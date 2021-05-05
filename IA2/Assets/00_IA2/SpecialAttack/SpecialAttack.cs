using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpecialAttack : MonoBehaviour
{
    CharacterController _character;
    SquareQuery _query;
    void Start()
    {
        _character = FindObjectOfType<CharacterController>();
    }

    public void Attack()
    {
        var result = _query.Query()
            .OfType<EnemyController>()
            .OrderBy(n => (_character.transform.position - n.transform.position).sqrMagnitude)
            .Take(3);

        foreach(var item in result)
        {
            
        }
        
    }
}
