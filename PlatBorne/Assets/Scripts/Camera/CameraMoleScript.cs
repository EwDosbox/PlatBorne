using UnityEngine;

public class CameraMoleScript : MonoBehaviour
{
    public Camera cam;
    public PlayerScript playerScript;
    private Vector3 position;

    private void Start()
    {
        playerScript = FindFirstObjectByType<PlayerScript>();
        cam = GetComponent<Camera>();
    }

    public void ChangeCamPosition()
    {
        playerScript.MovePlayer(2f,0f);
        cam.orthographicSize = 10;
        transform.position = new Vector3(0, 0, -10);
        Mole_Bossfight fight = FindFirstObjectByType<Mole_Bossfight>();
        StartCoroutine(fight.StartBossFight());
    }
}
