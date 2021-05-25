using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpecialAttack : MonoBehaviour
{
    private CharacterController _character;
    private SquareQuery _query;
    //int _layerMask;

    void Start()
    {
        _character = FindObjectOfType<CharacterController>();
        _query = FindObjectOfType<SquareQuery>();
        //_layerMask = 1 << LayerMask.NameToLayer("Enemy");
    }

    public void Attack()
    {
        //IA2-P2
        //IA2-P3

        var result = _query.Query()
            .OfType<AbstractEnemy>()
            .OrderBy(n => (_character.transform.position - n.transform.position).sqrMagnitude)
            .Where(n => !n._isDead)
            .Take(3);

        RaycastHit hit;
        foreach (var item in result)
        {
            Debug.Log(item);
            //if (!item.isActiveAndEnabled) continue;
            //if (item!=null) continue;
            //if (item.Equals(null)) continue;

            var twoDimensionPlayer = new Vector3(_character.transform.position.x, 0.5f, _character.transform.position.z);
            var twoDimensionEnemy = new Vector3(item.transform.position.x, 0.5f, item.transform.position.z);
            var dir = twoDimensionEnemy - twoDimensionPlayer;
            if (Physics.Raycast(_character.transform.position, dir, out hit, 5000f))
            {
                if (hit.collider.gameObject == item.gameObject)
                {
                    Debug.DrawRay(transform.position, dir
                        * hit.distance, Color.magenta);
                    print(hit.transform);
                    item.Damage();
                    print("Ray");
                }
            }
        }
    }
}
