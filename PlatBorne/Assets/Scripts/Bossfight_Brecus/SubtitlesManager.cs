using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossFightVoiceLines : MonoBehaviour
{
    private string dialogue = null;
    public GameObject DialoguePanel;
    public Text DialogueText;
    private int j = 0;
    //***
    public float textSpeed;
    public float waitingTime;
    private float timerTextSpeed = 0;
    private bool textType = false;
    private bool textReset = false;
    private float timerTextReset = 0;
    //***

    public void AddSubtitles(string subtitles)
    {

    }

    void Start()
    {
        GameObject SubtitleText = new GameObject("SubtitleText");

        // Set the TextObject as a child of the Canvas
        SubtitleText.transform.SetParent(gameObject.transform);

        // Add a Text component to the GameObject
        Text text = SubtitleText.AddComponent<Text>();

        // Configure the Text component
        text.text = "Hello, Unity!";
        text.font = Resources.GetBuiltinResource<Font>("Retro Gaming.ttf");
        text.fontSize = 24;
        text.color = Color.white;

        // Optionally, configure the RectTransform to position the text
        RectTransform rectTransform = text.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(160, 30); // Width and height
        rectTransform.anchoredPosition = new Vector2(0, 0); // Position on the canvas
    }
}

