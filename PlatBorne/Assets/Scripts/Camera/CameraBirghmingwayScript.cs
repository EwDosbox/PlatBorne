using System.Drawing;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraBirghmingwayScript : MonoBehaviour
{
    [SerializeField] private GameObject hunter;
    [SerializeField] private Camera cam;
    private Vector2 hP;
    private Vector2 cP;

    private void Update()
    {
        hP = hunter.transform.position;
        cP = hP;
        if (IsOutside())
        {
            cam.orthographicSize = 3.8f;
            //X: -32  -32  
            //Y: -4     2
            cP.x = -32;
            if (cP.y < -4f) cP.y = -4f;
            else if (cP.y > 2f) cP.y = 2f;
        }
        else if (IsInMainNave())
        {
            cam.orthographicSize = 5;
            if (cP.x < -17f) cP.x = -17f;
            else if (cP.x > 1f) cP.x = 1f;
            if (cP.y > 5f) cP.y = 5f;
            else if (cP.y < -3f) cP.y = -3f;
        }
        else if (IsInBellTower())
        {
            cam.orthographicSize = 5;
            //X: -1  1 
            //Y: 14  62
            if (cP.x < -1f) cP.x = -1f;
            else if (cP.x > 1f) cP.x = 1f;
            if (cP.y > 62f) cP.y = 62f;
            else if (cP.y < 14f) cP.y = 14f;
        }
        else
        {
            transform.position = new Vector3(hP.x, hP.y, -25);

        }
        cam.transform.position = new Vector3(cP.x, cP.y, -25);
    }
    private bool IsOutside()
    {
        return hP.x < -25;
    }
    private bool IsInMainNave()
    {
        return hP.x > -25 && hP.y < 9;
    }
    private bool IsInBellTower()
    {
        return hP.x > -10;
    }
}