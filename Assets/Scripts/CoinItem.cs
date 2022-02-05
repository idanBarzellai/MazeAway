using UnityEngine;

public class CoinItem : Item
{
    public override void ItemPower()
    {
        base.ItemPower();
        int coins = PlayerPrefs.GetInt("Coins");
        coins++;
        PlayerPrefs.SetInt("Coins", coins);
        SendMessageToCanvas("I AM RICH!");
    }
}
