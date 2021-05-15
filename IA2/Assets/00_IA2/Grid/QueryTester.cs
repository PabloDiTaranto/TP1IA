using System;
using System.Linq;
using UnityEngine;

public class QueryTester : MonoBehaviour {

    private IQuery[]           _queries;
    //private GridEntityTester[] _testers;
    [SerializeField] 
    EnemyController[] _testers;
    //private EnemyController[] _enemies;
    public SquareQuery _square;
    IGridEntity[] _example;

    IQuery charac;

    private void Awake() 
    {
        charac = FindObjectOfType<CharacterController>().GetComponent<IQuery>();
    }


    private void Update() {
        _queries = FindObjectsOfType<SquareQuery>();
        _testers = FindObjectsOfType<EnemyController>();
        // var highlighted = _queries.SelectMany(n => charac.Query()).OfType<GridEntityTester>();
        // var notHighlighted = _testers.Where(n => highlighted.All(x => n != x));


        var toListArray = _testers.ToList();
        for (int i = 0; i < toListArray.Count-1; i++)
        {
            if (toListArray[i])//Si se enciende este script tira error porque la lista sigue teniendo guardada la referencia al enemigo aunque sea destruido
                               //y no sirve preguntar si es nulo porque dentro de la lista no lo es porque sigue teniendo la referencia.
                               //Si se quisiera borrar de la lista, se tendria que sacar en el momento que el objeto va a destruirse ademas
                               //para tener referencia al objeto y saber cual sacar.
            {
                if (charac.Query().Contains(toListArray[i]))
                {
                    Debug.Log(_testers[i]);
                }
            }
            else
                toListArray.RemoveAt(i);
        }
        /*foreach (var item in _testers)
        {
            if(item == null)
            {
                _testers.ToList().Remove(item);
            }
            if (charac.Query().Contains(item))
            {
                Debug.Log(item);
            }
        }*/


        /*foreach (var tester in highlighted) 
        {
            tester.onGrid = true;
            Debug.Log(tester.name + "ongrid");
        }

        foreach (var tester in notHighlighted) 
        {
            tester.onGrid = false;
            Debug.Log(tester);
        }*/
    }
}