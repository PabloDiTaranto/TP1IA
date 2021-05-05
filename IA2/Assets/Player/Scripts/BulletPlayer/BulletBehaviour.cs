using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float speed, lifeTime;

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0f) Destroy(gameObject);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
