using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //This class causes the camera to follow the player around the screen.
    public GameObject player;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // This lateUpdate assures that the camera moves after the player has been moved
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
