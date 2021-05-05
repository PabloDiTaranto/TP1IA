using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtons : MonoBehaviour
{
    public void RestartButton()
    {
        MySceneManager.ChangeScene("MainLevel");
    }

    public void QuitButton()
    {
        MySceneManager.ChangeScene("MainMenu");
    }
}
