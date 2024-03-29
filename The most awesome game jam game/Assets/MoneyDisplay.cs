﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyDisplay : MonoBehaviour
{
    public static MoneyDisplay instance;

    public Text moneyText;
    public Text loseText;
    public TextMeshProUGUI winText;

    public RectTransform winScreen;

    [FMODUnity.EventRef]
    public string moneyDropEvent;

    private int moneyTarget;
    private int currentMoney;

    private FMOD.Studio.EventInstance moneyDropSoundInstance;

    private bool rolling;

    void Awake()
    {
        currentMoney = 0;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        moneyDropSoundInstance = FMODUnity.RuntimeManager.CreateInstance(moneyDropEvent);
    }

    public void SoftSetMoney(int money)
    {
        moneyTarget = money;

        moneyDropSoundInstance.setParameterValue("coins", 0.0f);
        moneyDropSoundInstance.start();
    }

    public void SetMoney(int money)
    {
        currentMoney = money;
        moneyTarget = money;
        SetMoneyText(money);
    }

    void SetMoneyText(int money)
    {
        moneyText.text = "" + money;
    }

    public void SetLoseTextEnabled(bool enabled)
    {
        loseText.enabled = enabled;
    }

    public void SetWinTextEnabled(bool enabled)
    {
        winText.enabled = enabled;
    }

    public void Win()
    {
        winScreen.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (moneyTarget <= 0) {
            moneyDropSoundInstance.setParameterValue("coins", 1.0f);
            SetMoney(0);
        } else if (currentMoney > moneyTarget) {
            if (currentMoney - moneyTarget > 2000) {
                currentMoney -= 500;
            } else if (currentMoney - moneyTarget > 50) {
                currentMoney -= 50;
            } else {
                currentMoney--;

                if (currentMoney <= moneyTarget)
                {
                    moneyDropSoundInstance.setParameterValue("coins", 1.0f);
                }
            }
            SetMoneyText(currentMoney);
        } else if (currentMoney < moneyTarget) {
            if (moneyTarget - currentMoney > 2000) {
                currentMoney += 500;
            } else if (moneyTarget - currentMoney > 50) {
                currentMoney += 50;
            } else {
                currentMoney++;

                if (currentMoney >= moneyTarget)
                {
                    moneyDropSoundInstance.setParameterValue("coins", 1.0f);
                }
            }
            SetMoneyText(currentMoney);
        }
    }
}
