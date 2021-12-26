using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinSystem : MonoBehaviour
{
    public int coin;

    public Text coinText;

    private void Start()
    {
        coin = PlayerPrefs.GetInt("coin");
        coinText = GameObject.FindWithTag("CoinText").GetComponent<Text>();
    }

    private void LateUpdate()
    {
        coinText.text = coin.ToString();
    }

    public void AddCoin(int coinAmount)
    {
        coin += coinAmount;
        PlayerPrefs.SetInt("coin", coin);
    }
}
