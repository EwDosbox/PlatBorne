using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_Spikes : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D collider2D;
    [SerializeField] private float timeToSelfDestruct;
    private float alpha = 0f;
    [SerializeField] private float fadeSpeed = 0.4f;
    private bool fadeIn = false;
    private bool fadeOut = false;
    private SpriteRenderer spriteRenderer;
    private float timer = 0;
    bool spikesAreActive = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
    private void Awake()
    {
        collider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        fadeIn = true;
    }

    private void Update()
    {
        if (fadeIn)
        {
            alpha += fadeSpeed * Time.deltaTime;
            spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            if (alpha >= 1)
            {
                fadeIn = false;
                spikesAreActive = true;
            }
        }
        else if (fadeOut)
        {
            spikesAreActive = false;
            alpha -= fadeSpeed * Time.deltaTime;
            spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            if (alpha <= 0) Destroy(gameObject);
        }
        timer += Time.deltaTime;
        if (timer > timeToSelfDestruct) fadeOut = true;
        if (spikesAreActive)
        {
            collider2D.enabled = true;
        }
        else
        {
            collider2D.enabled = false;
        }
    }
}