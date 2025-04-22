using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishInventory : MonoBehaviour
{
    #region Private
    private List<GameObject> fishInventory;

    private int noOfFishCatched;
    private List<Color> fishColors;

    #endregion
    #region Public

    public int NoOfFishCatched
    {
        get => noOfFishCatched;
        set
        {
            if (value >= 0 && value <= 6)
                noOfFishCatched = value;
            else
                noOfFishCatched = 0;
        }
    }

    public Color NextFishColor => fishColors[noOfFishCatched];
    #endregion

    private void Awake()
    {
        fishInventory = GameObject.FindGameObjectsWithTag("Fish").ToList();

        RandomizeColors();

        for (int i = 0; i < fishInventory.Count; i++)
        {
            fishInventory[i].GetComponent<SpriteRenderer>().color = fishColors[i];
        }
    }

    private void RandomizeColors()
    {
        fishColors = new List<Color>();
        BetterRandom random = new BetterRandom($"FishInventory at {Time.time}");
        for (int i = 0; i < 8; i++)
        {
            fishColors.Add(RandomColor(random));
        }
    }
    private Color RandomColor(BetterRandom random)
    {
        return new Color(random.Random(1f), random.Random(1f), random.Random(1f));
    }

    // Method to catch fish
    public void CatchedFish()
    {
        if (noOfFishCatched < 6)
        {
            fishInventory[noOfFishCatched].SetActive(false);

            SpriteRenderer checkSpriteRenderer = fishInventory[noOfFishCatched]
                .GetComponentsInChildren<SpriteRenderer>()
                .FirstOrDefault(sr => sr.name.Equals("Check"));

            checkSpriteRenderer.enabled = true;

        }
        noOfFishCatched++;
    }
}
