using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_AttackGroundFuckingSomething : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float timeToSelfDestruct;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxHeight;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float timeStayedInAir;

    private bool goDown = false;
    private float timeSpinning = 0;
    private float timer = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0, rotationSpeed);
        timer += Time.deltaTime;
        if (timer > timeToSelfDestruct)
        {
            Destroy(gameObject);
        }
        rb = GetComponent<Rigidbody2D>();
        if (goDown)
        {
            rb.velocity += Vector2.down * acceleration;
        }
        else rb.velocity += Vector2.up * acceleration;
        if (rb.position.y > maxHeight)
        {
            rb.velocity = Vector2.zero;
            timeSpinning += Time.deltaTime;
            if (timeSpinning > timeStayedInAir)
            {
                rb.velocity += Vector2.down * acceleration;
            }
        }
    }
}

