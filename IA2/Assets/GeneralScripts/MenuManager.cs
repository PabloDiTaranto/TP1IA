using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject[] menuItems;
    [SerializeField] private GameObject[] creditsItems;
    
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
        MySceneManager.ChangeScene("SelectTime");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
