using UnityEngine;

public class BossLeechLogic : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float timeToSelfDestruct;
    [SerializeField] private float acceleration;
    private float timer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity += Vector2.down * 3;
        transform.rotation = Quaternion.Euler(Vector3.forward * 90);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity += Vector2.down * acceleration * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer > timeToSelfDestruct)
        {
            Destroy(gameObject);
        }
    }
}
