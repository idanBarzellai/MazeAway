public class ClockItem : Item
{
    public override void ItemPower()
    {
        base.ItemPower();
        gm.SetCurrTime(15f);
        SendMessageToCanvas("15 SECS MORE!");
    }
}
