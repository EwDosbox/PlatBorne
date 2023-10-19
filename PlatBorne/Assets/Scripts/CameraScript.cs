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
        bool wasCamInBounds = true;
        if (math.abs(player.transform.position.x) <= offset.x / 2)
        {
            wasCamInBounds = false;
            if (player.transform.position.x > 0)
                transform.position = new Vector3(player.transform.position.x - offset.x / 2,
                    this.transform.position.y);
            else transform.position = new Vector3(player.transform.position.x + offset.x / 2,
                this.transform.position.y);
        }
        if (math.abs(player.transform.position.y) >= offset.y / 2)
        {
            wasCamInBounds = false;
            if (player.transform.position.y > 0)
                transform.position = new Vector3(this.transform.position.x,
                    player.transform.position.y - offset.y / 2);
            else transform.position = new Vector3(this.transform.position.x,
                player.transform.position.y + offset.y / 2);
        }
        if (wasCamInBounds) 
        {
            this.transform.position = new Vector3(
                player.transform.position.x,
                player.transform.position.y
           );
        }
    }
}
