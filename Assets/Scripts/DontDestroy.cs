using UnityEngine;

public class DontDestroy : MonoBehaviour
{    
    // Singleton instance
    public static DontDestroy Instance { get; private set; }
    
    // Audio sources
    public AudioSource _musicAudioSource; // Reference to the AudioSource component playing the music
    private bool _isMusicEnabled = true; // Flag to track if the music is currently enabled or disabled
    public bool isSfxEnabled = true; // Flag to track if the music is currently enabled or disabled

    // private void Start()
    // {
    //     // Find the AudioSource component attached to the GameObject with the "music" tag
    //     _musicAudioSource = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
    // }
    
    private void Awake()
    {
        _musicAudioSource = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();

        var objs = GameObject.FindGameObjectsWithTag("Music");
        
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
        
        // Set the instance to this object
        DontDestroy save = null;
        if (Instance != null && Instance != this)
        {
            save = Instance;
        }
        Instance = this;
        Destroy(save);
        DontDestroyOnLoad(Instance);
    }

    // Method to toggle the music on or off
    public void ToggleMusic()
    {
        _isMusicEnabled = !_isMusicEnabled; // Toggle the flag

        if (_isMusicEnabled)
        {
            // If music is enabled, resume playing
            _musicAudioSource.Play();
        }
        else
        {
            // If music is disabled, pause or stop playing
            _musicAudioSource.Pause(); // Alternatively, use Stop() if you want to reset playback position
        }
    }
    
    // Method to toggle the SFX on or off
    public void ToggleSfx()
    {
        print("Toggled SFX");
        isSfxEnabled = !isSfxEnabled;
    }
}