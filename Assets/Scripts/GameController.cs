using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    // Singleton instance
    public static GameController Instance { get; private set; }
    
    // Levels
    public int level;
    public bool isLevelingUp;
    public float levelIncrement;

    // Screen
    public TextMeshProUGUI pointsText;
    public GameOverScreen gameOverScreen;
    public bool isGameOver;

    // Emission rate
    public float emissionRate;

    // Color Emission
    public int currentColorIndex;
    public Color[] currentColor;
    
    // Audio effect
    public AudioSource audioSource;
    public GameObject explosionSfx;
    private AudioSource _explosionAudioSource;
    public AudioClip levelUpClip;
    public AudioClip explosionClip;
    public AudioClip startClip;
    
    // High Score
    public TextMeshProUGUI highScoreText;
    public bool newHighScoreAchieved;

    private void Awake()
    {
        // Set the instance to this object
        GameController save = null;
        if (Instance != null && Instance != this)
        {
            save = Instance;
        }
        Instance = this;

        // Set public variables
        emissionRate = Constants.StartEmissionRate;

        Destroy(save);
        
        // For build up audio source
        audioSource = gameObject.GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt(Constants.SfxToggledOn, 1) == 1)
        {
            audioSource.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeValue, 1f);
            audioSource.clip = startClip;
            audioSource.Play();
        }
        else
        {
            audioSource.volume = 0f;
        }
        
        // For explosion audio source
        _explosionAudioSource = explosionSfx.GetComponent<AudioSource>();
        
        // For testing high score
        // PlayerPrefs.SetInt(Constants.HighScore, 1);
    }

    public void Start()
    {
        pointsText.text = "Level 0";
        
        // For pause menu
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt(Constants.HighScore, 0);
        Cursor.visible = false;
    }

    public void GameOver()
    {
        // Set level text to white
        pointsText.color = Color.white;
        
        // Set cursor to visible, set up Game Over screen
        Cursor.visible = true;
        isGameOver = true;
        gameOverScreen.Setup(level);
    }

    private void FixedUpdate()
    {
        if (isLevelingUp)
        {
            levelIncrement += Time.deltaTime;
        }
    }

    public void LevelUp()
    {
        // Increment level
        levelIncrement = 0f;
        level++;
        if (PlayerPrefs.GetInt(Constants.SfxToggledOn, 1) == 1)
        {
            audioSource.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeValue, 1f);
            audioSource.clip = levelUpClip;
            audioSource.Play();
        }

        // Increment emission rate
        emissionRate = level < Constants.MaxLevelCapForIncrease ? emissionRate - Constants.EmissionRateDecrease : emissionRate;

        // Adjust color
        currentColorIndex++;
        if (currentColorIndex >= Constants.Colors.Count)
        {
            currentColorIndex = 0; // Reset to the first color set if reached the end
        }
        currentColor = Constants.Colors[currentColorIndex];
        
        // Set the text for the amount of levels passed
        pointsText.text = "Level " + level;

        // See if high score
        if (level > PlayerPrefs.GetInt(Constants.HighScore, 0))
        {
            NewHighScoreAchieved();
        }
    }
    
    public void NewHighScoreAchieved()
    {
        PlayerPrefs.SetInt(Constants.HighScore, level);
        highScoreText.text = "New High Score: " + level;
        newHighScoreAchieved = true;
    }
    
    // Play the explosion sound effect
    public void PlayExplosion()
    {
        if (PlayerPrefs.GetInt(Constants.SfxToggledOn, 1) != 1) return; 
        _explosionAudioSource.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeValue, 1f);
        _explosionAudioSource.clip = explosionClip;
        _explosionAudioSource.Play();
    }
}