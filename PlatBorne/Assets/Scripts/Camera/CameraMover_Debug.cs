using System.Collections;
using UnityEngine;

public class CameraMover_Debug : MonoBehaviour
{
    public Vector3 startPosition; // Starting position of the camera
    public Vector3 endPosition;   // End position of the camera
    public float waitTime = 5f;   // Time to wait before moving the camera (in seconds)
    public float moveDuration = 60f; // Time it takes to move the camera (in seconds)

    private float elapsedTime = 0f; // Tracks the elapsed time of the movement
    private bool isMoving = false; // Tracks whether the camera is currently moving

    void Start()
    {
        // Set the initial position of the camera
        transform.position = startPosition;
        // Start the movement coroutine
        StartCoroutine(StartCameraMovement());
    }

    private IEnumerator StartCameraMovement()
    {
        yield return new WaitForSeconds(waitTime);
        isMoving = true;
    }

    void Update()
    {
        if (isMoving)
        {
            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the interpolation factor (0 to 1)
            float t = Mathf.Clamp01(elapsedTime / moveDuration);

            // Smoothly move the camera from the start position to the end position
            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            // Stop moving when the duration is reached
            if (t >= 1f)
            {
                isMoving = false;
            }
        }
    }
}
