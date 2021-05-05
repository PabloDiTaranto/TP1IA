using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringStateEnemy<T> : State<T>
{
    private EnemyController _enemy;
    private EnemyView _enemyView;
    private Transform _target, _spawnBullet;
    private Rigidbody _enemyRB;
    private LineOfSight _lOS;
    private float _fireRate, _timerRate;
    private GameObject _bulletPrefab;

    private ISteering _seek, _pursuit;

    private Roulette roulette;
    private Dictionary<ISteering, int> dic = new Dictionary<ISteering, int>();
    private ISteering sb;

    public FiringStateEnemy(EnemyController enemy, Transform target, Rigidbody enemyRB, LineOfSight lOS, float fireRate, GameObject bulletPrefab, Transform spawnBullet, ISteering seek, ISteering pursuit, EnemyView enemyView)
    {
        _enemy = enemy;
        _target = target;
        _enemyRB = enemyRB;
        _lOS = lOS;
        _fireRate = fireRate;
        _bulletPrefab = bulletPrefab;
        _spawnBullet = spawnBullet;
        _seek = seek;
        _pursuit = pursuit;
        _enemyView = enemyView;
    }
    public override void Awake()
    {
        roulette = new Roulette();
        if (!dic.ContainsKey(_seek)) dic.Add(_seek, 50);
        if (!dic.ContainsKey(_pursuit)) dic.Add(_pursuit, 50);
        _timerRate = _fireRate/5;
        _enemyView.StepsStop();
        _enemyView.ShootAnimation();
    }


    public override void Execute()
    {
        _enemy.transform.LookAt(_target);
        if (_timerRate >= _fireRate)
        {
            Shoot();
        }
        else
        {
            _timerRate += Time.deltaTime;
        }
        if (!_enemy.HasLife())
            _enemy.isTimeToRespawn.Execute();
        if (!_lOS.IsInSight(_target))
            _enemy.isPlayerOnSight.Execute();
        if (!_enemy.CheckDistanceToFire())
            _enemy.isOnDistanceToFire.Execute();
    }

    private void Shoot()
    {        
        sb = roulette.Run(dic);
        _bulletPrefab.transform.position = _spawnBullet.position;
        _bulletPrefab.transform.forward = sb.GetDir(_spawnBullet.transform.position);
        Object.Instantiate(_bulletPrefab);
        _enemyView.Shoot();
        _timerRate = 0f;        
    }
}
