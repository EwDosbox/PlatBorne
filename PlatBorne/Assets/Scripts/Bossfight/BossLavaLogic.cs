using UnityEngine;

public class BossLavaLogic : MonoBehaviour
{
    private Rigidbody2D rb;
    private float timer;
    private bool goUp;
    [SerializeField] float acceleration;
    [SerializeField] float waitingTimeToGoDown;
    private bool maxUp;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxUp = false;
        goUp = true;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (maxUp && goUp)
        {
            timer += Time.deltaTime;
            if (timer >= waitingTimeToGoDown) goUp = false;
        }
        else if (goUp)
        {
            if (rb.position.y <= -11.4f)
            {
                rb.velocity += Vector2.up * acceleration * Time.deltaTime;
            }
            else
            {
                rb.velocity = Vector2.up * 0;
                maxUp = true;
            }
        }
        else
        {
            rb.velocity = Vector2.down * acceleration * 10;
            Bossfight.attackIsGoingOn = false; //End of Attack
        }
        if (rb.position.y < -18f) Destroy(gameObject);
    }
}
