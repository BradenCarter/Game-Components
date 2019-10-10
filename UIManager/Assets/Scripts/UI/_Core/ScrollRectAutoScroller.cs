using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectAutoScroller : MonoBehaviour 
{
	ScrollRect sRect;
	[SerializeField]
	private bool scroll = true;
	[SerializeField]
	private Vector2 scrollPixelsPerFrame;

	private void Awake()
	{
		sRect = GetComponent<ScrollRect>();
	}

    public void Scroll()
	{
		scroll = true;	
	}

    public void Stop()
	{
		scroll = false;
	}


	private void Update()
    {
		if(scroll)
		{
			Vector2 scrollValue = Vector2.zero;
            if(sRect.vertical && sRect.verticalNormalizedPosition > 0 && Mathf.Abs(scrollPixelsPerFrame.y) > 0)
            {
				scrollValue.y = scrollPixelsPerFrame.y;
            }
			if(sRect.horizontal && sRect.horizontalNormalizedPosition > 0 && Mathf.Abs(scrollPixelsPerFrame.x) > 0)
            {
				scrollValue.x = scrollPixelsPerFrame.x;
            }
			sRect.content.anchoredPosition += scrollValue;
		}
	}
}
