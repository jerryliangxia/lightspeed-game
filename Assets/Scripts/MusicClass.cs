using UnityEngine;

public class MusicClass : MonoBehaviour
{
    // For PlayerPrefs
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
        if (!PlayerPrefs.HasKey(Constants.Music))
        {
            PlayerPrefs.SetString(Constants.Music, Constants.On);
        }
        _musicAudioSource.Play();
        
        if (!PlayerPrefs.HasKey(Constants.Sfx))
        {
            PlayerPrefs.SetString(Constants.Sfx, Constants.On);
        }
        PlayerPrefs.Save();
    }

    public void ToggleMusic()
    {
        // If the music is playing
        if (PlayerPrefs.GetString(Constants.Music, Constants.On) == Constants.On)
        {
            _musicAudioSource.Pause();
            PlayerPrefs.SetString(Constants.Music, Constants.Off);
        }
        else
        {
            _musicAudioSource.Play();
            PlayerPrefs.SetString(Constants.Music, Constants.On);
        }
        PlayerPrefs.Save();
        print(PlayerPrefs.GetString(Constants.Music, Constants.On));
    }
    
    public void ToggleSfx()
    {
        // If the Sfx is on
        PlayerPrefs.SetString(Constants.Sfx, PlayerPrefs.GetString(Constants.Sfx, Constants.On) == Constants.On ? Constants.Off : Constants.On);
        PlayerPrefs.Save();
        print(PlayerPrefs.GetString(Constants.Sfx, Constants.On));
    }

    public static void PlayExplosion()
    {
        if (PlayerPrefs.GetInt("SfxToggledOn", 1) != 1) return;
        _source.volume = PlayerPrefs.GetFloat("SfxVolumeValue", 0.0f);
        _source.Play();
    }
}