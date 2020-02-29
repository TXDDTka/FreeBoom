using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererAdvancedOptions : MonoBehaviour {

    [Header("LineRenderer Parameters for unity 2017x")]

    [Tooltip("Assign Line Renderer Object")]
    public LineRenderer LineRendererObject;

    [Header("Parameters :")]

    [Tooltip("Old Parameter Options")]
    public float StartWidth = 0f;
    public float EndWidth = 0f;

  //  public Color StartColor;
   // public Color Endcolor;

   // public bool UseWorldSpace;

    public Vector3 localPosition = Vector3.zero;
    public Quaternion localRotation = Quaternion.identity;

    void Start () {
        LineRendererObject.startWidth = StartWidth;
        LineRendererObject.endWidth = EndWidth;

        // LineRendererObject.startColor = StartColor;
        //  LineRendererObject.endColor = Endcolor;

        //  UseWorldSpace = LineRendererObject.useWorldSpace;

      //  ChangeTransform();
    }
	
    public void ChangeTransform()
    {
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
    }
}
