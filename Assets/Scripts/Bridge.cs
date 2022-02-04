using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public GameObject obstacle;
    private bool active = true;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private HashSet<BlockContorller> vertexes = new HashSet<BlockContorller>();

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SwitchActive()
    {
        // Sets the active variable and deactivate/activate sprite and collider
        active = !active;
        boxCollider.enabled = !active;
        obstacle.SetActive(!active);
    }

    public bool GetActive()
    {
        return active;
    }

    public void AddBlock(BlockContorller b)
    {
        vertexes.Add(b);
    }

    public BlockContorller GetTheOtherBlock(BlockContorller me)
    {
        foreach (BlockContorller b in vertexes)
        {
            if (!b.Equals(me))
                return b;
        }
        return null;
    }
}
