using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_DirtSuicide : MonoBehaviour
{
    float timer = 0;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 4) Destroy(gameObject);
    }
}
