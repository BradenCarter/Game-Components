using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleAudioPlayer : UIAudioPlayer {
   
	private void Awake()
    {
		GetComponent<Toggle>().onValueChanged.AddListener(PlayAudio);      
    }   

    private void OnDestroy()
    {
		GetComponent<Toggle>().onValueChanged.RemoveListener(PlayAudio);
    } 

    private void PlayAudio(bool selected)
	{
		base.PlayAudio();
	}
}
