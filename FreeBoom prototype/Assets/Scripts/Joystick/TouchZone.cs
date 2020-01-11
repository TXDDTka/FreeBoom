﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchZone : MonoBehaviour
{
    [SerializeField] private bool editInScene = false;
    [SerializeField] private RectTransform leftZone = null;
    [SerializeField] private RectTransform rightZone = null;

    [Header("Sides")]
    [SerializeField, Range(0, 0.2f)] private float hOffet = 0;
    [SerializeField, Range(0, 0.2f)] private float vOffset = 0;

    [Header("Middle")]
    [SerializeField, Range(0, 0.2f)] private float hOffset1 = 0;
    [SerializeField, Range(0, 0.2f)] private float vOffset1 = 0;

    private void OnValidate()
    {
        if (editInScene)
        {
            if (!leftZone || !rightZone) return;

            leftZone.anchorMin = new Vector2(hOffet, vOffset);
            //leftZone.anchorMax = new Vector2(0.5f, 0.5f);
            leftZone.anchorMax = new Vector2(0.5f - hOffset1, 0.5f - vOffset1);

            //rightZone.anchorMin = new Vector2(0.5f, vOffset);
            //rightZone.anchorMax = new Vector2(1 - hOffet, 0.5f);
            rightZone.anchorMin = new Vector2(0.5f + hOffset1, vOffset);
            rightZone.anchorMax = new Vector2(1 - hOffet, 0.5f - vOffset1);

            leftZone.sizeDelta = leftZone.anchoredPosition = rightZone.sizeDelta = rightZone.anchoredPosition = Vector2.zero;
        }
    }
}