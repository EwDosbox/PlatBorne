using UnityEngine;

public class BossSwordLogic : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject left;
    private GameObject right;
    [SerializeField] private float speed = 2;
    private bool isRightSword = false;
    private float rotation = 0f;
    [SerializeField] private float wantedRotation = 180;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb.position.x >= 0)
        {
            isRightSword = true;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        rotation = wantedRotation * (timer / speed);
        if (!isRightSword) rotation = -rotation;
        Quaternion newRotation = Quaternion.Euler(0, 0, rotation);
        transform.rotation = newRotation;
        if (timer > speed + (speed / 2)) Destroy(gameObject);
    }
}
