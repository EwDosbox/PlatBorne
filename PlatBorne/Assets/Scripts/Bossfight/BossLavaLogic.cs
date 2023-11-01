using UnityEngine;

public class BossLavaLogic : MonoBehaviour
{
    private Rigidbody2D rb;
    private float timer = 0;
    private bool goUp = true;
    [SerializeField] float speed = 0.5f;
    [SerializeField] float maxSpeed = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 10) goUp = false;
        if (goUp)
        {
            if (rb.position.y <= -7.5f)
            {
                maxSpeed += speed * Time.deltaTime;
                if (maxSpeed > 2) maxSpeed = 2;
                rb.velocity = Vector2.up * maxSpeed;              
            }
            else rb.velocity = Vector2.up * 0;
        }
        else rb.velocity = Vector2.down * speed * 2;
        if (rb.position.y < -18f) Destroy(gameObject);
    }
}
