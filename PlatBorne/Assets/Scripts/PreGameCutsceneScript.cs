using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PreGameCutscene : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer.loopPointReached += videoDone;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Cutscene skipped");
            SceneManager.LoadScene("LevelLondon");
            Debug.Log("Scene: LevelLondon");
        }
    }

    private void videoDone(VideoPlayer videoPlayer)
    {
        SceneManager.LoadScene("LevelLondon");
        Debug.Log("Scene: LevelLondon");
    }
}
