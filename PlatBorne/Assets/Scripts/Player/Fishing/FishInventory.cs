using System.Collections;
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
        get
        {
            return fishCatched;
        }
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

    public FishInventory()
    {
        fishInventory = Resources.FindObjectsOfTypeAll<GameObject>().Where(go => go.name.StartsWith("InventoryFish (")).ToList();
        fishSpriteRenderer = new List<SpriteRenderer>();
        //SAVE!!!!!!!!!!
        fishInventory.ForEach(fish => fishSpriteRenderer.Add(fish.GetComponent<SpriteRenderer>()));
        RandomizeColors();
        fishCatched = 0;
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
        //Save
        if (fishCatched < fishInventory.Count)
        {
            fishInventory[fishCatched].GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(sr => sr.name.Equals("Check")).enabled = true;
        }
        fishCatched++;
    }
}
