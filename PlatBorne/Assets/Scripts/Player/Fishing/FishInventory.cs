using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishInventory : MonoBehaviour
{
    //********************* PRIVATE
    private List<GameObject> fishInventory;
    private List<SpriteRenderer> fishSpriteRenderer;

    private int fishCatched;
    private List<Color> fishColors;

    //********************* PUBLIC

    public int FishCatched
    {
        get => fishCatched;
        set
        {
            if (value >= 0 && value <= 6)
                fishCatched = value;
            else
                fishCatched = 0;
        }
    }

    public int Count => fishInventory.Count;

    public Color NextFishColor => fishColors[fishCatched];

    private void Awake()
    {
        fishInventory = Resources.FindObjectsOfTypeAll<GameObject>()
            .Where(go => go.name.StartsWith("InventoryFish ("))
            .ToList();

        fishSpriteRenderer = new List<SpriteRenderer>();
        fishInventory.ForEach(fish => fishSpriteRenderer.Add(fish.GetComponent<SpriteRenderer>()));
        RandomizeColors();

        // Initialize the colors for each fish
        for (int i = 0; i < fishSpriteRenderer.Count; i++)
        {
            fishSpriteRenderer[i].color = fishColors[i];
        }
    }

    // Random Color Generation
    private void RandomizeColors()
    {
        fishColors = new List<Color>();
        for (int i = 0; i < 8; i++) // Generate 8 random colors
        {
            fishColors.Add(new Color(Random.value, Random.value, Random.value));
        }
    }

    // Method to catch fish
    public void CatchedFish()
    {
        if (fishCatched < fishInventory.Count)
        {
            var checkSpriteRenderer = fishInventory[fishCatched]
                .GetComponentsInChildren<SpriteRenderer>()
                .FirstOrDefault(sr => sr.name.Equals("Check"));

            if (checkSpriteRenderer != null)
            {
                checkSpriteRenderer.enabled = true;
            }
        }
        fishCatched++;
    }
}
