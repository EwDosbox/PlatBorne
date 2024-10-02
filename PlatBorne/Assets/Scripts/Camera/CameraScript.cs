using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    void Update()
    {
        if (player.transform.position.y < -60)
        {
            this.transform.position = new Vector3(
                0,
                -60,
                -10
                );
        }
        else if (player.transform.position.y > 0)
        {
            this.transform.position = new Vector3(
                0,
                0,
               -10
                );
        }
        else
        {
            this.transform.position = new Vector3(
                this.transform.position.x,
                player.transform.position.y,
                -10
           );
        }
    }
}