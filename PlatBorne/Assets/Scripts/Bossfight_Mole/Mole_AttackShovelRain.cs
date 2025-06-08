using System.Collections;
using UnityEngine;

public class Mole_AttackShovelRain : MonoBehaviour
{
    bool fadeOut = false;
    float timer = 0;
    float timeToBoost = 1;
    bool speedBoost = false;
    public float timeToStartFadeOut;
    [SerializeField] float timeToSelfDestruct;
    [SerializeField] float fadeSpeed;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    [SerializeField] BoxCollider2D boxCollider;
    bool canDamage = true;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            if (!fadeOut)
            {
                fadeOut = true;
                StartCoroutine(FadeOutCoroutine());
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0f;
            }
        }
    }

    private void Update()
    {
        if (!speedBoost)
        {
            if (timer > 1)
            {
                rb.gravityScale = 2f;
                speedBoost = true;
            }
            else timer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canDamage && collision.CompareTag("Player"))
        {
            PlayerHealth hp = FindAnyObjectByType<PlayerHealth>();
            hp.PlayerDamage();
            Destroy(gameObject);
        }
    }

    private IEnumerator FadeOutCoroutine()
    {
        yield return new WaitForSeconds(timeToStartFadeOut);
        canDamage = false;
        rb.bodyType = RigidbodyType2D.Static;
        boxCollider.enabled = false;
        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= fadeSpeed * Time.deltaTime;
            spriteRenderer.color = new Color(1f, 1f, 1f, Mathf.Clamp01(alpha));
            yield return null;
        }
        Destroy(gameObject);
    }
}
