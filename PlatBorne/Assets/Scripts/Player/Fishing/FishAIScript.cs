using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishAIScript : MonoBehaviour
{
    private Rigidbody2D fishRB;
    private Collider fishLCollider;
    private Collider fishRCollider;
    private Collider waterLCollider;
    private Collider waterRCollider;

    private float nextFishMovementChange;
    private bool isTimeToChangeMovement;

    private void Awake()
    {
        fishRB = GetComponent<Rigidbody2D>();
        Collider[] colliders = FindObjectsOfType<Collider>();
        fishRCollider = colliders.FirstOrDefault(x => x.name == "RightSide");
        waterRCollider = colliders.FirstOrDefault(x => x.name == "WaterRightSide");
        waterLCollider = colliders.FirstOrDefault(x => x.name == "WaterLeftSide");
        fishLCollider = colliders.FirstOrDefault(x => x.name == "LeftSide");
        Debug.Log(colliders.ToString());
    }

    private void FixedUpdate()
    {
        isTimeToChangeMovement = Time.time > nextFishMovementChange;
        if (isTimeToChangeMovement)
        {
            nextFishMovementChange = RandomTime();
            // if (Physics2D.IsTouching(fishLCollider, waterLCollider))
            {
                fishRB.velocity += RandomVector();
            }
            //else if (Physics2D.IsTouching(fishRCollider, waterRCollider))
            {
                fishRB.velocity -= RandomVector();
            }
            //else
            {
                if (fishRB.velocity.x > 0)
                {
                    if (Random.value > 0.3) fishRB.velocity += RandomVector();
                    else fishRB.velocity -= RandomVector();
                }
                else
                {
                    if (Random.value > 0.3) fishRB.velocity -= RandomVector();
                    else fishRB.velocity += RandomVector();
                }
            }
        }
    }

    private Vector2 RandomVector()
    {
        float x = 3;
        return new Vector2(+Equation(Random.Range(0, x)), 0);
    }
    private float Equation(double x)
    {
        return 3 + (0.5f - 3) / Mathf.Pow((1f + (float)x / 2.6f), 6);
    }

    private float RandomTime()
    {
        float x = 3;
        return Time.time + Random.Range(0, x);
    }
}
