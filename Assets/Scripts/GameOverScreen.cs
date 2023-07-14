using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    // Game objects
    public GameObject playerObject;
    public GameObject baseParticleEffect;
    public GameObject gainParticleEffect;
    private static readonly int CosmicEmissionColor = Shader.PropertyToID("_EmissionColor");

    // UI
    public Slider slider;
    public GameObject imageObject;
    public TextMeshProUGUI pointsText;

    public void Setup(int level)
    {
        gameObject.SetActive(true);
        if (GameController.Instance.newHighScoreAchieved)
        {
            pointsText.fontSize = 23f;
            pointsText.text = "New High Score! Level " + level;
        }
        else
        {
            pointsText.fontSize = 24f;
            pointsText.text = "Score: " + level;
        }
    }

    public void RestartButton()
    {
        Cursor.visible = false;
        
        // GameController
        // Levels
        GameController.Instance.level = 0;
        GameController.Instance.isLevelingUp = false;
        GameController.Instance.levelIncrement = 0f;
        
        GameController.Instance.pointsText.text = "Level 0";
        GameController.Instance.gameOverScreen.gameObject.SetActive(false);
        GameController.Instance.isGameOver = false;
        
        GameController.Instance.emissionRate = 0.4f;
        GameController.Instance.currentColorIndex = 0;
        GameController.Instance.audioSource.clip = GameController.Instance.startClip;

        GameController.Instance.highScoreText.text = "High Score: " + PlayerPrefs.GetInt(Constants.HighScore);
        GameController.Instance.newHighScoreAchieved = false;

        // Player
        playerObject.SetActive(true);
        playerObject.GetComponent<MeshRenderer>().material.SetColor(CosmicEmissionColor, Color.white);
        playerObject.transform.position = new Vector3(15.5f, 0f, 0f);
        
        // Particles on player
        var mainModule = baseParticleEffect.GetComponent<ParticleSystem>().main;
        mainModule.startColor = Color.white;
        var mainModule1 = gainParticleEffect.GetComponent<ParticleSystem>().main;
        mainModule1.startColor = Color.white;
        
        // Explosion effect
        var pemBase = PlayerController.Instance.explosionEffect.GetComponent<Renderer>().sharedMaterial;
        pemBase.SetColor(CosmicEmissionColor, Color.white); 
        
        // UI
        slider.value = 0f;
        slider.fillRect.GetComponent<Image>().color = Color.white;
        imageObject.GetComponent<Image>().color = Color.white;
        imageObject.GetComponent<Image>().CrossFadeColor(Color.white, 0.25f, true, true);

        // Play intro sound
        if (PlayerPrefs.GetInt(Constants.SfxToggledOn, 1) != 1) return; 
        GameController.Instance.audioSource.volume = PlayerPrefs.GetFloat(Constants.SfxVolumeValue, 1f);
        GameController.Instance.audioSource.Play();
    }
    
    public void ExitButton()
    {
        SceneManager.LoadScene(Constants.MainMenuScene);
    }
}
