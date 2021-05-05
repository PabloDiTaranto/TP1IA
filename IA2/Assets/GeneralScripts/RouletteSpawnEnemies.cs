using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteSpawnEnemies
{
    public Transform Run(Dictionary<Transform, int> dic)
    {
        int total = 0;
        foreach (var item in dic)
        {
            total += item.Value;
        }
        int random = Random.Range(0, total);
        foreach (var item in dic)
        {
            random = random - item.Value;
            if (random < 0)
            {
                return item.Key;
            }
        }
        return Run(dic);
    }
}
