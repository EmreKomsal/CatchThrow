using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointHandler : MonoBehaviour
{
    public bool isDebugBuild;
    private int levelID = 1;
    // Start is called before the first frame update
    void Awake()
    {
        if (isDebugBuild)
        {
            PlayerPrefs.DeleteAll();
        }
        
        levelID = PlayerPrefs.GetInt("checkpoint");
        if (PlayerPrefs.GetInt("checkpoint") < 1)
        {
            PlayerPrefs.SetInt("checkpoint", 1);
            levelID = 1;
        }
        
        SceneManager.LoadScene(levelID);
    }
}
