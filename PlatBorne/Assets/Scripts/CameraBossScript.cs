using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBossScript : MonoBehaviour
{
    public GameObject player;
    void Update()
    {
        this.transform.position = new Vector2(  player.transform.position.x,
                                                player.transform.position.y);
    }
}
