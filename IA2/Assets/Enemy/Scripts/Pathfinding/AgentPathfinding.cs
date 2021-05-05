using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentPathfinding : MonoBehaviour
{
    private List<Vector3> _listVector;
    [SerializeField] private LayerMask maskPF;
    private AStar<Vector3> _aStarVector = new AStar<Vector3>();
    [SerializeField] private Transform _target;
    [SerializeField] private float distanceMax;

    public List<Vector3> PathFindingAstarVector(Transform init)
    {
        _target = FindObjectOfType<PlayerModel>().transform;
        Vector3 posInit = init.transform.position;
        var path = _aStarVector.Run(posInit, SatisfiesVector, GetNeighbours, GetCost, HeuristicVector, 1000);
        _listVector = PathCleaner(path);
        return _listVector;
    }

    bool SatisfiesVector(Vector3 pos)
    {
        return Vector3.Distance(pos, _target.transform.position) <= distanceMax;
    }

    List<Vector3> GetNeighbours(Vector3 curr)
    {
        var list = new List<Vector3>();
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0) continue;
                Vector3 newPos = new Vector3(curr.x + x, curr.y, curr.z + z);
                if (!InSight(curr, newPos)) continue;
                list.Add(newPos);
            }
        }
        return list;
    }

    float GetCost(Vector3 from, Vector3 to)
    {
        return Vector3.Distance(from, to);
    }

    float HeuristicVector(Vector3 curr)
    {
        return Vector3.Distance(curr, _target.transform.position);
    }

    bool InSight(Vector3 gP, Vector3 gC)
    {
        Vector3 dir = gC - gP;
        return !Physics.Raycast(gP, dir.normalized, dir.magnitude, maskPF);
    }

    public List<Vector3> PathCleaner(List<Vector3> path)
    {
        if (path.Count == 0) return path;
        if (path.Count <= 2)
        {
            return path;
        }
        else
        {
            var list = new List<Vector3>();
            list.Add(path[0]);
            if (path.Count > 1)
            {
                for (int i = 1; i < path.Count; i++)
                {
                    var curr = path[i];
                    if (InSight(list[list.Count - 1], curr)) continue;
                    list.Add(path[i - 1]);
                }
                list.Add(path[path.Count - 1]);
            }
            return list;
        }
    }
}
