using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class FaceSwitcher : MonoBehaviour
{
    [SerializeField] private List<GameObject> filters = new();
    [SerializeField] private List<Material> materials = new();
    [SerializeField] private Material defaultMat;
    [SerializeField] private SkinnedMeshRenderer mesh;

    private int filterIndex = 0;
    private int materialIndex = 0;

    public void ChangeFilter(bool next)
    {
        filters[filterIndex].SetActive(false);

        filterIndex = next ? filterIndex + 1 : filterIndex - 1;

        //Check for values out of range 
        if (filterIndex >= filters.Count)
            filterIndex = 0;
        else if (filterIndex < 0)
            filterIndex = filters.Count - 1;

        //Activate next or last face filter
        filters[filterIndex].SetActive(true);

        //Change mat
        materialIndex = filterIndex;
        mesh.material =
            materialIndex >= materials.Count ? defaultMat : materials[filterIndex];
    }
}
