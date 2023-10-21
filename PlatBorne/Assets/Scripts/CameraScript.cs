using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    public Vector2 offset = Vector2.zero;
    public CompositeCollider2D mapCollider;
    void Update()
    {
        if (player.transform.position.y < -60)
        {
            this.transform.position = new Vector3(
                0,
                -60  
                );         
        }
        else if (player.transform.position.y > 0)
        {
            this.transform.position = new Vector3(
                0,
                0
                );
        }
        else 
        {
            this.transform.position = new Vector3(
                this.transform.position.x, 
                player.transform.position.y
           );
        }
    }
}
