using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAIScript : MonoBehaviour
{
    private Rigidbody2D fishRB;
    private Collider2D fishCollider;
    private Collider2D waterBounds;

    private float nextFishMovementChange;
    private bool isTimeToChangeMovement;

    private void Awake()
    {
        fishRB = GetComponent<Rigidbody2D>();
        fishCollider = GetComponent<Collider2D>();
        waterBounds = GameObject.Find("Water").GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        isTimeToChangeMovement = Time.time > nextFishMovementChange;
        if (isTimeToChangeMovement)
        {
            nextFishMovementChange = RandomTime();
            if (!Physics2D.IsTouching(fishCollider, waterBounds))
            {
                
            }
            else
            {
                if (fishRB.velocity.x > 0)
                {
                    if (Random.value > 0.3) fishRB.velocity += RandomPositoveVector();
                    else fishRB.velocity += RandomNegativeVector();
                }
                else
                {
                    if (Random.value > 0.3) fishRB.velocity += RandomNegativeVector();
                    else fishRB.velocity += RandomPositoveVector();
                }
            }
        }
    }

    private Vector2 RandomPositoveVector()
    {
        float x = 3;
        return new Vector2(+Equation(Random.Range(0, x)), 0);
    }
    private Vector2 RandomNegativeVector()
    {
        float x = 3;
        return new Vector2(-Equation(Random.Range(0, x)), 0);
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
