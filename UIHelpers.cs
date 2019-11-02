using PlayFab.ClientModels;
using PlayFab.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public static class UIHelpers
{
    #region Extension Methods

    public static string SplitCamelCase(this string str)
    {
        return Regex.Replace(
            Regex.Replace(
                str,
                @"(\P{Ll})(\P{Ll}\p{Ll})",
                "$1 $2"
            ),
            @"(\p{Ll})(\P{Ll})",
            "$1 $2"
        );
    }


    public static bool AlmostEquals(this float float1, float float2, int precision = 2)
    {
        float epsilon = Mathf.Pow(10.0f, -precision);
        return (Mathf.Abs(float1 - float2) <= epsilon);
    }

    public static float NormalizeBetween(this float float1, float min, float max)
    {
        return Mathf.Clamp01((float1 - min) / (max - min));
    }

    public static float NormalizeBetween(this int number, int min, int max)
    {
        return Mathf.Clamp01((float)(number - min) / (float)(max - min));
    }

    public static List<T> Shuffle<T>(this IList<T> list)
    {
        System.Random rand = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }


    public static string FormatToGameString(this TimeSpan time)
    {
        string formattedString = "";
        if (time.Days > 0)
        {
            formattedString += time.Days + "D";
        }
        if (time.Hours > 0)
        {
            if (time.Days > 0)
            {
                formattedString += " ";
                formattedString += time.Hours + "H";
                return formattedString;
            }
            formattedString += time.Hours + "H";
        }
        if (time.Minutes > 0)
        {
            if (time.Hours > 0) formattedString += " ";
            formattedString += time.Minutes + "MIN";
        }
        if (time.Hours == 0 && time.Seconds > 0)
        {
            if (time.Minutes > 0) formattedString += " ";
            formattedString += time.Seconds + "SEC";
        }
        return formattedString;
    }

    #region Unity Specific
    public static Transform SearchForChild(this Transform target, string name)
    {
        for (int i = 0; i < target.childCount; ++i)
        {
            if (target.GetChild(i).name == name) return target.GetChild(i);
            var result = SearchForChild(target.GetChild(i), name);
            if (result != null) return result;
        }

        return null;
    }

    public static Toggle GetActive(this ToggleGroup aGroup)
    {
        IEnumerator<Toggle> toggleEnum = aGroup.ActiveToggles().GetEnumerator();
        toggleEnum.MoveNext();
        return toggleEnum.Current;
    }


    public static Vector2 WorldToCanvas(this Canvas canvas, Vector3 world_position, bool keepOnScreen = false, Camera camera = null)
    {
        if (camera == null)
        {
            camera = Camera.main;
        }

        var viewport_position = camera.WorldToViewportPoint(world_position);
        return ViewportToCanvas(canvas, viewport_position, keepOnScreen, camera);
    }

    public static Vector2 GetCanvasPosForWorldObject(Transform target, RectTransform canvasRect)
    {
        return GetCanvasPosForWorldObject(target, canvasRect, Vector2.zero);
    }

    public static Vector2 GetCanvasPosForWorldObject(Transform target, RectTransform canvasRect, Vector2 offset)
    {
        Vector2 canvasSize = canvasRect.sizeDelta;
        Vector3 targetCanvasPercent = Camera.main.WorldToViewportPoint(target.position);
        float xPos = canvasSize.x * targetCanvasPercent.x;
        float yPos = canvasSize.y * targetCanvasPercent.y;
        return new Vector2(xPos, yPos) + offset;
    }

    public static Vector2 GetPosInCanvasForUIObject(RectTransform target, Transform canvasTransform = null)
    {
        if (canvasTransform == null)
            canvasTransform = target.GetComponentInParent<Canvas>().transform;
        return canvasTransform.InverseTransformPoint(target.position);
    }

    public static Vector2 GetCanvasPosForPixelInput(Vector2 input, RectTransform canvasRect)
    {
        float xBounds = canvasRect.sizeDelta.x * canvasRect.localScale.x;
        float yBounds = canvasRect.sizeDelta.y * canvasRect.localScale.y;
        float normX = input.x / xBounds;
        float normY = input.y / yBounds;
        Vector2 pos = new Vector2(canvasRect.sizeDelta.x * normX, canvasRect.sizeDelta.y * normY);
        return pos;
    }

    public static Vector2 ViewportToCanvas(this Canvas canvas, Vector3 viewport_position, bool keepOnScreen = false, Camera camera = null)
    {
        if (camera == null)
        {
            camera = Camera.main;
        }
        var canvas_rect = canvas.GetComponent<RectTransform>();
        if (keepOnScreen && TargetIsOffScreen(viewport_position))
            viewport_position = new Vector2(0.5f, 0.5f);
        return new Vector2((viewport_position.x * canvas_rect.sizeDelta.x) - (canvas_rect.sizeDelta.x * 0.5f),
                           (viewport_position.y * canvas_rect.sizeDelta.y) - (canvas_rect.sizeDelta.y * 0.5f));
    }

    private static bool TargetIsOffScreen(Vector3 targetScreenPos)
    {
        if (targetScreenPos.x < 0 || targetScreenPos.x > 1) return true;
        if (targetScreenPos.y < 0 || targetScreenPos.y > 1) return true;
        if (targetScreenPos.z < 0) return true;
        return false;
    }

    public static bool IsSameAs(this Color color, Color compareColor)
    {
        if (compareColor == null) return false;
        float threshold = .01f;
        return System.Math.Abs(color.r - compareColor.r) < threshold && System.Math.Abs(color.g - compareColor.g) < threshold && System.Math.Abs(color.b - compareColor.b) < threshold;
    }

    public static Vector2 SizeToParent(this RawImage image, float padding = 0)
    {
        float w = 0, h = 0;
        var parent = image.GetComponentInParent<RectTransform>();
        var imageTransform = image.GetComponent<RectTransform>();

        // check if there is something to do
        if (image.texture != null)
        {
            if (!parent) { return imageTransform.sizeDelta; } //if we don't have a parent, just return our current width;
            padding = 1 - padding;
            float ratio = image.texture.width / (float)image.texture.height;
            var bounds = new Rect(0, 0, parent.rect.width, parent.rect.height);
            if (Mathf.RoundToInt(imageTransform.eulerAngles.z) % 180 == 90)
            {
                //Invert the bounds if the image is rotated
                bounds.size = new Vector2(bounds.height, bounds.width);
            }
            //Size by height first
            h = bounds.height * padding;
            w = h * ratio;
            if (w > bounds.width * padding)
            { //If it doesn't fit, fallback to width;
                w = bounds.width * padding;
                h = w / ratio;
            }
        }
        imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
        return imageTransform.sizeDelta;
    }


    public static void ShowAndBlock(this CanvasGroup cGroup)
    {
        cGroup.alpha = 1;
        cGroup.interactable = true;
        cGroup.blocksRaycasts = true;
    }

    public static void HideAndPassthrough(this CanvasGroup cGroup)
    {
        cGroup.alpha = 0;
        cGroup.interactable = false;
        cGroup.blocksRaycasts = false;
    }
    #endregion Unity Specific
    #endregion Extension Methods


    public static Type GetAssemblyType(string typeName)
    {
        var type = Type.GetType(typeName);
        if (type != null) return type;
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = a.GetType(typeName);
            if (type != null) return type;
        }
        return null;
    }

    public static void DisableObjects<T>(List<T> objects) where T : MonoBehaviour
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].gameObject.SetActive(false);
        }
    }
}

public static class UIListBuilder
{
    public static void UpdateDisplays<T, U>(ICollection<T> itemList, GameObject template, LayoutGroup container, ref List<U> displays, Action<U, int> OnDisplaySet = null) where U : MonoBehaviour
    {
        UpdateDisplays(itemList, template, container.transform, ref displays, OnDisplaySet);
    }

    public static void UpdateDisplays<T, U>(ICollection<T> itemList, GameObject template, Transform container, ref List<U> displays, Action<U, int> OnDisplaySet = null) where U : MonoBehaviour
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (displays.Count <= i)
            {
                displays.Add(UnityEngine.Object.Instantiate<GameObject>(template.gameObject, container.transform).GetComponent<U>());
            }
            OnDisplaySet?.Invoke(displays[i], i);
            displays[i].gameObject.SetActive(true);
        }
    }
}