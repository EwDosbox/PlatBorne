using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDaggerLogic : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject player;
    bool goLeft = false;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (rb.position.x >= 0)
        {
            rb.velocity = new Vector2(-25.7f, rb.position.y) * speed;
        }
        else
        {
            rb.velocity = new Vector2(14f, rb.position.y) * speed;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
