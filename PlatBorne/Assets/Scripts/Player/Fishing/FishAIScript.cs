using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAIScript : MonoBehaviour
{
    [Header("Fish AI Settings")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float forceMultiplier = 150f;
    [SerializeField] private float movementIntervalMin = 0.5f;
    [SerializeField] private float movementIntervalMax = 1.5f;

    public (Collider2D left, Collider2D right) waterColliders;
    private SpriteRenderer sprite;
    private Rigidbody2D fishRB;
    private float nextFishMovementChange;
    private BetterRandom random = new BetterRandom($"FishAIScript at {System.DateTime.Now}");

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        fishRB = GetComponent<Rigidbody2D>();
        waterColliders = (
            GameObject.Find("WaterLeftSide").GetComponent<Collider2D>(),
            GameObject.Find("WaterRightSide").GetComponent<Collider2D>()
        );
    }

    private void FixedUpdate()
    {
        float currentTime = Time.time;

        if (currentTime > nextFishMovementChange)
        {
            nextFishMovementChange = currentTime + RandomTime();

            Vector2 force = RandomVector();
            bool addForce = random.Random(1f) > 0.4f;

            fishRB.AddForce((addForce ? force : -force) * forceMultiplier, ForceMode2D.Impulse);
        }

        fishRB.velocity = Vector2.ClampMagnitude(fishRB.velocity, maxSpeed);
        sprite.flipX = !(fishRB.velocity.x < 0);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider == waterColliders.left)
        {
            fishRB.velocity = new Vector2(1f, 0);
            nextFishMovementChange = Time.time + random.Range(0.2f, 0.5f);
        }
        else if (collider == waterColliders.right)
        {
            fishRB.velocity = new Vector2(-1f, 0);
            nextFishMovementChange = Time.time + random.Range(0.2f, 0.5f);
        }
    }

    private Vector2 RandomVector()
    {
        return new Vector2(random.Range(0.5f, 1.5f), random.Range(-0.3f, 0.3f));
    }

    private float RandomTime()
    {
        return random.Range(movementIntervalMin, movementIntervalMax);
    }
}
