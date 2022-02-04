public class MagnifyingGlass : Item
{   
    public override void ItemPower()
    {
        base.ItemPower();
        gm.MagnifyingGLass(view * 1.5f);
        SendMessageToCanvas("WE CAN SEE!");
    }
}
