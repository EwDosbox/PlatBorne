using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CameraBirghmingwayScript : MonoBehaviour
{
    [SerializeField] private GameObject hunter;
    private void Update()
    {
        this.transform.position = new Vector3(
            hunter.transform.position.x,
            hunter.transform.position.y,
            -25
       );
    }
}
