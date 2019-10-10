
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class UIButtonAudioPlayer : UIAudioPlayer, IPointerDownHandler
{
    public bool useClick = false;
    private Button btn;
    
	private void Awake()
	{
        btn = GetComponent<Button>();

        if(useClick)
            btn.onClick.AddListener(PlayAudio);      
	}   

	private void OnDestroy()
	{
        if (useClick)
            btn.onClick.RemoveListener(PlayAudio);
	}
    

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (useClick || btn.interactable == false) return;

        PlayAudio();
    }
}
