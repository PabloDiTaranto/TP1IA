using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MySceneManager
{
    public static void ChangeScene(string newScene)
    {
        EventManager.ClearDic();
        SceneManager.LoadScene(newScene);
        PauseManager.isPause = false;
        Time.timeScale = 1f;
    }
}
