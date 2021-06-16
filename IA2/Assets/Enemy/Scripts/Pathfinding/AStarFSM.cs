using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarFSM<T>
{
    public delegate bool Satisfies(T curr);
    public delegate List<T> GetNeighbours(T curr);
    public delegate float GetCost(T father, T child);
    public delegate float Heuristic(T current);
    public List<T> Run(T start, Satisfies satisfies, GetNeighbours getNeighbours, GetCost getcost, Heuristic heuristic, int watchDog = 500)
    {
        Dictionary<T, float> cost = new Dictionary<T, float>();
        Dictionary<T, T> parents = new Dictionary<T, T>();
        PriorityQueueFSM<T> pending = new PriorityQueueFSM<T>();
        HashSet<T> visited = new HashSet<T>();
        pending.Enqueue(start, 0);
        cost.Add(start, 0);
        while (!pending.IsEmpty)
        {
            T current = pending.Dequeue();
            watchDog--;
            if (watchDog <= 0) return new List<T>();
            if (satisfies(current))
            {
                return ConstructPath(current, parents);
            }
            visited.Add(current);
            List<T> neighbours = getNeighbours(current);
            for (int i = 0; i < neighbours.Count; i++)
            {
                T node = neighbours[i];
                if (visited.Contains(node)) continue;
                float nodeCost = getcost(current, node);
                float totalCost = cost[current] + nodeCost;
                if (cost.ContainsKey(node) && cost[node] < totalCost) continue;
                cost[node] = totalCost;
                parents[node] = current;
                pending.Enqueue(node, totalCost + heuristic(node));
            }
        }
        return new List<T>();
    }

    List<T> ConstructPath(T end, Dictionary<T, T> parents)
    {
        var path = new List<T>();
        path.Add(end);
        while (parents.ContainsKey(path[path.Count - 1]))
        {
            var lastNode = path[path.Count - 1];
            path.Add(parents[lastNode]);
        }
        path.Reverse();
        return path;
    }
    List<T> ConstructPath2(T end, Dictionary<T, T> parents)
    {
        var path = new List<T>();
        path.Add(end);
        T currentGrandFather = end;
        while (parents.ContainsKey(path[path.Count - 1]))
        {
            var lastNode = path[path.Count - 1];
            path.Add(parents[lastNode]);
        }
        path.Reverse();
        return path;
    }
}
