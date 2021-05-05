using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnsTransforms;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemies;
    [SerializeField] private float timeBetweenSpawns;
    private float spawnTimer;
    private int enemiesOnScene;
    private bool isPlayerDead;
    private Dictionary<Transform, int> dic = new Dictionary<Transform, int>();

    private RouletteSpawnEnemies rouletteSpawn;

    private void Awake()
    {
        EventManager.Subscribe("OnEnemyDestroy", SubstractEnemyOnScene);
        EventManager.Subscribe("OnPlayerDead", SwapPlayerLifeState);
        EventManager.Subscribe("OnPlayerRespawn", SwapPlayerLifeState);
        rouletteSpawn = new RouletteSpawnEnemies();
    }
    private void Update()
    {
        if(enemiesOnScene < maxEnemies && spawnTimer > timeBetweenSpawns && !isPlayerDead)
        {
            SpawnEnemy();
            spawnTimer = 0;
        }
        else
        {
            spawnTimer += Time.deltaTime;
        }
    }

    private void SpawnEnemy()
    {
        dic.Clear();
        foreach (var spawn in spawnsTransforms)
        {
            dic.Add(spawn, (int)Vector3.Distance(spawn.position, playerTransform.position));
        }
        var spawnPos = rouletteSpawn.Run(dic);
        Instantiate(enemyPrefab, spawnPos.position, Quaternion.identity);
        enemiesOnScene++;
    }

    private void SubstractEnemyOnScene(params object[] parameters)
    {
        enemiesOnScene--;
    }

    private void SwapPlayerLifeState(params object[] parameters)
    {
        enemiesOnScene = 0;
        isPlayerDead = !isPlayerDead;
    }
}
