/*using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;

    [SerializeField] string _androidIntersAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOsIntersAdUnitId = "Interstitial_iOS";

    [SerializeField] string _androidRewardedAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOsRewardedAdUnitId = "Rewarded_iOS";

    [SerializeField] string _androidBannerAdUnitId = "Banner_Android";
    [SerializeField] string _iOSBannerAdUnitId = "Banner_iOS";

    string _adIntersUnitId;
    string _adRewardedUnitId;
    string _adBannerUnitId;
    string _gameId;

    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

    [SerializeField] bool _testMode = true;

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }

        _adIntersUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsIntersAdUnitId
            : _androidIntersAdUnitId;

        _adRewardedUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsRewardedAdUnitId
            : _androidRewardedAdUnitId;

        _adBannerUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSBannerAdUnitId
            : _androidBannerAdUnitId;

        Advertisement.Banner.SetPosition(_bannerPosition);
    }

    // Implement a method to call when the Load Banner button is clicked:
    public void LoadBanner()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Advertisement.Banner.Load(_adBannerUnitId, options);
    }

    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
        ShowBannerAd();
    }

    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
    }

    void ShowBannerAd()
    {
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        Advertisement.Banner.Show(_adBannerUnitId, options);
    }

    void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }

    public void LoadInterstitial()
    {
        Debug.Log("Loading Ad: " + _adIntersUnitId);
        Advertisement.Load(_adIntersUnitId, this);
    }

    public void ShowInterstitial()
    {
        Debug.Log("Showing Ad: " + _adIntersUnitId);
        Advertisement.Show(_adIntersUnitId, this);
    }

    public void LoadRewarded()
    {
        Debug.Log("Loading Ad: " + _adRewardedUnitId);
        Advertisement.Load(_adRewardedUnitId, this);
    }

    public void ShowRewarded()
    {
        Debug.Log("Showing Ad: " + _adRewardedUnitId);
        Advertisement.Show(_adRewardedUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
    }

    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string _adUnitId) { }
    public void OnUnityAdsShowClick(string _adUnitId) { }
    public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (_adUnitId.Equals(_adRewardedUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            AdManager.Instance.CompletedRewarded();
        }
        if (_adUnitId.Equals(_adIntersUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Interstitial Ad Completed");
            AdManager.Instance.CompletedInterstitial();
        }
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");

        LoadBanner();
        LoadInterstitial();
        LoadRewarded();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
*/