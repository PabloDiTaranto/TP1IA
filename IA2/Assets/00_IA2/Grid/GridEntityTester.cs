using System;
using UnityEngine;

public class GridEntityTester : MonoBehaviour, IGridEntity {
    
    public event Action<IGridEntity> OnMove;

    public  Vector3                 velocity = new Vector3(0, 0, 0);
    public  bool                    onGrid;
    private Renderer                _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        OnMove += SpatialGrid._instance.UpdateEntity;
        SpatialGrid._instance.UpdateEntity(this);
    }

    void Update() {
        if (onGrid)
            Debug.Log("En Grilla");
        //_renderer.material.color = Color.red;
        else
            Debug.Log("NO");
            //_renderer.material.color = Color.gray;
        
        transform.position += velocity * Time.deltaTime;
        OnMove?.Invoke(this);
    }
    
    public Vector3 Position {
        get => transform.position;
        set => transform.position = value;
    }
}