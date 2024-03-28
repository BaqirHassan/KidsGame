using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum SFXType
    {
        Celebration,
        BalloonPopping
    }


    [SerializeField] AudioSource MenuMusic;
    [SerializeField] AudioSource PlayMusic;

    [SerializeField] AudioClip[] CelebrationSounds;
    [SerializeField] AudioClip[] BalloonPoppingSounds;

    private AudioSource SFXPlayer;

    public static SoundManager Instance;

    void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;

        SFXPlayer = GetComponent<AudioSource>();
        if (SFXPlayer == null )
            Debug.LogWarning("No Audio Source attached to Sound Manager.", this);
    }

    private void Start()
    {
        GameConstantsAndData.OnMusicPreferanceChange += MusicPreferanceChanged;
        GameConstantsAndData.OnSoundPreferanceChange += SoundPreferanceChanged;
    }

    private void OnDestroy()
    {
        GameConstantsAndData.OnMusicPreferanceChange -= MusicPreferanceChanged;
        GameConstantsAndData.OnSoundPreferanceChange -= SoundPreferanceChanged;
    }

    #region Musics

    private void MusicPreferanceChanged(bool Value)
    {
        if (Value)
        {
            if (GameConstantsAndData.IsMenuMusic)
                PlayMenuMusic();
            else
                PlayPlayMusic();
        }
        else
        {
            StopAllMusic();
        }
    }

    public static void StopMenuMusic()
    {
        Instance.MenuMusic.Stop();
    }

    public static void StopPlayMusic()
    {
        Instance.PlayMusic.Stop();
    }

    public static void StopAllMusic()
    {
        Instance.MenuMusic.Stop();
        Instance.PlayMusic.Stop();
    }

    public static void PlayMenuMusic()
    {
        if (!GameConstantsAndData.MusicPreferance) return;
        Instance.PlayMusic.Stop();

        if(!Instance.MenuMusic.isPlaying)
        {
            Instance.MenuMusic.Play();
        }
    }

    public static void PlayPlayMusic()
    {
        if (!GameConstantsAndData.MusicPreferance) return;
        Instance.MenuMusic.Stop();

        if (!Instance.PlayMusic.isPlaying)
        {
            Instance.PlayMusic.Play();
        }
    }

    public static void BlendMenuPlayMusic()
    {
        if (!GameConstantsAndData.MusicPreferance) return;
        if (!Instance.MenuMusic.isPlaying)
        {
            Instance.PlayMusic.Play();
            Instance.PlayMusic.volume = 1f;
        }
        else
        {
            Instance.PlayMusic.volume = 0f;
            Instance.StartCoroutine(Instance.BlendMenuPlayMusicCO());
        }
    }

    IEnumerator BlendMenuPlayMusicCO()
    {
        PlayMusic.Play();
        while(PlayMusic.volume < 1)
        {
            PlayMusic.volume += Time.deltaTime;
            MenuMusic.volume -= Time.deltaTime;
            yield return null;
        }

        PlayMusic.volume = 1;
        MenuMusic.volume = 0;

        MenuMusic.Stop();
    }

    #endregion


    #region Sound Effects

    private void SoundPreferanceChanged(bool Value)
    {
        if (!Value)        
        {
            StopAllSounds();
        }
    }

    public static void StopAllSounds()
    {   
        Instance.SFXPlayer.Stop();
    }

    /// <summary>
    /// Play a sound effect with "PlayOneShot" method.
    /// </summary>
    public static void PlaySFX(AudioClip audioToPlay)
    {
        if (!GameConstantsAndData.SoundPreferance) return;
        if (audioToPlay == null)
        {
            Debug.LogError("audioToPlay is null.", Instance);
            return;
        }

        Instance.SFXPlayer.PlayOneShot(audioToPlay);
    }

    /// <summary>
    /// Play an SFX that is preConfigured in Sound Manager.
    /// </summary>
    /// <param name="PreLoadedSFX"> The name/type of preConfigured SFX</param>
    public static void PlaySFX(SFXType PreLoadedSFX)
    {
        if (!GameConstantsAndData.SoundPreferance) return;
        if (PreLoadedSFX == SFXType.Celebration)
        {
            PlayCelebrationSound();
            return;
        }
        else if(PreLoadedSFX == SFXType.BalloonPopping)
        {
            PlayBalloonPoppingSound();
            return;
        }
    }

    /// <summary>
    /// Play a sound effect that can be controll later. Like stop or Volume.
    /// </summary>
    public static void PlaySFXWithControll()
    {
        if (!GameConstantsAndData.SoundPreferance) return;

    }

    /// <summary>
    /// Stops any sound effect that was played with "PlaySFXWithControll" method.
    /// </summary>
    public static void StopPlayingSFX()
    {
        if (!GameConstantsAndData.SoundPreferance) return;
    }

    private static void PlayCelebrationSound()
    {
        if (!GameConstantsAndData.SoundPreferance) return;
        Instance.SFXPlayer.PlayOneShot(Instance.CelebrationSounds[Random.Range(0, Instance.CelebrationSounds.Length)]);
    }

    private static void PlayBalloonPoppingSound()
    {
        if (!GameConstantsAndData.SoundPreferance) return;
        Instance.SFXPlayer.PlayOneShot(Instance.BalloonPoppingSounds[Random.Range(0, Instance.BalloonPoppingSounds.Length)]);
    }

    #endregion
}
