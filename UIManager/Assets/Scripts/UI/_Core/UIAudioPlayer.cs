using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class UIAudioPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip sfxClip;

    [SerializeField]
    private float volume = 1;

    private AudioSource sfxSource;

    public void PlayAudio()
    {
        AudioManager.Play2DSFX(sfxClip, volume);
    }
}