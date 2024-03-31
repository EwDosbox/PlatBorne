using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class FishInventory
{
    //********************* PRIVATE
    private List<GameObject> fishInventory;
    private List<SpriteRenderer> fishSpriteRenderer;

    private int fishCatched;
    private List<Color> fishColors;

    //********************* PUBLIC
    public Saves save;

    public int FishCatched
    {
        get
        {
            return fishCatched;
        }
        set { if (value >= 0 && value <= 6) fishCatched = value; else fishCatched = 0; }
    }
    public int Count
    {
        get
        {
            return fishInventory.Count;
        }
    }
    public Color NextFishColor
    {
        get { return fishColors[fishCatched]; }

    }

    public FishInventory(int fishCatched)
    {
        fishInventory = Resources.FindObjectsOfTypeAll<GameObject>().Where(go => go.name.StartsWith("InventoryFish (")).ToList();
        fishSpriteRenderer = new List<SpriteRenderer>();        
        fishInventory.ForEach(fish => fishSpriteRenderer.Add(fish.GetComponent<SpriteRenderer>()));
        RandomizeColors();
        FishCatched = fishCatched;
        for (int i = 0; i < fishSpriteRenderer.Count; i++)
        {
            fishSpriteRenderer[i].color = fishColors[i];
        }
    }

    // Random Color
    private void RandomizeColors()
    {
        fishColors = new List<Color>();
        for (int i = 0; i <= 7; i++)
        {
            fishColors.Add(new Color(Random.value, Random.value, Random.value));
        }
    }
    //Catched Fish
    public void CatchedFish()
    {
        if (fishCatched < fishInventory.Count)
        {
            fishInventory[fishCatched].GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(sr => sr.name.Equals("Check")).enabled = true;
        }
        fishCatched++;
    }
}
