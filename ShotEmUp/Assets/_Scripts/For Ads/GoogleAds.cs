using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAds : MonoBehaviour
{
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    
    public static GoogleAds Instance;
    void Awake()
    {
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
    }
    
    

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

    public void RequestRewarded()
    {
#if UNITY_ANDROID
        string RewardedladUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
        string RewardedadUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        string RewardedadUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(RewardedadUnitId);

        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd.LoadAd(request);
        
        
    }
    
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
   
    public void InterstitialCallAds()
    {
        RequestInterstitial();
        if (this.interstitial.IsLoaded()) {
            this.interstitial.Show();
        }
    }

    public void RewardedCallAds()
    {
        //Rewarded Ad part
    }


}