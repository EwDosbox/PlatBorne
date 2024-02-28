using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishInventory : MonoBehaviour
{
    //********************* PRIVATE
    private List<GameObject> fishInventory;
    private List<SpriteRenderer> fishSpriteRenderer;

    private static int fishCatched;
    private static List<Color> fishColors;

    //********************* PUBLIC
    public static List<Color> FishColors
    {
        get { return fishColors; }
    }
    public static Color NextFishColor
    {
        get { return fishColors[fishCatched]; }
    }
    private void Awake()
    {
        fishInventory = Resources.FindObjectsOfTypeAll<GameObject>().Where(go => go.name.StartsWith("InventoryFish (")).ToList();
        fishSpriteRenderer = new List<SpriteRenderer>();
        fishInventory.ForEach(fish => fishSpriteRenderer.Add(fish.GetComponent<SpriteRenderer>()));
        Debug.Log(fishSpriteRenderer[0].color);
        RandomizeColors();
        fishCatched = 0;
        for (int i = 0; i < fishSpriteRenderer.Count; i++)
        {
            fishSpriteRenderer[i].color = fishColors[i];
        }
    }
    // Random Color
    public static void RandomizeColors()
    {
        fishColors = new List<Color>();
        for (int i = 0; i < 6; i++)
        {
            fishColors.Add(new Color(Random.value, Random.value, Random.value));
        }
    }
    //Catched Fish
    public static void CatchedFish()
    {
        fishCatched++;
    }
}
