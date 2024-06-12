public class PlayerBigFallVoiceLine : Monobehaviour
{
    public AudioSource[] voiceLines;
    public Subtitles subtitle;
    int[] randomNumbers;
    string[] voiceLineText;
    int index = 0;
    System.Random rng = new System.Random();

    void Randomize()
    {
        int random1, random2, bucket;
        for (int i = 0; i < 1000; i++)
        {
            random1 = rng.Next(0, randomNumbers.Length);
            random2 = rng.Next(0, randomNumbers.Length);
            bucket = randomNumbers[random1];
            randomNumbers[random1] = randomNumbers[random2];
            randomNumbers[random2] = bucket;
        }
    }

    void Start()
    {
        randomNumbers = new int[voiceLines.Length];
        for (int i = 0; i < voiceLines.Length;i++)
        {
            randomNumbers[i] = i;
        }
        Randomize();
    }

    public void PlayVL()
    {
        voiceLines[randomNumbers[index]].Play();
        subtitles.ShowSubtitles(voiceLineText[randomNumbers[index]],voiceLines[randomNumbers[index]].clip.Length);
        index++;
        if (index == voiceLines.Length - 1)
        {
            index = 0;
            Randomize();
        }
    }
}