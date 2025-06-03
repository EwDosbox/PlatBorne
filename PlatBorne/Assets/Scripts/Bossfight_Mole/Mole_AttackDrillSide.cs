using UnityEngine;

public class Mole_AttackDrillSide : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] AudioSource sfx;
    [SerializeField] float maxDistance = 8f; // max distance at which sound is audible
    GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth ph = FindAnyObjectByType<PlayerHealth>();
            ph.PlayerDamage();
            Destroy(gameObject);
        }
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<PlayerHealth>().gameObject;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rb.velocity = Vector2.up * speed;
    }

    void Update()
    {
        if (Mathf.Abs(transform.position.x) > 18f) Destroy(gameObject);
        //change volume        
        float distance = Vector2.Distance(transform.position, player.transform.position);
        float volume = Mathf.Clamp01(1f - (distance / maxDistance)) * 0.1f;
        sfx.volume = volume;
    }
}
