using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndgameCutscene : MonoBehaviour
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
            SceneManager.LoadScene("MainMenu");
            Debug.Log("Scene: MainMenu");
        }
    }

    private void videoDone(VideoPlayer videoPlayer)
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Scene: MainMenu");
    }
}
