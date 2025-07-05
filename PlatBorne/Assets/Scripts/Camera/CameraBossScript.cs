using UnityEngine;

public class CameraBossScript : MonoBehaviour
{
    public GameObject player;
    public Camera cam;
    bool once = false;
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
            if (!once)
            {
                once = true;
                Bossfight boss = FindAnyObjectByType<Bossfight>();
                boss.StartBossfight();
            }
        }
        this.transform.position = position;
    }
}
