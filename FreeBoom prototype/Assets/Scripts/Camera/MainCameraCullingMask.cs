using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraCullingMask : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }

    public void CullingMaskOn(int mask)
    {
        //int layers = 1 << 8;
        mainCamera.cullingMask = mask;
    }

}
