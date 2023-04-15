using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// It takes a screenshot when a button is pressed.
/// 
/// (Camera icon by Icons8)
/// </summary>

[RequireComponent(typeof(Button))]
public class ScreenCapturer : MonoBehaviour
{
     private Button screenshotButton;

    private void OnEnable()
    {
        screenshotButton = GetComponent<Button>();
        screenshotButton.onClick.AddListener(() => StartCoroutine(TakeScreenshot()));
    }
    private void OnDisable()
    {
        if (screenshotButton)
            screenshotButton.onClick.AddListener(() => StartCoroutine(TakeScreenshot()));
    }

    IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshot("screenshot " + System.DateTime.Now.ToString("MM-dd-yy (HH-mm-ss)") + ".png");
    }
}
