using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _popUpPanel;
    [SerializeField] private TextMeshProUGUI playerKills, enemiesKills, resultText,_rankingsText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] gameOverSounds;
    [SerializeField] private Button[] _uiButtons;


    RankingResults _rankingResults;

    private void Awake()
    {
        EventManager.Subscribe("OnGameOver", ShowPopUp);
        _rankingResults = FindObjectOfType<RankingResults>();
    }

    public void ShowPopUp(params object[] parameters)
    {
        Cursor.lockState = CursorLockMode.None;
        var kills = ScoreManager.GetPoints();
        playerKills.text = "Kills: " + kills[0].ToString();
        enemiesKills.text = "Deaths: " + kills[1].ToString();

        _popUpPanel.SetActive(true);
        if (kills[0] >= 15 && kills[0] > kills[1])//////////////////
        {
            _rankingsText.gameObject.SetActive(false);
            audioSource.Stop();
            audioSource.clip = gameOverSounds[0];
            audioSource.loop = false;
            audioSource.Play();
            resultText.text = "You Win :D";
           // _popUpPanel.SetActive(true);
            UIFinishedButtons(false,true);
        }
        else
        {
            _rankingsText.gameObject.SetActive(false);
            audioSource.Stop();
            audioSource.clip = gameOverSounds[1];
            audioSource.loop = false;
            audioSource.Play();
            resultText.text = "You Lose :C";
            //_popUpPanel.SetActive(true);
            UIFinishedButtons(true,false);
        }            
    }

    void UIFinishedButtons(bool panelButtonsState, bool rankingButtonsState)
    {
        for (int i = 0; i < _uiButtons.Length; i++)
        {
            _uiButtons[i].gameObject.SetActive(panelButtonsState);

            if (i == _uiButtons.Length-1)
            {
                _uiButtons[i].gameObject.SetActive(rankingButtonsState);
            }
        }
    }

    public void ShowRankingsPopUp()
    {
        playerKills.gameObject.SetActive(false);
        enemiesKills.gameObject.SetActive(false);
        resultText.gameObject.SetActive(false);
        _rankingsText.gameObject.SetActive(true);
        UIFinishedButtons(true, false);
        //EventManager.Trigger("OnChangedRanking");
        RankingsPopUp();
    }

    public void RankingsPopUp()
    {
        var player = FindObjectOfType<CharacterModel>();
        var playersLenght = 6;
        var getText = _rankingResults.CheckValues(player, playersLenght);
        foreach (var item in getText)
        {
            _rankingsText.text += "\n"+ item;
            Debug.Log(item);
        }
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
