using System.Collections;
using UnityEngine;

public class Mole_AttackShovelRain : MonoBehaviour
{
    bool fadeOut = false;
    public float timeToStartFadeOut;
    bool isTouchingGround = false;
    [SerializeField] float timeToSelfDestruct;
    [SerializeField] float fadeSpeed;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    [SerializeField] BoxCollider2D boxCollider;

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
            if (!fadeOut) // Prevent starting the coroutine multiple times
            {
                fadeOut = true;
                StartCoroutine(FadeOutCoroutine());
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth hp = FindAnyObjectByType<PlayerHealth>();
            hp.PlayerDamage();
            Destroy(gameObject);
        }
    }

    private IEnumerator FadeOutCoroutine()
    {
        yield return new WaitForSeconds(timeToStartFadeOut);
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
