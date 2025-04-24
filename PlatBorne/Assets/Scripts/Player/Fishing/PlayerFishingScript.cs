using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerFishingScript : MonoBehaviour
{
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
        PlayerPrefs.SetString("Level", "fish");

        subtitlesManager = FindFirstObjectByType<SubtitlesManager>();
        hookRB = GetComponent<Rigidbody2D>();
        Collider2D[] colliders = FindObjectsOfType<Collider2D>();
        hookCatchArea = colliders.FirstOrDefault(collider => collider.name == "HookCatchArea");
        startOfCatch = Time.time;
        fishInventory = GameObject.FindGameObjectsWithTag("Fish").ToList();
        goFishParent = GameObject.Find("Inventory");

        RandomizeColors();

        SpawnFish(nextFishColor);

        // Random voice line
        if (PlayerPrefs.GetInt("wasFishing") == 1)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    if (PlayerPrefs.HasKey("Subtitles"))
                        subtitlesManager.Write("Yeah sure, go fishing, we have all the time in the world...", fishVoiceLine[0].clip.length);
                    fishVoiceLine[0].Play();
                    break;
                case 1:
                    if (PlayerPrefs.HasKey("Subtitles"))
                        subtitlesManager.Write("What makes you think a little fishing could help you?", fishVoiceLine[1].clip.length);
                    fishVoiceLine[1].Play();
                    break;
                case 2:
                    if (PlayerPrefs.HasKey("Subtitles"))
                        subtitlesManager.Write("You're so desperate that you go fishing? That's kinda fishy", fishVoiceLine[2].clip.length);
                    fishVoiceLine[2].Play();
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        //Movement
        if (shouldHookMoveLeft)
        {
            hookRB.AddRelativeForce(new Vector2(-(float)Equation(Time.time - holdTime), 0));
        }
        else if (shouldHookMoveRight)
        {
            hookRB.AddRelativeForce(new Vector2(+(float)Equation(Time.time - holdTime), 0));
        }

        // Fish Catching logic
        if (Physics2D.IsTouching(hookCatchArea, fishCatchArea))
        {
            if (isCatching)
            {
                float timeElapsed = Time.time - startOfCatch;
                if (timeElapsed > timeNeededToCatch)
                {
                    CatchedFish();

                    switch (noOfFishCatched)
                    {
                        case 6:
                            {
                                SpawnFish();
                                break;
                            }
                        case 7:
                            {
                                SceneManager.LoadScene("Cutscene_Ending Fish");
                                break;
                            }
                        default:
                            {
                                SpawnFish(nextFishColor);
                                break;
                            }
                    }
                }
            }
            else
            {
                startOfCatch = Time.time;
                isCatching = true;
            }
        }

    }

    private double Equation(float time)
    {
        return 4 + (1 - 4) / (1 + Mathf.Pow(time / 0.5f, 26));
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
}
