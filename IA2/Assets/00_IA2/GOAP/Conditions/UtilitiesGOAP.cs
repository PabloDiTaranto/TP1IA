using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class UtilitiesGOAP 
{
    
    public static AbstractEnemy GetNearestEnemy(Vector3 position, LayerMask layerMask)
    {
        var radius = 50;
        Collider[] enemy = Physics.OverlapSphere(position, radius);

        List<AbstractEnemy> enemiesList = new List<AbstractEnemy>();
        foreach (var item in enemy)
        {
            if(item.gameObject.GetComponent<AbstractEnemy>())
                enemiesList.Add(item.GetComponent<AbstractEnemy>());
        }
        var obtainedEnemy = enemiesList
           .OrderBy(n => (n.transform.position - position).sqrMagnitude)
           .Where(n => !n._isDead&&n.enemyType != EnemyType.GOAP)
           .FirstOrDefault();

        Debug.Log(obtainedEnemy);
        return obtainedEnemy;
    }

    public static bool IsNeededWeapon(EnemyType type, AbstractEnemy currentEnemy)
    {
        if (currentEnemy!=null&&currentEnemy.enemyType == type)
        {
            Debug.Log("true");
            return true;
        }

        else
        {
            Debug.Log("false");
            return false;
        }
    }

    public static bool IsEnemyNear(Vector3 init, Vector3 finit)
    {
        return (finit - init).sqrMagnitude < 2 * 2;
    }


}
