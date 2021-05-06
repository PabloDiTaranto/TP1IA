using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleQuery : MonoBehaviour, IQuery {

    public SpatialGrid targetGrid;
    public float radius = 5f;


    public IEnumerable<IGridEntity> Query() {


        return targetGrid.Query(transform.position + new Vector3(-radius, 0, -radius),
                                transform.position + new Vector3(radius,  0, radius),
                                x => Vector3.Distance(x, transform.position) <= radius);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
