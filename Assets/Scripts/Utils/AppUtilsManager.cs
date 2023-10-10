using UnityEngine;
using System.Collections;
#if UNITY_ANDROID
using Google.Play.Review;
#endif
#if UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
using UnityEngine.iOS;
#endif

public class AppUtilsManager : MonoBehaviour
{
    public string googlePlayLink;
    public string appStoreLink;

#if UNITY_ANDROID
    private Google.Play.Review.ReviewManager reviewManager;
#endif

    public void ShowReviewFlow()
    {
        StartCoroutine(ShowGooglePlayReview());
        ShowIOSReview();
    }

    /// <summary>
    /// Locate proguard file "Assets/Plugins/Android/proguard-user.txt" and
    /// copy inside it the contents of:
    /// - "Assets/GooglePlayPlugins/com.google.play.core/Proguard/common.txt"
    /// - "Assets/GooglePlayPlugins/com.google.play.review/Proguard/review.txt"
    /// </summary>
    private IEnumerator ShowGooglePlayReview(bool fallbackToUrl = false)
    {
#if UNITY_ANDROID
        Debug.Log("Review requested");
        if (reviewManager == null)
        {
            reviewManager = new Google.Play.Review.ReviewManager();
        }
        var requestFlowOperation = reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error.
            // For example, using requestFlowOperation.Error.ToString().
            Application.OpenURL(googlePlayLink);
            yield break;
        }
        PlayReviewInfo _playReviewInfo = requestFlowOperation.GetResult();

        var launchFlowOperation
            = reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (fallbackToUrl
            && launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error.
            // For example, using requestFlowOperation.Error.ToString().
            Application.OpenURL(googlePlayLink);
            yield break;
        }
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown.
        // Thus, no matter the result, we continue our app flow.
#else
        yield break;
#endif
    }

    private void ShowIOSReview(bool fallbackToUrl = false)
    {
        Debug.Log("Review requested");
#if UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        bool isAvailable = Device.RequestStoreReview();
        if (fallbackToUrl && !isAvailable)
        {
            Application.OpenURL(appStoreLink);
        }
#endif
    }

    public void OpenAppStoreLink()
    {
#if UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        Application.OpenURL(appStoreLink);
#elif UNITY_ANDROID
        Application.OpenURL(googlePlayLink);
#endif
    }
}
