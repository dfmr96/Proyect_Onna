using System.IO;
using UnityEngine;

public class PlayerWallet
{
    public int Coins { private set; get; }
    public PlayerWallet() { LoadCoins(); }

    private void LoadCoins() { Coins = SaveSystem.LoadData().totalCoins; }
    private void SaveCoins() { SaveSystem.SaveCoins(Coins); }

    public void AddCoins(int amount)
    {
        Coins += amount;
        SaveCoins();
        Debug.Log($"Se sumaron {amount} monedas. Total: {Coins}");
    }

    public bool TrySpend(int amount)
    {
        if (Coins >= amount)
        {
            Coins -= amount;
            SaveCoins();
            Debug.Log($"Se gastaron {amount} monedas. Total restante: {Coins}");
            return true;
        }
        else
        {
            Debug.Log("No hay suficientes monedas para gastar.");
            return false;
        }
    }
}
