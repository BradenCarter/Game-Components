using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Manager class to handle global audio.
///
/// Implemented through static functions to make it easily accessible.
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Static instance of the audio.
    /// </summary>
    private static AudioManager m_instance;

    /// <summary>
    /// Source to play background music.
    /// </summary>
    [SerializeField] private AudioSource m_bgMusic;

    /// <summary>
    /// Name of the current track being played.
    /// </summary>
    private string m_currentTrack;

    /// <summary>
    /// Source to play background noise.
    /// </summary>
    [SerializeField]
    private AudioSource m_bgNoise;

    /// <summary>
    /// Fade time for the music
    /// </summary>
    private float m_musicFadeTime = 0f;

    /// <summary>
    /// Fade counter for the music
    /// </summary>
    private float m_musicFadeCounter = 0f;

    /// <summary>
    /// Fade audio volume for music
    /// </summary>
    private float m_musicFadeAudioStart = 0f;

    /// <summary>
    /// Fade time for the noise
    /// </summary>
    private float m_noiseFadeTime = 0f;

    /// <summary>
    /// Fade counter for the noise
    /// </summary>
    private float m_noiseFadeCounter = 0f;

    /// <summary>
    /// Fade audio volume for noise
    /// </summary>
    private float m_noiseFadeAudioStart = 0f;

    /// <summary>
    /// Volume modifier for SFX. Should be a normalized value
    /// </summary>
    private static float sfxVolumeModifier = 1;
    public static float SFXVolumeModifier
    {
        get
        {
            return sfxVolumeModifier;
        }
        set
        {
            sfxVolumeModifier = value;
            PlayerPrefs.SetFloat(GameConstants.PPKEY_AUDIO_SFX, value);
            PlayerPrefs.Save();
            sfxVolumeModifier = Mathf.Clamp(sfxVolumeModifier, .01f, 1);
            m_instance.sfxMixer.audioMixer.SetFloat("sfxVolume", Mathf.Log(sfxVolumeModifier) * 20);
        }
    }

    /// <summary>
    /// Volume modifier for Music. Should be a normalized value
    /// </summary>
    private static float musicVolumeModifier = 1;
    public static float MusicVolumeModifier
    {
        get
        {
            return musicVolumeModifier;
        }
        set
        {
            musicVolumeModifier = value;
            PlayerPrefs.SetFloat(GameConstants.PPKEY_AUDIO_Music, value);
            PlayerPrefs.Save();
            musicVolumeModifier = Mathf.Clamp(musicVolumeModifier, .01f, 1);
            m_instance.musicMixer.audioMixer.SetFloat("musicVolume", Mathf.Log(musicVolumeModifier) * 20);
        }
    }

    private const float mixerGroupMuteSetting = -80;

    [SerializeField]
    private AudioMixerGroup musicMixer;
    public static AudioMixerGroup MusicMixer { get { return m_instance.musicMixer; } }

    [SerializeField]
    private AudioMixerGroup sfxMixer;
    public static AudioMixerGroup SFXMixer { get { return m_instance.sfxMixer; } }

    private void Awake()
    {
        m_instance = this;
        m_bgMusic.outputAudioMixerGroup = musicMixer;
        m_bgNoise.outputAudioMixerGroup = sfxMixer;
    }

    private void Update()
    {
        if (m_musicFadeCounter > 0f)
        {
            m_musicFadeCounter = Mathf.Max(0f, m_musicFadeCounter - Time.deltaTime);
            float curVol = (m_musicFadeCounter / m_musicFadeTime) * m_musicFadeAudioStart;

            if (curVol == 0)
            {
                m_bgMusic.Stop();
                m_instance.m_currentTrack = "";
            }
            else
                m_bgMusic.volume = curVol;
        }

        if (m_noiseFadeCounter > 0f)
        {
            m_noiseFadeCounter = Mathf.Max(0f, m_noiseFadeCounter - Time.deltaTime);
            float curVol = (m_noiseFadeCounter / m_noiseFadeTime) * m_noiseFadeAudioStart;

            if (curVol == 0)
                m_bgNoise.Stop();
            else
                m_bgNoise.volume = curVol;
        }
    }

    /// <summary>
    /// Loads up a background music track and plays it.
    ///
    /// 2D sound to keep consistent volume in AR.
    /// </summary>
    /// <param name="trackName">Name of track to load</param>
    /// <param name="volume">Volume to play the track at.</param>
    public static void PlayMusic(string trackName, float volume = 0.125f)
    {
        if (m_instance.m_currentTrack == trackName) return;

        AudioSource bg = m_instance.m_bgMusic;

        if (bg.isPlaying) bg.Stop();
        m_instance.m_currentTrack = trackName;

        AudioClip clip = (AudioClip)Resources.Load("Music/" + trackName);
        bg.outputAudioMixerGroup = MusicMixer;
        bg.clip = clip;
        bg.volume = volume;
        bg.Play();
    }

    /// <summary>
    /// Stops any music that is playing.
    /// </summary>
    public static void StopMusic()
    {
        AudioSource bg = m_instance.m_bgMusic;
        m_instance.m_currentTrack = "";
        if (bg.isPlaying) bg.Stop();
    }

    /// <summary>
    /// Starts up the fading of the music.
    /// </summary>
    /// <param name="fadeTime">The amount of time it takes to fade the song to nothing.</param>
    public static void FadeMusic(float fadeTime)
    {
        if (!m_instance.m_bgMusic.isPlaying) return;

        m_instance.m_musicFadeTime = m_instance.m_musicFadeCounter = fadeTime;
        m_instance.m_musicFadeAudioStart = m_instance.m_bgMusic.volume;
    }

    /// <summary>
    /// Loads up a background noise track and plays it.
    ///
    /// 2D sound to keep consistent volume in AR.
    /// </summary>
    /// <param name="trackName">Name of track to load</param>
    /// <param name="volume">Volume to play the track at.</param>
    public static void PlayBGNoise(string trackName, float volume = 0.125f)
    {
        AudioSource bg = m_instance.m_bgNoise;

        if (bg.isPlaying) bg.Stop();

        AudioClip clip = (AudioClip)Resources.Load("BGNoise/" + trackName);
        bg.clip = clip;
        bg.outputAudioMixerGroup = SFXMixer;
        bg.volume = volume;
        bg.Play();
    }

    /// <summary>
    /// Stops any background noise that is playing.
    /// </summary>
    public static void StopBGNoise()
    {
        AudioSource bg = m_instance.m_bgNoise;

        if (bg.isPlaying) bg.Stop();
    }

    /// <summary>
    /// Starts up the fading of the background noise.
    /// </summary>
    /// <param name="fadeTime">The amount of time it takes to fade the noise track to nothing.</param>
    public static void FadeBGNoise(float fadeTime)
    {
        if (!m_instance.m_bgMusic.isPlaying) return;

        m_instance.m_noiseFadeTime = m_instance.m_noiseFadeCounter = fadeTime;
        m_instance.m_noiseFadeAudioStart = m_instance.m_bgNoise.volume;
    }

    /// <summary>
    /// Creates and plays a self destroying SFX object with 3D sound
    /// </summary>
    /// <param name="sfxName">Name of the sfx object</param>
    /// <param name="position">World position to spawn the sfx at</param>
    /// <param name="parent">The optional parent transform for the object.</param>
    /// <param name="volume">Volume of the clip</param>
    public static void Play3DSFX(string sfxName, Vector3 position, Transform parent = null, float volume = 0.25f)
    {
        AudioSource SFX = (GameObject.Instantiate(Resources.Load("SFX/SpatialAudioSource")) as GameObject).GetComponent<AudioSource>();
        SFX.clip = (AudioClip)Resources.Load("SFX/" + sfxName);
        SFX.transform.position = position;
        SFX.volume = volume;
        if (parent != null)
            SFX.transform.parent = parent;
        SFX.outputAudioMixerGroup = SFXMixer;
        SFX.Play();
    }

    /// <summary>
    /// Creates and plays a self destroying SFX object with 2D sound
    /// </summary>
    /// <param name="sfxName">Name of the sfx object</param>
    /// <param name="volume">Volume of the clip</param>
    public static void Play2DSFX(string sfxName, float volume = 0.5f)
    {
        Play2DSFX((AudioClip)Resources.Load("SFX/" + sfxName));
    }

    public static void Play2DSFX(AudioClip sfxClip, float volume = 0.5f)
    {
        AudioSource SFX = (GameObject.Instantiate(Resources.Load("SFX/2DAudioSource")) as GameObject).GetComponent<AudioSource>();
        SFX.clip = sfxClip;
        SFX.outputAudioMixerGroup = SFXMixer;
        SFX.volume = volume;
        SFX.Play();
    }
}