using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class CoinHuntGameMode : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinsCollected;

    [SerializeField]  Coin[] coins;

    List<Coin> coinList = new List<Coin>();

    int initialCoinAmount;

    ManagementSystem managementSystem;

    private void Start()
    {
        managementSystem = FindObjectOfType<ManagementSystem>();
        coins = FindObjectsOfType<Coin>();

        coinList = coins.ToList();
        initialCoinAmount = coins.Length;

        CheckCoins();
    }
    public void RemoveCoin(Coin coin)
    {
        coinList.Remove(coin);
        CheckCoins();
    }

    private void CheckCoins()
    {
        coinsCollected.text = (coinList.Count) + "/" + initialCoinAmount;

        if (coinList.Count == 0)
        {
            managementSystem.WinGame();
        }
    }
}
