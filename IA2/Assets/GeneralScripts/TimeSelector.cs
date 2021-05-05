using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSelector : MonoBehaviour
{    
    public static float gameTime = 180f;

    public void ButtonFunction(float newGameTime)
    {
        gameTime = newGameTime;
        MySceneManager.ChangeScene("MainLevel");
    }

}
