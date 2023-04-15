using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// It takes a screenshot when a button is pressed. It saves the screenshot on phone gallery.
/// 
/// (Camera icon by Icons8)
/// </summary>

public class ScreenCapturer : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask screenshotLayerMask;
    [SerializeField] private TextMeshProUGUI screenshotText;
    [SerializeField] private Button screenshotButton;

    private LayerMask normalLayerMask;
    private void OnEnable()
    {
        normalLayerMask = cam.cullingMask;

        if (screenshotButton)
            screenshotButton.onClick.AddListener(() => StartCoroutine(TakeScreenshot()));
    }
    private void OnDisable()
    {
        if (screenshotButton)
            screenshotButton.onClick.AddListener(() => StartCoroutine(TakeScreenshot()));
    }

    private IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();
        string name = $"screenshot-{System.DateTime.Now:MM-dd-yy (HH-mm-ss)}.png";
        SaveImageToGallery(GetCameraView(), name, "Screenshot from AR Filter app (FEZ Workshop)");
        StartCoroutine(ShowTextForTwoSec());
    }

    private Texture2D GetCameraView()
    {
        //Create render texture and add it to cam
        RenderTexture screenTexture = new(Screen.width, Screen.height, 16);
        cam.targetTexture = screenTexture;

        //Hide UI and other things
        cam.cullingMask = screenshotLayerMask;

        //Activate render texture and render
        RenderTexture.active = screenTexture;
        cam.Render();

        //Read actual texture
        Texture2D renderedTexture = new(Screen.width, Screen.height);
        renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        //Reset afterwards
        screenTexture.Release();
        RenderTexture.active = null;
        cam.targetTexture = null;
        cam.cullingMask = normalLayerMask;

        return renderedTexture;
    }
    private IEnumerator ShowTextForTwoSec()
    {
        screenshotText.text = "Image captured!";
        yield return new WaitForSeconds(1.5f);
        screenshotText.text = "";
    }
 

    #region ScreenshotSaving
    // Solution by http://answers.unity.com/answers/1216480/view.html

    private const string mediaStoreImagesMediaClass = "android.provider.MediaStore$Images$Media";
    private static AndroidJavaObject _activity;
    public static AndroidJavaObject Activity
    {
        get
        {
            if (_activity == null)
            {
                var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                _activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return _activity;
        }
    }

    public static string SaveImageToGallery(Texture2D texture2D, string title, string description)
    {
        using var mediaClass = new AndroidJavaClass(mediaStoreImagesMediaClass);
        using var cr = Activity.Call<AndroidJavaObject>("getContentResolver");
        var image = Texture2DToAndroidBitmap(texture2D);
        var imageUrl = mediaClass.CallStatic<string>("insertImage", cr, image, title, description);
        return imageUrl;
    }

    public static AndroidJavaObject Texture2DToAndroidBitmap(Texture2D texture2D)
    {
        byte[] encoded = texture2D.EncodeToPNG();
        using var bf = new AndroidJavaClass("android.graphics.BitmapFactory");
        return bf.CallStatic<AndroidJavaObject>("decodeByteArray", encoded, 0, encoded.Length);
    }
    #endregion
}
