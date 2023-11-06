using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquareVisual : MonoBehaviour
{

    [SerializeField] private MeshRenderer meshRenderer;

    private void Start()
    {
        //if(!TryGetComponent<MeshRenderer>(out meshRenderer))
        //{
        //    Debug.LogError("Grid Square Visual unable to get mesh renderer!");
        //}
    }

    public void Show()
    {
        meshRenderer.enabled = true;
    }

    public void Hide()
    {
        meshRenderer.enabled=false;
    }

    private void OnValidate()
    {
        if(meshRenderer == null) { Debug.LogError($"{name} meshRenderer not assigned"); }
    }
}
