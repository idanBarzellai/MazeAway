public class KeyItem : Item
{
    public override void ItemPower()
    {
        base.ItemPower();
        gm.player.GotKey(true);
        gm.canvas.GotKey(false);
        SendMessageToCanvas("I GOT IT!");
    }
}
