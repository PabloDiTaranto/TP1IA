using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static bool isPause;
    [SerializeField] private Canvas pauseCanvas;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !GameTimeManager.isGameOver)
        {
            PauseCanvas();
        }
    }

    public static void SwapTimeScale()
    {
        if (isPause)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    private void PauseCanvas()
    {
        isPause = !isPause;
        pauseCanvas.enabled = !pauseCanvas.isActiveAndEnabled;
        if (isPause)
        {
            EventManager.Trigger("OnPause");
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            EventManager.Trigger("OnUnpause");
            Cursor.lockState = CursorLockMode.Locked;
        }
        SwapTimeScale();
    }

    public void ResumeButton()
    {
        PauseCanvas();
    }
}
