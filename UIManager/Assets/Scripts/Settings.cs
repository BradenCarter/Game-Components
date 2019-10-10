using System.Collections.Generic;

#if UNITY_EDITOR

using UnityEditor;

#endif

using UnityEngine;

public class Settings
{
    private static List<string> iPhoneXDeviceIDs = new List<string>()
    {
        "iPhone10,3", "iPhone10,6", "iPhone11,8", "iPhone11,2", "iPhone11,4", "iPhone11,6"
    };

    public static class UI
    {
        public enum AspectRatio
        {
            Standard,
            Tablet,
            iPhoneX,
        }

        public static AspectRatio aspectRatio = AspectRatio.Standard;

        public static void DetermineAspectRatio()
        {
            //Standard = .56 or 1.78
            // iPhoneX = .46 or 2.1
            //iPad = .75 or 1.33
            float aspectRatioDec = 0f;
#if UNITY_EDITOR || UNITY_ANDROID
#if UNITY_EDITOR
            string[] res = UnityStats.screenRes.Split('x');
            Vector2 resolution = new Vector2(float.Parse(res[0]), float.Parse(res[1]));

#else
            Vector2 resolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
#endif
            aspectRatioDec = resolution.x / resolution.y;
            if (aspectRatioDec.AlmostEquals(.56f) || aspectRatioDec.AlmostEquals(1.78f))
                aspectRatio = AspectRatio.Standard;
            else if (aspectRatioDec.AlmostEquals(.46f) || aspectRatioDec.AlmostEquals(2.1f))
                aspectRatio = AspectRatio.iPhoneX;
            else if (aspectRatioDec.AlmostEquals(.75f) || aspectRatioDec.AlmostEquals(1.33f))
                aspectRatio = AspectRatio.Tablet;
#elif UNITY_IOS
            string deviceID = SystemInfo.deviceModel;
        if (iPhoneXDeviceIDs.Contains(deviceID))
            aspectRatio = AspectRatio.iPhoneX;
        else if (deviceID.Contains("iPad"))
            aspectRatio = AspectRatio.Tablet;
        else aspectRatio = AspectRatio.Standard;
#endif
        }

        public static bool IsIPhoneX
        {
            get
            {
                DetermineAspectRatio();
                return aspectRatio == AspectRatio.iPhoneX;
            }
        }
    }
}