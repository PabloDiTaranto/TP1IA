using System;
using System.Linq;
using UnityEngine;

public class QueryTester : MonoBehaviour {

    private IQuery[]           _queries;
    private GridEntityTester[] _testers;
    private EnemyController[] _enemies;
    public SquareQuery _square;
    IGridEntity[] _example;

    IQuery charac;

    private void Awake() 
    {
        charac = FindObjectOfType<CharacterController>().GetComponent<IQuery>();
    }


    private void LateUpdate() {
        _queries = FindObjectsOfType<SquareQuery>();
        _testers = FindObjectsOfType<GridEntityTester>();
        var highlighted = _queries.SelectMany(n => charac.Query()).OfType<GridEntityTester>();
        var notHighlighted = _testers.Where(n => highlighted.All(x => n != x));

        /*foreach (var item in _testers)
        {
            if (charac.Query().Contains(item))
            {
                Debug.Log(item+"WW");
            }
        }*/


        foreach (var tester in highlighted) 
        {
            tester.onGrid = true;
            Debug.Log(tester.name + "ongrid");
        }

        foreach (var tester in notHighlighted) 
        {
            tester.onGrid = false;
            Debug.Log(tester);
        }
    }
}