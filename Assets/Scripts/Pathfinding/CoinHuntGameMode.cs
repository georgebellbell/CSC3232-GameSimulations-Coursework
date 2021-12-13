using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

// Gamemode implemented with pathfinding where player must collect all coins while avoiding a chasing enemy
public class CoinHuntGameMode : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinsCollected;
    [SerializeField] Coin[] coins;

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

    // Called by Rover's CoinState when it collides with a coin, then checks if their are any coins left
    public void RemoveCoin(Coin coin)
    {
        coinList.Remove(coin);
        CheckCoins();
    }

    // Checks how many coins are left, updates display and if their are none left, player will win
    private void CheckCoins()
    {
        coinsCollected.text = "Coins Left: " +(initialCoinAmount - coinList.Count) + "/" + initialCoinAmount;

        if (coinList.Count == 0)
        {
            managementSystem.WinGame();
        }
    }
}
