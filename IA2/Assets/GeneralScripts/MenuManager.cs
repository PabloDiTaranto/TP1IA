using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject[] menuItems;
    [SerializeField] private GameObject[] creditsItems;
    [SerializeField] private TMP_InputField _inputField;


    public void CreditButton()
    {
        foreach (var item in menuItems)
        {
            item.SetActive(false);
        }
        foreach (var item in creditsItems)
        {
            item.SetActive(true);
        }
    }

    public void GoBackToMenuButton()
    {
        foreach (var item in menuItems)
        {
            item.SetActive(true);
        }
        foreach (var item in creditsItems)
        {
            item.SetActive(false);
        }
    }

    public void PlayButton()
    {
        if (_inputField.text == "")
            _inputField.text = "Guest";

        PlayerNameManager._name = _inputField.text;
        MySceneManager.ChangeScene("SelectTime");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
