using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableCoinUI : MonoBehaviour
{
    private Text coinText;
    string fmt = "000";
    private void Awake()
    {
        coinText = GetComponentInChildren<Text>();
    }

    public void Refresh()
    {
        if(coinText != null)
        {
            coinText.text = $"x{CoinString(CoinCollectorManager.CollectedCoins)}";
        }
    }

    //yeah its cringe, do something about it then.
    private string CoinString(int coins)
    {
        if (coins > 99)
        {
            return coins.ToString();
        }
        else if (coins > 9)
        {
            return "0" + coins.ToString();
        }
        else if (coins > 0)
        {
            return "00" + coins.ToString();
        }
        else
        {
            return "000";
        }
    }
}