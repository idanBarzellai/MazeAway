using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockContorller : MonoBehaviour
{
    public Bridge[] bridges;
    public BlockContorller[] neighbors;

    private void Awake()
    {
        for (int i = 0; i < bridges.Length; i++)
        {
            bridges[i].AddBlock(this);
        }
    }
}
