using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("Canvas")] 
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private Canvas settingsCanvas;
    [SerializeField] private Canvas failCanvas;
    [SerializeField] private Canvas levelCompleteCanvas;

    [Header("Events List")] 
    [SerializeField] private Button startButton;
    [SerializeField] private Button adButton;

    [Header("Level Controller")] [SerializeField]
    private LevelController controller;
    private bool levelStarted = false;

    [Header("Coin System")] 
    [SerializeField] private CoinSystem coinSystem;

    [SerializeField] private Text levelRewardText;
    
    private void Start()
    {
        controller = GetComponent<LevelController>();
        coinSystem = GetComponent<CoinSystem>();
    }

    private void LateUpdate()
    {
        if (levelCompleteCanvas.enabled)
        {
            levelRewardText.text = controller.GetRewardCoin().ToString();
        }
    }

    public void StartLevel()
    {
        controller.NextEncounter();
        levelStarted = true;
        startButton.gameObject.SetActive(false);
        adButton.gameObject.SetActive(false);
    }

    public void Settings(bool active)
    {
        menuCanvas.gameObject.SetActive(!active);
        settingsCanvas.gameObject.SetActive(active);
        if (active)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void LevelFail()
    {
        Time.timeScale = 0f;
        failCanvas.gameObject.SetActive(true);      
        GoogleAds.Instance.InterstitialCallAds();
    }

    public void LevelSuccess()
    {
        Time.timeScale = 1f;
        levelCompleteCanvas.gameObject.SetActive(true);
    }
    
    public void ResetLevel()//Reset level if button clicked
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
    public void NextLevel()//Reset level if button clicked
    {
        coinSystem.AddCoin(controller.GetRewardCoin());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;  
        GoogleAds.Instance.InterstitialCallAds();
    }

    public void WatchToResume()
    {
        GoogleAds.Instance.ResumeRewardedAd();
        Debug.Log("Resume rewarded Ad Called");
        failCanvas.transform.GetChild(2).gameObject.SetActive(false);//Close resume button for next time
    }

    public void WatchToMoreReward()
    {
        GoogleAds.Instance.LevelEndRewardedAd();
        Debug.Log("x3 rewarded Ad Called");
        levelCompleteCanvas.transform.GetChild(2).gameObject.SetActive(false);//Close resume button for next time
    }

    public void ResumeGame()
    {
        failCanvas.gameObject.SetActive(false);
    }
}
