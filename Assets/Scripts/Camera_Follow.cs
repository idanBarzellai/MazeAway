using UnityEngine;
using System.Collections;
using System;

public class Camera_Follow : MonoBehaviour
{
    // Control the camera, Following the player
    public GameObject player;
    private PlayerController playerCon;
    private Vector3 offset;
    private Vector3 newPos;
    private float followSpeed = 3.1f;
    private bool waited = false;


    void Start()
    {
        offset.x = transform.position.x - player.transform.position.x;
        offset.y = transform.position.y - player.transform.position.y;
        playerCon = player.GetComponent<PlayerController>();
        waited = false;
        
    }
    void FixedUpdate()
    {
        if (!waited && !playerCon.GetFreezeMovement())
        {
            transform.position = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, -10f);

            StartCoroutine(WaitXSecondsThenStartFollow(0.2f));
        }
        if (waited)
        {
            newPos = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, -10f);
            transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.fixedDeltaTime);

        }
    }

    IEnumerator WaitXSecondsThenStartFollow(float x)
    {
        yield return new WaitForSecondsRealtime(x);
        waited = true;
    }

    public void SetWaited(bool set)
    {
        waited = set;
    }
}