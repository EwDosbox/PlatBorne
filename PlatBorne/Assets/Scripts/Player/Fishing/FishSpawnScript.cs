using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FishSpawnScript : MonoBehaviour
{
    private GameObject fishPrefab;

    private void Awake()
    {
        PlayerPrefs.SetString("Level", "fish");
    }
    public FishSpawnScript(GameObject fishPrefab)
    {
        Destroy(Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "Fish(Clone)"));
        this.fishPrefab = fishPrefab;
    }
    public void SpawnFish(Color color)// spawn colored one
    {
        fishPrefab.GetComponent<SpriteRenderer>().color = color;
        Instantiate(fishPrefab, RandomFishLocation(),new Quaternion());
        PlayerFishingScript.FishCatchArea = FindObjectsOfType<Collider2D>().FirstOrDefault(collider => collider.name == "FishCatchArea");
    }
    public void SpawnFish()// spawn rainbow one
    {
        Instantiate(fishPrefab, RandomFishLocation(), new Quaternion());
        PlayerFishingScript.FishCatchArea = FindObjectsOfType<Collider2D>().FirstOrDefault(collider => collider.name == "FishCatchArea");
    }
    private Vector3 RandomFishLocation()
    {
        return new Vector3(Random.Range(-4f, 8.25f), -4.4f, 0);
    }
}
