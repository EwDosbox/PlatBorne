using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerFishingScript : MonoBehaviour
{
    #region Private
    private Rigidbody2D hookRB;
    //Movement
    private bool shouldHookMoveLeft = false;
    private bool shouldHookMoveRight = false;

    private float holdTime = 0f;
    //fish Catching Logic
    private Collider2D fishCatchArea;
    private Collider2D hookCatchArea;

    private float startOfCatch;
    private SubtitlesManager subtitlesManager;
    private bool isCatching = false;
    #endregion
    #region SerializeField
    [Header("Links")]
    [SerializeField] private FishManager fishManager;
    [Header("Fish Settings")]
    [SerializeField] private float timeNeededToCatch = 1.5f;

    [Header("Fish Prefabs")]
    [SerializeField] private AudioSource[] fishVoiceLine;
    #endregion
    #region Properties
    public Collider2D FishCatchArea { get => fishCatchArea; set => fishCatchArea = value; }
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
                    fishManager.CatchFish();
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
}
