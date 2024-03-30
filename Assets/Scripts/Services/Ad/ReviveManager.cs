using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Services.Ad
{
    public class ReviveManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] string _androidGameId = "5586514";
        [SerializeField] string _iOSGameId = "5586515";
        [SerializeField] bool _testMode = true;
        [SerializeField] string _androidAdUnitId = "Rewarded_Android";
        [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
        [SerializeField] Button _adButtonRevive;
        
        private string _gameId;
        private string _adUnitId = null;
        
        private void Awake()
        {
            InitializeAds();
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
#elif UNITY_EDITOR
            _adUnitId = _androidAdUnitId;
#endif
            LoadAd();
        }
        
        public void InitializeAds()
        {
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
            _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId;
#endif
            if (!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(_gameId, _testMode, this);
            }
        }
        
        public void LoadAd()
        {
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }
        
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            Debug.Log("Ad Loaded: " + adUnitId);
 
            if (adUnitId.Equals(_adUnitId))
            {
                _adButtonRevive.onClick.AddListener(ShowAd);
            }
        }
        
        public void ShowAd()
        {
            Advertisement.Show(_adUnitId, this);
        }
        
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                Debug.Log("Unity Ads Rewarded Ad Completed");
                EventBus.Instance.onAdToReviveCompleted?.Invoke();
            }
        }
        
        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        }
 
        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
            LoadAd();
        }
 
        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }
        
        public void OnUnityAdsShowStart(string adUnitId) { }
        public void OnUnityAdsShowClick(string adUnitId) { }
        
        private void OnDestroy()
        {
            _adButtonRevive.onClick.RemoveAllListeners();
        }
    }
}