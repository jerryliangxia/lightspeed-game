using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    // Singleton instance
    public TextMeshProUGUI pointsText;
    
    // Game objects
    public GameObject playerObject;
    public GameObject baseParticleEffect;
    public GameObject gainParticleEffect;
    private static readonly int CosmicEmissionColor = Shader.PropertyToID("_EmissionColor");

    // UI
    public Slider slider;
    public GameObject imageObject;
    
    public void Setup(int level)
    {
        gameObject.SetActive(true);
        pointsText.text = "Score: " + level;
    }

    public void RestartButton()
    {
        Cursor.visible = false;
        
        // GameController
        // Levels
        GameController.Instance.level = 0;
        GameController.Instance.isLevelingUp = false;
        GameController.Instance.levelIncrement = 0f;

        GameController.Instance.pointsText.text = "0";
        GameController.Instance.gameOverScreen.gameObject.SetActive(false);
        GameController.Instance.isGameOver = false;
        
        GameController.Instance.emissionRate = 0.4f;
        GameController.Instance.currentColorIndex = 0;
        GameController.Instance.audioSource.clip = GameController.Instance.startClip;

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
        if (PlayerPrefs.GetInt("SfxToggledOn", 1) != 1) return; 
        GameController.Instance.audioSource.volume = PlayerPrefs.GetFloat("SfxVolumeValue", 1f);
        GameController.Instance.audioSource.Play();
    }
    
    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
