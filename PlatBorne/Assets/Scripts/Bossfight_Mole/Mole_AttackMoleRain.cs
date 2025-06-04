using UnityEngine;

public class Mole_AttackMoleRain : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float timeToSelfDestruct;
    [SerializeField] private float maxSpeed;

    private float timer = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.down * maxSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth ph = FindAnyObjectByType<PlayerHealth>();
            ph.PlayerDamage();
            Destroy(gameObject);
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

