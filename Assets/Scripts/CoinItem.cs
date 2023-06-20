using UnityEngine;

public class CoinItem : Item
{
    public override void ItemPower()
    {
        base.ItemPower();
        int coins = PlayerPrefs.GetInt("Coins");
        coins++;
        PlayerPrefs.SetInt("Coins", coins);
        int rand = Random.Range(0, 4);
        SendMessageToCanvas(rand == 0 ? "I AM RICH!" : rand == 1 ? "MONEY MONEY!" : rand == 2 ? "BLING BLING!" : "MAKE IT RAIN!");
    }
}
