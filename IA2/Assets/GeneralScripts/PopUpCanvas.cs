using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpCanvas : MonoBehaviour
{
    [SerializeField] private GameObject popUpPanel;
    [SerializeField] private TextMeshProUGUI playerKills, enemiesKills, resultText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] gameOverSounds;

    private void Awake()
    {
        EventManager.Subscribe("OnGameOver", ShowPopUp);
    }

    public void ShowPopUp(params object[] parameters)
    {
        Cursor.lockState = CursorLockMode.None;
        var kills = ScoreManager.GetPoints();
        playerKills.text = "Kills: " + kills[0].ToString();
        enemiesKills.text = "Deaths: " + kills[1].ToString();
        if (kills[0] >= 15 && kills[0] > kills[1])
        {
            audioSource.Stop();
            audioSource.clip = gameOverSounds[0];
            audioSource.loop = false;
            audioSource.Play();
            resultText.text = "You Win :D";
        }
        else
        {
            audioSource.Stop();
            audioSource.clip = gameOverSounds[1];
            audioSource.loop = false;
            audioSource.Play();
            resultText.text = "You Lose :C";
        }            
        popUpPanel.SetActive(true);
    }

    public void RestartButton()
    {
        MySceneManager.ChangeScene("MainLevel");
    }

    public void MenuButton()
    {
        MySceneManager.ChangeScene("MainMenu");
    }
}
