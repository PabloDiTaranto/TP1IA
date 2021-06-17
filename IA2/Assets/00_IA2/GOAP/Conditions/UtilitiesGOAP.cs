using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class UtilitiesGOAP 
{
    
    public static AbstractEnemy GetNearestEnemy(Vector3 position, LayerMask layerMask)
    {
        var radius = 50;///REVISAR
        Collider[] enemy = Physics.OverlapSphere(position, radius, layerMask);

        return enemy.OfType<AbstractEnemy>()
           .OrderBy(n => (position - n.transform.position).sqrMagnitude)
           .Where(n => !n._isDead)
           .FirstOrDefault();
    }

    public static bool IsNeededWeapon(EnemyType type, AbstractEnemy currentEnemy)
    {
        if (currentEnemy.enemyType == type)
            return true;

        else
            return false;
    }
}
