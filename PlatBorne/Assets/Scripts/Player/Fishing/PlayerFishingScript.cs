using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerFishingScript : MonoBehaviour
{
    #region Private
    private BetterRandom random = new BetterRandom($"FishManager at {System.DateTime.Now}");
    private int noOfFishCatched = 0;
    private GameObject fishInScene;
    private Rigidbody2D hookRB;
    //Movement
    private bool shouldHookMoveLeft = false;
    private bool shouldHookMoveRight = false;
    private float holdTime = 0f;
    //fish Catching Logic
    private Collider2D fishCatchArea;
    private float startOfCatch;
    private SubtitlesManager subtitlesManager;
    private bool isCatching = false;
    #endregion
    #region SerializeField

    [Header("Inventories")]
    [SerializeField] private List<GameObject> fishInventory = new List<GameObject>();
    [SerializeField] private Color[] fishColors = new Color[6];
    [Header("Fish Settings")]
    [SerializeField] private float timeNeededToCatch = 1.5f;

    [Header("Links")]
    [SerializeField] private GameObject goFishParent;
    [SerializeField] private Collider2D hookCatchArea;
    [Header("Fish Prefabs")]
    [SerializeField] private AudioSource[] fishVoiceLine;
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private GameObject fishRainbowPrefab;
    #endregion
    #region Properties
    public float HoldTime { get => holdTime; set => holdTime = value; }
    public bool ShouldHookMoveLeft { get => shouldHookMoveLeft; set => shouldHookMoveLeft = value; }
    public bool ShouldHookMoveRight { get => shouldHookMoveRight; set => shouldHookMoveRight = value; }
    #endregion

    private void Awake()
    {
        PlayerPrefs.SetString("Level", "fish");

        fishColors = RandomColors(6);
        for (int i = 0; i < fishInventory.Count; i++)
        {
            fishInventory[i].GetComponent<SpriteRenderer>().color = fishColors[i];
        }
        SpawnFish(fishColors[0]);

        subtitlesManager = FindFirstObjectByType<SubtitlesManager>();
        hookRB = GetComponent<Rigidbody2D>();
        startOfCatch = Time.time;

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
                    CatchFish();
            }
            else
            {
                startOfCatch = Time.time;
                isCatching = true;
            }
        }
        else
        {
            isCatching = false;
        }
    }

    private void CatchFish()
    {
        noOfFishCatched++;
        Destroy(fishInScene);

        isCatching = false;
        fishInventory[noOfFishCatched - 1].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;

        if (noOfFishCatched < fishColors.Count())
            SpawnFish(fishColors[noOfFishCatched]);
        else if (noOfFishCatched == fishColors.Count())
            SpawnFish(Color.clear, true);
        else
            Debug.Log("Move onto credits");
    }
    private void SpawnFish(Color color, bool isRainbow = false)
    {
        if (!isRainbow)
        {
            fishInScene = Instantiate(fishPrefab, RandomFishLocation(), Quaternion.identity, goFishParent.transform);
            fishInScene.GetComponent<SpriteRenderer>().color = color;
        }
        else
            fishInScene = Instantiate(fishRainbowPrefab, RandomFishLocation(), Quaternion.identity, goFishParent.transform);
        fishCatchArea = fishInScene.transform.GetChild(2).GetComponent<Collider2D>();
    }
    private Vector3 RandomFishLocation()
    {
        return new Vector3(random.Range(-4f, 8.25f), -4.4f, 0);
    }
    private double Equation(float time)
    {
        return 4 + (1 - 4) / (1 + Mathf.Pow(time / 0.5f, 26));
    }
    private Color[] RandomColors(int number)
    {
        Color[] colors = new Color[number];
        for (int i = 0; i < number; i++)
        {
            Color color;
            do
            {
                color = random.Color(0.2f, 1f);
            } while (AreSimilar(color, colors));
            colors[i] = color;
        }
        return colors;
    }
    private bool AreSimilar(Color a, Color[] colors)
    {
        foreach (Color color in colors)
        {
            float distance = Mathf.Sqrt(Mathf.Pow(a.r - color.r, 2) + Mathf.Pow(a.g - color.g, 2) + Mathf.Pow(a.b - color.b, 2));
            if (distance < 0.25f)
                return true;
        }
        return false;
    }
}
