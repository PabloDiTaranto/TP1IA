using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpecialAttack : MonoBehaviour
{
    CharacterController _character;
    SquareQuery _query;
    [SerializeField]
    int _layerMask;

    EnemyController _enemies;
    bool once;

    void Start()
    {
        _character = FindObjectOfType<CharacterController>();
        _query = FindObjectOfType<SquareQuery>();
        _layerMask = 1 << LayerMask.NameToLayer("Enemy");
    }

    private void Update()
    {
        /*if(!once)
        _enemies = FindObjectOfType<EnemyController>();
        if (_enemies != null)
        {
            Debug.Log(_enemies.name);
            once = true;
            RaycastHit hit;
            var twoDimensionPlayer = new Vector3(_character.transform.position.x, 0.5f, _character.transform.position.z);
            var twoDimensionEnemy = new Vector3(_enemies.transform.position.x, 0.5f, _enemies.transform.position.z);
            var dir = twoDimensionEnemy - twoDimensionPlayer;
            if (Physics.Raycast(_character.transform.position, dir, out hit, 5000f))
            {
                //if (hit.collider.gameObject.layer == _layerMask)
                //{
                    print("Ray");
                    Debug.DrawRay(transform.position, dir
                        * hit.distance, Color.magenta);
                //}
            }
        }*/
    }

    public void Attack()
    {
        //IEnumerable result;
        var result = _query.Query()
            .OfType<EnemyController>()
            .OrderBy(n => (_character.transform.position - n.transform.position).sqrMagnitude)
            .Where(n => !n._isDead&&n.isActiveAndEnabled)
            .Take(3);


        //if (result == null) return;

        RaycastHit hit;
        foreach (var item in result)
        {
            if (!item.isActiveAndEnabled) continue;
            Debug.Log(item);
            //if (item!=null) continue;

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
