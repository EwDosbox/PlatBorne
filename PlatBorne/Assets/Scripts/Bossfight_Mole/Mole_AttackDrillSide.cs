using Unity.VisualScripting;
using UnityEngine;

public class Mole_AttackDrillSide : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] AudioSource sfx;
    [SerializeField] float maxDistance = 15f; // max distance at which sound is audible
    float timer = 0f;
    float invincTimer = 2f; //so it doesnt destroy right after spawning
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
        player = FindAnyObjectByType<PlayerScript>().gameObject;
        if (player == null) Debug.LogError("Could not find PlayerHealth");
        if (transform.position.x > 0)
        {
            rb.velocity = Vector2.left * speed; //from left to right
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            rb.velocity = Vector2.right * speed; //from right to left
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    void Update()
    {
        if (timer < invincTimer)
        {
            timer += Time.deltaTime;
        }
        else if (Mathf.Abs(transform.position.x) > 18f) Destroy(gameObject);

        //change volume        
        float distance = Vector2.Distance(transform.position, player.transform.position);
        float rawRatio = 1f - (distance / maxDistance);
        float volume = Mathf.Clamp(rawRatio * 0.1f, 0f, 0.1f);
        sfx.volume = volume;


    }
}
