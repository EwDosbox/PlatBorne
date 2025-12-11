using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    private BetterRandom random = new BetterRandom($"FishManager at {System.DateTime.Now}");
    private int noOfFishCatched = 0;

    [Header("Fish Prefabs")]
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private GameObject fishRainbowPrefab;

    [Header("Color Inventory")]
    [SerializeField] private List<Color> fishColors = new List<Color>();
    [Header("Links")]
    [SerializeField] private GameObject goFishParent;
    [SerializeField] private GameObject fishInScene;
    [SerializeField] private PlayerFishingScript playerFishingScript;

    void Awake()
    {
        fishColors = RandomColors();
        SpawnFish(fishColors[0]);       
    }

    public void CatchFish()
    {
        noOfFishCatched++;
        Destroy(fishInScene);
        if (noOfFishCatched < fishColors.Count)
        {
            SpawnFish(fishColors[noOfFishCatched]);
        }
        else
        {
            SpawnFish();
        }
        playerFishingScript.FishCatchArea = fishInScene.GetComponent<Collider2D>();
    }

    private List<Color> RandomColors()
    {
        List<Color> colors = new List<Color>();
        for (int i = 0; i < 6; i++)
        {
            colors.Add(RandomColor());
        }
        return colors;
    }
    private Color RandomColor()
    {
        // TODO: Add a better random color generator
        return new Color(random.Random(1f), random.Random(1f), random.Random(1f));
    }

    private void SpawnFish(Color color)
    {
        fishInScene = Instantiate(fishPrefab, RandomFishLocation(), Quaternion.identity, goFishParent.transform);
        fishInScene.GetComponent<SpriteRenderer>().color = color;
    }
    private void SpawnFish()
    {//Rainbow fish
        fishInScene = Instantiate(fishRainbowPrefab, RandomFishLocation(), Quaternion.identity, goFishParent.transform);
    }
    
    private Vector3 RandomFishLocation()
    {
        return new Vector3(random.Range(-4f, 8.25f), -4.4f, 0);
    }
}
/*

    #region Private
    private Rigidbody2D hookRB;
    //Movement
    private bool shouldHookMoveLeft = false;
    private bool shouldHookMoveRight = false;

    private float holdTime = 0f;
    //fish Catching Logic
    private static Collider2D fishCatchArea;
    private Collider2D hookCatchArea;

    private float startOfCatch;
    private SubtitlesManager subtitlesManager;
    private bool isCatching = false;

    private List<GameObject> fishInventory = new List<GameObject>();
    private List<Color> fishColors = new List<Color>();
    private int noOfFishCatched = 0;
    private GameObject fishInScene;
    private BetterRandom random = new BetterRandom($"FishInventory at {System.DateTime.Now}");
    private GameObject goFishParent;
    #endregion
    #region SerializeField
    [Header("Fish Settings")]
    [SerializeField] private float timeNeededToCatch = 1.5f;

    [Header("Fish Prefabs")]
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private GameObject fishRainbowPrefab;
    [SerializeField] private AudioSource[] fishVoiceLine;
    #endregion
    #region Properties
    private Color nextFishColor => fishColors[noOfFishCatched + 1];
    public float HoldTime { get => holdTime; set => holdTime = value; }
    public bool ShouldHookMoveLeft { get => shouldHookMoveLeft; set => shouldHookMoveLeft = value; }
    public bool ShouldHookMoveRight { get => shouldHookMoveRight; set => shouldHookMoveRight = value; }
    #endregion

    private void Awake()
    {
        startOfCatch = Time.time;
        fishInventory = GameObject.FindGameObjectsWithTag("Fish").ToList();
        goFishParent = GameObject.Find("Inventory");

        RandomizeColors();
        SpawnFish(nextFishColor);
    }
    private Vector3 RandomFishLocation()
    {
        return new Vector3(Random.Range(-4f, 8.25f), -4.4f, 0);
    }

    private void SpawnFish(Color color)// spawn colored one
    {
        fishPrefab.GetComponent<SpriteRenderer>().color = color;
        //Add parent goFishParent
        fishInScene = Instantiate(fishPrefab, RandomFishLocation(), Quaternion.identity);
        fishCatchArea = FindObjectsOfType<Collider2D>().FirstOrDefault(collider => collider.name == "FishCatchArea");
    }
    private void SpawnFish()// spawn rainbow one
    {
        fishInScene = Instantiate(fishRainbowPrefab, RandomFishLocation(), Quaternion.identity);
        fishCatchArea = FindObjectsOfType<Collider2D>().FirstOrDefault(collider => collider.name == "FishCatchArea");
    }

    private void RandomizeColors()
    {
        fishColors = new List<Color>();
        for (int i = 0; i < 8; i++)
        {
            fishColors.Add(RandomColor(random));
        }
        foreach (GameObject fish in fishInventory)
        {
            SpriteRenderer checkSpriteRenderer = fish.GetComponentsInChildren<SpriteRenderer>()
                .FirstOrDefault(sr => sr.name.Equals("Check"));
            checkSpriteRenderer.enabled = false;
            fish.GetComponent<SpriteRenderer>().color = fishColors[fishInventory.IndexOf(fish)];
        }
    }
    private Color RandomColor(BetterRandom random)
    {
        return new Color(random.Random(1f), random.Random(1f), random.Random(1f));
    }

    public void CatchedFish()
    {
        isCatching = false;
        if (noOfFishCatched < 6)
        {
            fishInScene.SetActive(false);

            SpriteRenderer checkSpriteRenderer = fishInventory[noOfFishCatched]
                .GetComponentsInChildren<SpriteRenderer>()
                .FirstOrDefault(sr => sr.name.Equals("Check"));

            checkSpriteRenderer.enabled = true;

        }
        noOfFishCatched++;
    }
*/
