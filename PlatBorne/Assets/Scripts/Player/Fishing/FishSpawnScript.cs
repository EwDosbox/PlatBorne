using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishSpawnScript : MonoBehaviour
{
    public GameObject fish;
    private void Awake()
    {
        Instantiate(fish);
        PlayerFishingScript.FishCatchArea = fish.GetComponentsInChildren<Collider2D>().FirstOrDefault(collider => collider.name == "FishCatchArea");
    }
    public void CatchedFish()
    {
        Destroy(fish);
        Instantiate(fish);
        PlayerFishingScript.FishCatchArea = fish.GetComponentsInChildren<Collider2D>().FirstOrDefault(collider => collider.name == "FishCatchArea");
    }
}
