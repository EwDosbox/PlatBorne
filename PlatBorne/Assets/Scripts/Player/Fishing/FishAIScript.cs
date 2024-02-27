using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishAIScript : MonoBehaviour
{
    private Rigidbody2D fishRB;
    public Collider2D fishLCollider;
    public Collider2D fishRCollider;
    public Collider2D waterLCollider;
    public Collider2D waterRCollider;

    private float nextFishMovementChange;
    private bool isTimeToChangeMovement;

    private void Awake()
    {
        fishRB = GetComponent<Rigidbody2D>();
        Collider2D[] colliders = FindObjectsOfType<Collider2D>();
        fishLCollider = colliders.FirstOrDefault(x => x.name == "LeftSide");
        fishRCollider = colliders.FirstOrDefault(x => x.name == "RightSide");
        waterRCollider = colliders.FirstOrDefault(x => x.name == "WaterRightSide");
        waterLCollider = colliders.FirstOrDefault(x => x.name == "WaterLeftSide");
    }

    private void FixedUpdate()
    {
        isTimeToChangeMovement = Time.time > nextFishMovementChange;
        if (isTimeToChangeMovement)
        {
            nextFishMovementChange = RandomTime();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider == waterLCollider)
        {
            nextFishMovementChange = RandomTime();
            fishRB.velocity += RandomVector();
        }
        else if (collision.otherCollider == waterRCollider)
        {
            nextFishMovementChange = RandomTime();
            fishRB.velocity -= RandomVector();
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
