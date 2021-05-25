
//IA2-P1
//(Este enemigo fue desarrollado durante la cursada de IA1)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : MonoBehaviour
{
    public Transform target, spawnBullets;
    public Rigidbody rbPlayer;
    public Vector3 playerDir;
    public float radius, avoidWeight, distanceToFire, speed, speedRot, timeToCheckPlayerPos, timeToRespawn, timePredictionChase, timePredictionBullet, fireRate;
    public LayerMask maskAvoidance;
    public GameObject bulletPrefab;

    private void Awake()
    {
        var player = FindObjectOfType<CharacterModel>();
        target = player.transform;
        rbPlayer = player._rb;
        playerDir = player._myDir;
    }
}
