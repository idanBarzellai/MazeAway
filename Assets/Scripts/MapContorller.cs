using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapContorller : MonoBehaviour
{
    
    public GameObject[] blockIcons;
    public PlayerController player;
    public GameObject dino; 
    private void Start()
    {
        foreach (GameObject blockIcon in blockIcons)
        {
            blockIcon.SetActive(false);
        }
    }
    void FixedUpdate()
    {
        float posX = player.transform.position.x;
        float posY = player.transform.position.y;

        DetermenBlcok(posX, posY);
    }

    public void resetIcons()
    {
        foreach (GameObject blockIcon in blockIcons)
        {
            blockIcon.SetActive(false);
        }
    }

    private void DetermenBlcok(float posX, float posY)
    {
        int y = -1;
        if (posY > 3.6 && posY < 4.4)
            y = 0;
        else if (posY > 1.6 && posY < 2.4)
            y = 1;
        else if (posY > -0.4 && posY < 0.4)
            y = 2;
        else if (posY > -2.4 && posY < -1.6)
            y = 3;
        else if (posY > -4.4 && posY < -3.6)
            y = 4;
        else if (posY > -6.4 && posY < -5.6)
            y = 5;


        int x = -1;
        if (posX > -5.4 && posX < -4.6)
            x = 0;
        else if (posX > -3.4 && posX < -2.6)
            x = 1;
        else if (posX > -1.4 && posX < -0.6)
            x = 2;
        else if (posX > 0.6 && posX < 1.4)
            x = 3;
        else if (posX > 2.6 && posX < 3.4)
            x = 4;
        else if (posX > 4.6 && posX < 5.4)
            x = 5;
        if (x >= 0 && y >= 0)
        {
            int index = (y * 6) + x;
            blockIcons[index].SetActive(true);
            Vector3 blockPos = blockIcons[index].transform.position;
            dino.transform.position = blockPos;
        }
    }
}
