using UnityEngine;

public class Mole_AttackDrillSide : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject player;
    [SerializeField] private float timeToSelfDestruct;
    [SerializeField] private float speed;
    private float timer = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth ph = FindAnyObjectByType<PlayerHealth>();
            ph.PlayerDamage();
            Destroy(gameObject);
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb.position.x >= 0)
        {
            rb.velocity = Vector2.left * speed;
            transform.rotation = Quaternion.Euler(0,0, 270);
        }
        else
        {
            rb.velocity = Vector2.right * speed;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }

    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToSelfDestruct)
        {
            Destroy(gameObject);
        }
    }
}
