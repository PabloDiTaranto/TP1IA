using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObtainEnergy : MonoBehaviour
{
    private CharacterModel _characterModel;
    private SquareQuery _query;
    void Awake()
    {
        _characterModel = FindObjectOfType<CharacterModel>();
        _query = FindObjectOfType<SquareQuery>();
    }

    //IA2-P2
    //IA2-P3
    public void GetEnergy()
    {
        var enemy = _query.Query().
            OfType<AbstractEnemy>()
            .ToArray();


        var result = enemy.
            SelectMany(n => n._items).
            Where(n => n == "Energy").
            ToArray();


        foreach (var item in result)
        {
            _characterModel._currentEnergy++;
        }

        for (int i = 0; i < enemy.Count() - 1; i++)
        {
            for (int j = 0; j < enemy[i]._items.Length; j++)
            {
                enemy[i]._items[j] = "Empty";
            }
        }
    }
}
