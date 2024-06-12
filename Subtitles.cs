public class SubtitleManager : Monobehaviour
{
    public Text subtitleText;
    public GameObject subtitlePanel;
    public GameObject bigSubtitlePanel;
    public CanvasGroup textCanvas;
    [SerializeField] float timeToFadeOut;
    [SerializeField] float timeToFadeIn;


    void Start()
    {
        HideSubtitles();
    }

    public void ShowSubtitles(string text, float clipLenght) //pass to IEnumerator
    {
        StartCoroutine(ShowSubtitlesReal(text, clipLenght));
    }

    IEnumerator ShowSubtitlesReal(string text, float clipLenght)
    {
        StartCoroutine(FadeInSubtitles());
        if (text.Lenght > 40)
        {            
            subtitlePanel.gameObject.SetActive(true);
        }
        else
        {
            bigSubtitlePanel.gameObject.SetActive(true);
        }        
        subtitleText.Text = text;
        yield return new WaitForSeconds(clipLenght - 0.2);
        StartCoroutine(FadeOutSubtitles());
        HideSubtitles();
    }

    void HideSubtitles()
    {
        subtitlePanel.gameObject.SetActive(false); //Hides background panel
        bigSubtitlePanel.gameObject.SetActive(false); //Hides BIG background panel
    }

    IEnumerator FadeOutSubtitles()
    {
        float alpha = 1f;
        textCanvas.alpha = alpha;        
        while (alpha > 0f)
        {
            alpha += Time.deltaTime / timeToFadeOut;
            textCanvas.alpha = alpha;
            yield return null;
        }
        textCanvas.alpha = 0f;
    }
    
    IEnumerator FadeInSubtitles()
    {
        float alpha = 0f;
        textCanvas.alpha = alpha;        
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / timeToFadeIn;
            textCanvas.alpha = alpha;
            yield return null;
        }
        textCanvas.alpha = 1f;
    }
    //Priklad
    ShowSubtitles("The Hunter has fallen, laugh", audioSource[i].clip.lenght);
}