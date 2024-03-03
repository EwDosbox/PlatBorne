using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CameraBirghmingwayScript : MonoBehaviour
{
    [SerializeField] private GameObject hunter;
    private Collider2D hunterHeart;

    public enum Camera
    {
        Enter,
        Exit
    }
       

    private void Awake()
    {
        hunterHeart = hunter.GetComponents<Collider2D>().FirstOrDefault(collider2D => collider2D.name.Equals("VoiceLineCollider"));
    }
    private void Update()
    {
        Debug.Log(hunterHeart.name);
        SetToHunter();
    }

    public static void Cam(Collision2D collision, Camera camera)
    {
        if (camera.Equals(Camera.Enter))
        {

        }
        else if (camera.Equals(Camera.Exit))
        {

        }
    }

    private void SetToHunter()
    {
        this.transform.position = new Vector3(
            hunter.transform.position.x,
            hunter.transform.position.y,
            -25
       );
    }
    private void SetTo(Vector2 position)
    {

        this.transform.position = new Vector3(
            position.x,
            position.y,
            -25
       );
    }
}
