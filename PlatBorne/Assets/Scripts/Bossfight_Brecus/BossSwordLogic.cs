using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossSwordLogic : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 60;
    private bool isRightSword = false;

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
        if (!isRightSword) transform.Rotate(Vector3.forward, -speed * Time.deltaTime);
        else transform.Rotate(Vector3.forward, speed * Time.deltaTime);
        if ((rb.transform.rotation.eulerAngles.z < 125  && !isRightSword)|| (rb.transform.rotation.eulerAngles.z > 180 && isRightSword))
        {
            Destroy(gameObject);
            Bossfight.attackIsGoingOn = false;
        }
    }
}
