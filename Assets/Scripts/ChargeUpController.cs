using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargeUpController : MonoBehaviour
{
    
    // Area in which the object generates charge
    public int multiplicationFactor = 2;
    public int visibleDuration = 500;
    public int invisibleDuration = 5000;
    public float fadeDuration = 0.25f; // Duration for fade-in and fade-out in seconds

    // Score bar to be used
    private int _scoreBar;
    private bool _isEntered;
    private HashSet<Collider2D> _triggeredObjects;
    private Coroutine _imageFadeCoroutine;

    public Slider slider;
    public GameObject imageObject;
    public GameObject player;
    public GameObject center;
    public float proximityRadius = 3.0f;
    public TextMeshProUGUI invincibleText;
    private ParticleSystem _gainParticleSystem;
    private ParticleSystem.MainModule _baseParticleSystem;
    private float _score;
    private static readonly int CosmicEmissionColor = Shader.PropertyToID("_EmissionColor");

    // Start is called before the first frame update
    private void Start()
    {
        transform.localScale *= multiplicationFactor;
        _triggeredObjects = new HashSet<Collider2D>();
        
        // Particle effect for close calls (can change colors)
        _gainParticleSystem = GameObject.Find("GainParticleEffect").GetComponent<ParticleSystem>();
        
        // Base particle effect on object (can change colors)
        _baseParticleSystem = GameObject.Find("BaseParticleEffect").GetComponent<ParticleSystem>().main;

        // Set background image and invincible text to invisible
        imageObject.GetComponent<Image>().CrossFadeAlpha(0f, 0f, true);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            _triggeredObjects.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            _triggeredObjects.Remove(other);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (PauseMenu.IsPaused) return;
        if (_triggeredObjects.Count != 0)
        {
            if (!_gainParticleSystem.isPlaying)
            {
                _gainParticleSystem.Play();
            }

            var multiplier = GameController.Instance.level < 14 ? -0.25 * (GameController.Instance.level + 1) + 5 : 1.5;
            multiplier *= Constants.Scale;
            var scoreIncrease = (float) multiplier * _triggeredObjects.Count / 10;
            slider.value += scoreIncrease;
        }
        else
        {
            _gainParticleSystem.Stop();
        }
        
        if (Vector3.Distance(player.transform.position, center.transform.position) <= proximityRadius)
        {
            slider.value += Time.deltaTime; // Increase slider gradually over time
        }

        if (!(slider.value >= 100)) return;

        _imageFadeCoroutine ??= null;
        _imageFadeCoroutine = StartCoroutine(ShowAndHideImage());
        slider.value = 0f;

        // Activate player GameObject's isTrigger effect for 2 seconds
        StartCoroutine(ActivatePlayerTriggerEffect());
    }
    
    private IEnumerator ShowAndHideImage()
    {
        var image = imageObject.GetComponent<Image>();

        // Set the desired color with the desired alpha value
        var targetColor = Color.white;
        if (GameController.Instance.currentColor.Length != 0)
        {
            targetColor = GameController.Instance.currentColor[Random.Range(0, GameController.Instance.currentColor.Length)]; // White color with 0 alpha (fully transparent)

        }

        // Set the initial color of the image
        image.color = targetColor;

        // Fade in
        image.CrossFadeColor(Color.white, 0f, true, true);
        yield return new WaitForSeconds(visibleDuration / 1000f);

        // Fade out
        image.CrossFadeColor(targetColor, fadeDuration, true, true);
        yield return new WaitForSeconds(invisibleDuration / 1000f);
    }


    private IEnumerator ActivatePlayerTriggerEffect()
    {
        GameController.Instance.LevelUp();
        GameController.Instance.isLevelingUp = true; // For planet prefabs
        
        // Control color rendering
        var gainParticleEffectMainModule = _gainParticleSystem.main;
        var playerMeshRenderer = player.GetComponent<MeshRenderer>();
    
        // Rapidly change colors
        Color currentColor;
        
        while (GameController.Instance.levelIncrement < Constants.InvincibleDuration/2)
        {
            var colorIndex = Mathf.FloorToInt(GameController.Instance.levelIncrement / Constants.ColorFlashDuration) % GameController.Instance.currentColor.Length;
            currentColor = GameController.Instance.currentColor[colorIndex];
            playerMeshRenderer.material.SetColor(CosmicEmissionColor, currentColor);
            invincibleText.color = currentColor;
            _baseParticleSystem.startColor = currentColor; // Set particle system color
            gainParticleEffectMainModule.startColor = currentColor; // Set near miss particle system color
            yield return null;
        }
    
        // Fade effect
        player.GetComponent<MeshRenderer>().enabled = true;

        const float colorFadeDuration = 1f; // Duration of fade effect
        var startColor = GameController.Instance.currentColor[UnityEngine.Random.Range(0, GameController.Instance.currentColor.Length)];
        var endColor = Color.white;
        invincibleText.color = Color.white;

        while (Constants.InvincibleDuration/2 <= GameController.Instance.levelIncrement && GameController.Instance.levelIncrement < Constants.InvincibleDuration)
        {
            var t = (GameController.Instance.levelIncrement - 1) / colorFadeDuration;
            currentColor = Color.Lerp(startColor, endColor, t);
            player.GetComponent<MeshRenderer>().material.SetColor(CosmicEmissionColor, currentColor);
            _baseParticleSystem.startColor = currentColor; // Set particle system color
            gainParticleEffectMainModule.startColor = currentColor; // Set near miss particle system color
            yield return null;
        }

        player.GetComponent<MeshRenderer>().material.SetColor(CosmicEmissionColor, endColor);
        GameController.Instance.isLevelingUp = false; // For planet prefabs
    }
}