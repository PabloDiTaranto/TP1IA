using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [SerializeField] private float speed, lifeTime;
    private float timerLife;

    void Start()
    {
       EventManager.Subscribe("OnPlayerDead", DestroyBullet);
    }

    void Update()
    {
        if (timerLife > lifeTime)
            DestroyBullet();        
        else
            timerLife += Time.deltaTime;
        Move();
    }

    private void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void DestroyBullet(params object[] parameters)
    {
        EventManager.Unsubscribe("OnPlayerDead", DestroyBullet);
        Destroy(gameObject);
    }
    
}
