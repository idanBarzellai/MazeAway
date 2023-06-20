using UnityEngine;

public class MagnifyingGlass : Item
{
    public PlayerController player;

    protected override void Start()
    {
        base.Start();
        player = FindObjectOfType<PlayerController>();
    }
    public override void ItemPower()
    {
        base.ItemPower();
        gm.MagnifyingGLass(view * 1.5f);
        player.SetSpeed(0.8f);
        SendMessageToCanvas("I CAN SEE!");
    }
}
