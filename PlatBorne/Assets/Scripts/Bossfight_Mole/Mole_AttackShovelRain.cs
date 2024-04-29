using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_AttackShovelRain : MonoBehaviour
{
    bool fadeOut = false;
    float alpha = 1f;
    float timer = 0;
    bool isTouchingGround = false;
    [SerializeField]  float timeToSelfDestruct;
    [SerializeField] Collider ground;
    [SerializeField] float fadeSpeed = 0.3f;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isTouchingGround = true;
    }
    void Update()
    {
        if (isTouchingGround) timer += Time.deltaTime;
        if (fadeOut)
        {            
            alpha -= fadeSpeed * Time.deltaTime;
            spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            if (alpha <= 0) Destroy(gameObject);
        }    
    }
}