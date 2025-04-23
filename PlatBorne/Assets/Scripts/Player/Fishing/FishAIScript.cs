using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishAIScript : MonoBehaviour
{
    [Header("Fish AI Settings")]
    [SerializeField] private float maxSpeed = 4f;

    public (Collider2D, Collider2D) fishColliders;
    public (Collider2D, Collider2D) waterColliders;

    private SpriteRenderer sprite;
    private Rigidbody2D fishRB;
    private float nextFishMovementChange;
    private bool isTimeToChangeMovement;
    private BetterRandom random = new BetterRandom("FishAIScript");

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        fishRB = GetComponent<Rigidbody2D>();
        Collider2D[] colliders = FindObjectsOfType<Collider2D>();
        fishColliders = (colliders.FirstOrDefault(x => x.name == "FishLeftSide"), colliders.FirstOrDefault(x => x.name == "FishRightSide"));
        waterColliders = (colliders.FirstOrDefault(x => x.name == "WaterLeftSide"), colliders.FirstOrDefault(x => x.name == "WaterRightSide"));
    }

    private void FixedUpdate()
    {
        isTimeToChangeMovement = Time.time > nextFishMovementChange;
        if (!isTimeToChangeMovement) return;

        nextFishMovementChange = RandomTime();
        bool movingRight = fishRB.velocity.x > 0;
        bool shouldAdd = random.Random(1f) > 0.3f;

        Vector2 newVelocity = shouldAdd ? RandomVector() : -RandomVector();
        fishRB.velocity += movingRight ? newVelocity : -newVelocity;
        sprite.flipX = movingRight ? shouldAdd : !shouldAdd;

        fishRB.velocity += new Vector2(newVelocity.x, 0);

        fishRB.velocity = new Vector2(Mathf.Clamp(fishRB.velocity.x, -maxSpeed, maxSpeed), fishRB.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider == waterColliders.Item1)
        {
            nextFishMovementChange = RandomTime();
            fishRB.velocity = -RandomVector();
        }
        else if (collision.otherCollider == waterColliders.Item2)
        {
            nextFishMovementChange = RandomTime();
            fishRB.velocity = -RandomVector();
        }
    }

    private Vector2 RandomVector()
    {
        return new Vector2(+Equation(random.Range(0, 3)), 0);
    }
    private float Equation(double x)
    {
        return 3 + (0.5f - 3) / Mathf.Pow((1f + (float)x / 2.6f), 6);
    }

    private float RandomTime()
    {
        return Time.time + random.Range(0.5f, 1.5f);
    }
}
