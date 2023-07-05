using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class FilterController : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;

    private FaceSwitcher switcher;
    private int indx;
    private void OnEnable()
    {
        nextButton.onClick.AddListener(() => SwitchFilter(true));
        backButton.onClick.AddListener(() => SwitchFilter(false));
    }

    private void SwitchFilter(bool next)
    {
        if (!switcher) switcher = FindObjectOfType<FaceSwitcher>();
        if (!switcher) return; //No face, no switcher

        switcher.ChangeFilter(next);
    }

    private void OnDisable()
    {
        nextButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }

}
