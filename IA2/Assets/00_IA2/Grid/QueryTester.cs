using System;
using System.Linq;
using UnityEngine;

public class QueryTester : MonoBehaviour {

    private IQuery[]           _queries;
    private GridEntityTester[] _testers;
    public SquareQuery _square;

    private void Awake() {
        _queries = FindObjectsOfType<SquareQuery>();
        _testers = FindObjectsOfType<GridEntityTester>();
    }

    private void LateUpdate() {
        var highlighted = _queries.SelectMany(n => n.Query()).OfType<GridEntityTester>();
        var notHighlighted = _testers.Where(n => highlighted.All(x => n != x));
        Debug.Log(_queries.Length);
        Debug.Log(_testers.Length);


        foreach (var tester in highlighted) 
        {
            tester.onGrid = true;
            Debug.Log(tester + "ongrid");
        }

        foreach (var tester in notHighlighted) 
        {
            tester.onGrid = false;
            Debug.Log(tester);
        }
    }
}