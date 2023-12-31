using UnityEngine;

public class BossDaggerLogic : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject player;
    [SerializeField] private float timeToSelfDestruct;
    [SerializeField] private float speed;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb.position.x >= 0)
        {
            rb.velocity = Vector2.left * speed;
            transform.rotation = Quaternion.Euler(Vector3.forward * 270);
        }
        else
        {
            rb.velocity = Vector2.right * speed;
            transform.rotation = Quaternion.Euler(Vector3.forward * 90);
        }

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToSelfDestruct)
        {
            Destroy(gameObject);
        }
    }
}
