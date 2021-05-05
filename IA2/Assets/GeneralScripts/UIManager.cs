using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI time, ammo;
    [SerializeField] private GameObject uiCanvas;

    private void Awake()
    {
        EventManager.Subscribe("OnTimeMod", SetTime);
        EventManager.Subscribe("OnPlayerLifeChange", SetHealtBarAmmount);
        EventManager.Subscribe("OnAmmoChange", SetAmmo);
        EventManager.Subscribe("OnGameOver", HideUI);
    }

    private void SetHealtBarAmmount(params object[] parameters)
    {
        healthBar.fillAmount = (float)parameters[0]/(float)parameters[1];
    }

    private void SetTime(params object[] parameters)
    {
        time.text = Mathf.Round((float)parameters[0]).ToString();
    }

    private void SetAmmo(params object[] parameters)
    {
        ammo.text = "x " + ((int)parameters[0]).ToString();
    }

    private void HideUI(params object[] parameters)
    {
        uiCanvas.SetActive(false);
    }
}
