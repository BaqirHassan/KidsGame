using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstantsAndData : MonoBehaviour
{
    public delegate void PreferanceChanged(bool Value);
    public static PreferanceChanged OnSoundPreferanceChange;
    public static PreferanceChanged OnMusicPreferanceChange;

    /// <summary>
    /// The prefered state of all Game Sounds. True means On/Sounds should be played and False Means off/Sounds should not be played.
    /// </summary>
    public static bool SoundPreferance
    {
        get
        {
            return PlayerPrefs.GetInt("SoundPreferance", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("SoundPreferance", value ? 1 : 0);
            PlayerPrefs.Save();

            OnSoundPreferanceChange?.Invoke(value);
        }
    }

    /// <summary>
    /// The prefered state of all Music Sounds. True means On/Music should be played and False Means off/Music should not be played.
    /// </summary>
    public static bool MusicPreferance
    {
        get
        {
            return PlayerPrefs.GetInt("MusicPreferance", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("MusicPreferance", value ? 1 : 0);
            PlayerPrefs.Save();

            OnMusicPreferanceChange?.Invoke(value);
        }
    }

    /// <summary>
    /// Type of music that should be played. i.e Menu music or Play music.
    /// </summary>
    public static bool IsMenuMusic = true;
}
