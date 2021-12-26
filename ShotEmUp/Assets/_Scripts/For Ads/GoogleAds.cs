using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using Unity.VisualScripting;
using UnityEditor.PackageManager;

public class GoogleAds : MonoBehaviour
{
    public int playerHealth = 3;
    
    private LevelController controller;
    
    private InterstitialAd interstitial;
    private RewardedAd resumeRewardedAd;
    private RewardedAd levelEndRewardedAd;


    public static GoogleAds Instance;
    void Awake()
    {
        controller = FindObjectOfType<LevelController>();
        if (controller == null)
        {
            Debug.LogError("Level controller does not found");
        }
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        //Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        RequestInterstitial();

        this.resumeRewardedAd = RequestRewarded();
        this.levelEndRewardedAd = RequestRewarded();
    }

    #region Interstitial Ad
    public void RequestInterstitial()
    {
#if UNITY_ANDROID
        string InterstitialadUnitId = "ca-app-pub-5875384463485278/9600515707";
#elif UNITY_IPHONE
        string InterstitialadUnitId = "ca-app-pub-5875384463485278/2101299632";
#else
        string InterstitialadUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(InterstitialadUnitId);
        
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
        
        
        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;
    }

    #region Interstitial Ad Events
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
        print("Interstitial failed to load: " + args.Message);
        // Handle the ad failed to load event.
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
   #endregion
    
    public void InterstitialCallAds()
    {
        RequestInterstitial();
        if (this.interstitial.IsLoaded()) {
            this.interstitial.Show();
        }
    }
    #endregion

    #region Rewarded Ad
    public RewardedAd RequestRewarded()
    {
#if UNITY_ANDROID
        string RewardedadUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
        string RewardedadUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        string RewardedadUnitId = "unexpected_platform";
#endif
        RewardedAd rewardedAd = new RewardedAd(RewardedadUnitId);

        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        
        rewardedAd.LoadAd(CreateAdRequest());

        return rewardedAd;
    }

    private void HandleRewardedAdClosed(object sender, EventArgs e)
    {
        MonoBehaviour.print("HandleRewardedAdClosed");
    }

    private void HandleUserEarnedReward(object sender, Reward e)
    {
        MonoBehaviour.print("HandleUserEarnedReward event received and LevelEnd:" + controller.LevelEndCheck());
        if (!controller.LevelEndCheck())//Resume Game
        {
            controller.readyForEncounter = true;
            controller.RewardResume(playerHealth);
        }
        else if (controller.LevelEndCheck())//3x Reward
        {
            controller.SetRewardCoin(controller.GetRewardCoin() * 3);
        }
    }

    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        MonoBehaviour.print("HandleRewardedAdFailedToShow event received with message: " + e.Message);
    }

    private void HandleRewardedAdOpening(object sender, EventArgs e)
    {
        MonoBehaviour.print("HandeRewardedAdOpening event received");
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs e)
    {
        MonoBehaviour.print("HandleRewardedAdFailedToLoad event received with message:" + e.Message);
    }

    private void HandleRewardedAdLoaded(object sender, EventArgs e)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }
    
    public void ResumeRewardedAd()
    {
        if (this.resumeRewardedAd.IsLoaded())
        {
            this.resumeRewardedAd.Show();
        }
    }

    public void LevelEndRewardedAd()
    {
        if (this.levelEndRewardedAd.IsLoaded())
        {
            this.levelEndRewardedAd.Show();
        }
        Debug.Log(levelEndRewardedAd.GetRewardItem());
    }
    #endregion


    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddKeyword("unity-admob")
            .Build();
    }
}