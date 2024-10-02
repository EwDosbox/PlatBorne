using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScript1 : MonoBehaviour
{
    private Camera cam;
    private Rigidbody2D rb;

    [SerializeField] public List<Vector2> cameraCorners;
    private Vector2 camPosition;
    private int camIndex;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        camIndex = 0;
    }

    private void FixedUpdate()
    {
        if(camPosition.x > cameraCorners[camIndex].x)
        {

        }

    }
}
