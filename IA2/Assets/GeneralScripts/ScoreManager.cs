using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static int playerPoints, enemiesPoints;

    private void Awake()
    {
        playerPoints = 0;
        enemiesPoints = 0;
        EventManager.Subscribe("OnEnemyDestroy", AddPointsPlayer);
        EventManager.Subscribe("OnPlayerDead", AddPointsEnemies);
    }
    
    private void AddPointsPlayer(params object[] parameters)
    {
        playerPoints++;
    }

    private void AddPointsEnemies(params object[] parameters)
    {
        enemiesPoints++;
    }

    public static int[] GetPoints()
    {
        return new int[] {playerPoints, enemiesPoints};
    }
}
