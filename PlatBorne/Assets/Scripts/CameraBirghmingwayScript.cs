using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CameraBirghmingwayScript : MonoBehaviour
{
    [SerializeField] private GameObject hunter;
    [SerializeField] private Camera camera;
    private Vector2 hT;
    private void Update()
    {
        hT = hunter.transform.position;
        if (hT.x < -26)
        {
            SetTo(new Vector2(-32, -3));
            camera.orthographicSize = 4;
        }
        else if
            (
                hT.y > 5 &&
                hT.x < -9
            )
        {
            SetTo(new Vector2(hT.x, 5));
        }
        else if
            (
                hT.y > 9 &&
                hT.x > -9
            )
        {
            SetTo(new Vector2(-1, hT.y));
        }
        else if
            (
                hT.x > -25 &&
                hT.y < -3
            )
        {
            SetTo(new Vector2(hT.x, -3));
        }
        else
        {
            SetToHunter();
            camera.orthographicSize = 5;
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
