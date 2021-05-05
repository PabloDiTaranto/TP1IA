using UnityEngine;

public class PursuitObstacleAvoidance : ISteering
{
    private Rigidbody _target;
    private float _radius, _avoidWeight, _timePrediction;

    private LayerMask _mask;

    public PursuitObstacleAvoidance(Rigidbody target, float radius, float avoidWeight, LayerMask mask, float timePrediction)
    {
       
        _target = target;
        _radius = radius;
        _avoidWeight = avoidWeight;
        _mask = mask;
        _timePrediction = timePrediction;
    }

    public Vector3 GetDir(Vector3 _from)
    {
        Vector3 dir = _target.transform.position + _target.velocity * _timePrediction;

        Collider[] obstacles = Physics.OverlapSphere(_from, _radius, _mask);
        if (obstacles.Length > 0)
        {
            float distance = Vector3.Distance(obstacles[0].transform.position, _from);
            int indexSave = 0;
            for (int i = 1; i < obstacles.Length; i++)
            {
                float currDistance = Vector3.Distance(obstacles[i].transform.position, _from);
                if (currDistance < distance)
                {
                    distance = currDistance;
                    indexSave = i;
                }
            }
            Vector3 dirFromObs = (_from - obstacles[indexSave].transform.position).normalized * ((_radius - distance) / _radius) * _avoidWeight;
            dir += dirFromObs;
        }
        return (dir - _from).normalized;
    }
}
