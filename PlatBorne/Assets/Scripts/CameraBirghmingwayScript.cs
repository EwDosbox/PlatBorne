using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CameraBirghmingwayScript : MonoBehaviour
{
    [SerializeField] private GameObject hunter;
    private Vector2 hT;
    private void Update()
    {
        hT = hunter.transform.position;
        Debug.Log(HunterBetween(new Vector2(10, 10), new Vector2(-10, -10)));
        SetToHunter();
    }

    private bool HunterBetween(Vector2 v1, Vector2 v2)
    {
        bool hBetween1XA2X = (v1.x > hT.x && hT.x < v2.x);
        bool hBetween2XA1X = (v2.x > hT.x && hT.x < v1.x);
        bool hBetweenX = hBetween1XA2X || hBetween2XA1X;
        bool hBetween1YA2Y = (v1.y > hT.y && hT.y < v2.y);
        bool hBetween2YA1Y = (v2.y > hT.y && hT.y < v1.y);
        bool hBetweenY = hBetween1YA2Y || hBetween2YA1Y;
        return hBetweenX && hBetweenY;
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
