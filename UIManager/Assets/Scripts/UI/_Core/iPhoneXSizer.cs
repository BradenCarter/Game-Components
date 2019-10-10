using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class iPhoneXSizer : MonoBehaviour
{
    //"Values for when device is iPhoneX"
    private Vector4 iPhoneXOffset = new Vector4(65f, 0, -60, 60);

    // Use this for initialization
    private void Start()
    {
        RectTransform rectTrans = GetComponent<RectTransform>();
        Vector4 offsets = Vector4.zero;
        if (Settings.UI.IsIPhoneX)
            offsets = iPhoneXOffset;
        rectTrans.offsetMax = new Vector2(offsets.z, offsets.y);
        rectTrans.offsetMin = new Vector2(offsets.x, offsets.w);
    }
}