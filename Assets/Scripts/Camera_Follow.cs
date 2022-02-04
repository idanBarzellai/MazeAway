using UnityEngine;
using System.Collections;

public class Camera_Follow : MonoBehaviour
{
    // Control the camera, Following the player
    public GameObject player;
    private Vector3 offset;
    private Vector3 newtrans;

    void Start()
    {
        offset.x = transform.position.x - player.transform.position.x;
        offset.y = transform.position.y - player.transform.position.y;
        newtrans = transform.position;
    }
    void LateUpdate()
    {
        newtrans.x = player.transform.position.x + offset.x;
        newtrans.y = player.transform.position.y + offset.y;
        transform.position = newtrans;
    }

}