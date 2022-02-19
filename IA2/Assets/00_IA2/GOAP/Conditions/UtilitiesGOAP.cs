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
            if (item.gameObject.GetComponent<AbstractEnemy>())
                enemiesList.Add(item.GetComponent<AbstractEnemy>());
        }
        var obtainedEnemy = enemiesList
           .OrderBy(n => (n.transform.position - position).sqrMagnitude)
           .Where(n => !n._isDead && n.enemyType != EnemyType.GOAP)
           .FirstOrDefault();

        //Debug.Log(obtainedEnemy);
        return obtainedEnemy;
    }

    public static int IsNeededWeaponMelee(EnemyType type, AbstractEnemy currentEnemy)
    {
        //(int a, int b) => {if(a<b)return 0;else return 1;}//Lambda de ejemplo
        //(AbstractEnemy currentEnemy) => { if (currentEnemy != null && currentEnemy.enemyType == type) return 1; else return 0; }

        if (currentEnemy!=null&&currentEnemy.enemyType == type)
        {
            return 1;
        }

        else
        {
            return 0;
        }
    }
    
    public static float IsNeededWeaponRange(EnemyType type, AbstractEnemy currentEnemy)
    {
        //(AbstractEnemy currentEnemy) => { if(currentEnemy != null && currentEnemy.enemyType == type) return 1f;else return 0f;}
        if (currentEnemy!=null && currentEnemy.enemyType == type)
        {
            return 1f;
        }

        else
        {
            return 0f;
        }
    }

    public static string IsEnemyNear(Vector3 init, Vector3 finit)
    {
        //(Vector3 init, Vector3 finit) => { if ((finit - init).sqrMagnitude < 2 * 2) return "true"; else return "false"; }
        return (finit - init).sqrMagnitude < 2 * 2 ? "true" : "false";
    }


}
