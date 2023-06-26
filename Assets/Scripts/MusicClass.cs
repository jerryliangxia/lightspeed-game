using UnityEngine;

public class MusicClass : MonoBehaviour
{
    // For PlayerPrefs
    private const string Music = "music";
    private const string Sfx = "sfx";
    private const string Off = "off";
    private const string On = "on";
    private AudioSource _musicAudioSource;
    
    // Audio/Sfx
    private static AudioSource _source;
    private AudioClip _explosionSfx;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _source.clip = _explosionSfx;
        _musicAudioSource = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        
        // Set PlayerPrefs variables if not set
        if (!PlayerPrefs.HasKey(Music))
        {
            PlayerPrefs.SetString(Music, On);
            _musicAudioSource.Play();
        }
        else if (PlayerPrefs.GetString(Music, On) == On)
        {
            _musicAudioSource.Play();
        }
        
        if (!PlayerPrefs.HasKey(Sfx))
        {
            PlayerPrefs.SetString(Sfx, On);
        }
        PlayerPrefs.Save();
    }

    public void ToggleMusic()
    {
        // If the music is playing
        if (PlayerPrefs.GetString(Music, On) == On)
        {
            _musicAudioSource.Pause();
            PlayerPrefs.SetString(Music, Off);
        }
        else
        {
            _musicAudioSource.Play();
            PlayerPrefs.SetString(Music, On);
        }
        PlayerPrefs.Save();
        print(PlayerPrefs.GetString(Music, On));
    }
    
    public void ToggleSfx()
    {
        // If the Sfx is on
        PlayerPrefs.SetString(Sfx, PlayerPrefs.GetString(Sfx, On) == On ? Off : On);
        PlayerPrefs.Save();
        print(PlayerPrefs.GetString(Sfx, On));
    }

    public static void PlayExplosion()
    {
        if(PlayerPrefs.GetString(Sfx, On) == On) _source.Play();
    }
}