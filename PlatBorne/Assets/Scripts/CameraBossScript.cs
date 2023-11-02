using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraBossScript : MonoBehaviour
{
    public GameObject player;
    public Camera cam;
    private Vector3 position;
    void Update()
    {
        position = player.transform.position;

        if(position.x < -18f)// chodba
        {
            cam.orthographicSize = 4.9f;
            position = new Vector2(-23.2f, -9);
        }
        else// arena
        {
            cam.orthographicSize = 10;
            position = new Vector2(0, 0);
        }

        this.transform.position = position;
    }
}
