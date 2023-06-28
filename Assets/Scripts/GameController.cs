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
    private int _currentColorIndex;
    public Color[] currentColor;

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
    }

    public void Start()
    {
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

    private void Update()
    {
        pointsText.text = "Level " + level;

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
        
        // Increment emission rate
        emissionRate = level < Constants.MaxLevelCapForIncrease ? emissionRate - Constants.EmissionRateDecrease : emissionRate;

        // Adjust color
        _currentColorIndex++;
        if (_currentColorIndex >= Constants.Colors.Count)
        {
            _currentColorIndex = 0; // Reset to the first color set if reached the end
        }
        currentColor = Constants.Colors[_currentColorIndex];
    }
}