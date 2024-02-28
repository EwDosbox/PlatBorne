using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FishSpawnScript : MonoBehaviour
{
    private GameObject fishPrefab;
    public FishSpawnScript(GameObject fishPrefab)
    {
        Destroy(Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "Fish(Clone)"));
        this.fishPrefab = fishPrefab;
    }
    public void SpawnFish(Color color)
    {
        fishPrefab.GetComponent<SpriteRenderer>().color = color;
        Instantiate(fishPrefab, RandomFishLocation(),new Quaternion());
        PlayerFishingScript.FishCatchArea = FindObjectsOfType<Collider2D>().FirstOrDefault(collider => collider.name == "FishCatchArea");
    }
    private Vector3 RandomFishLocation()
    {
        return new Vector3(Random.Range(-4f, 8.25f), -4.4f, 0);
    }
}
