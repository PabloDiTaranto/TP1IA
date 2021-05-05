using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    private float gameTime = 40;
    private bool isPlayerDead;
    public static bool isGameOver;

    private void Awake()
    {
        EventManager.Subscribe("OnPlayerDead", SwapPlayerDead);
        EventManager.Subscribe("OnPlayerRespawn", SwapPlayerDead);        
        gameTime = TimeSelector.gameTime;
        isGameOver = false;
    }

    private void Start()
    {
        EventManager.Trigger("OnTimeMod", gameTime);
    }

    private void Update()
    {
        if(gameTime <= 0 && !isGameOver)
        {
            GameOver();
        }
        else if(!isPlayerDead)
        {
            gameTime -= Time.deltaTime;
            EventManager.Trigger("OnTimeMod", gameTime);
        }
    }

    public void SetGameTime(float newTime)
    {
        gameTime = newTime;
    }

    private void SwapPlayerDead(params object[] parameters)
    {
        isPlayerDead = !isPlayerDead;
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        isGameOver = true;
        EventManager.Trigger("OnGameOver");
    }
}
