using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class NearMissController : MonoBehaviour
{
    public int scoreBonus = 2000;
    public int multiplicationFactor = 2;
    public float secondsToWaitFor = 0.5f;

    private Coroutine _textFadeCoroutine;
    private HashSet<Collider2D> _triggeredObjects;

    private const float FadeOutTime = 1.5f;
    public TextMeshProUGUI bonusPointsText;
    public TextMeshProUGUI bonusPointsMessage;
    
    // Define the color thresholds
    private readonly Dictionary<int, Color> _colorThresholds = new Dictionary<int, Color>()
    {
        { 4000, new Color(1f, 1f, 0.5f) },         // Light yellow
        { 6000, new Color(1f, 0.75f, 0.25f) },     // Light orange
        { 8000, new Color(1f, 0.5f, 0.5f) },       // Light red
        { 10000, new Color(1f, 0.75f, 0.75f) },     // Light pink
        { 12000, new Color(0.9f, 0.75f, 1f) },      // Light purple
        { 14000, new Color(0.75f, 0.75f, 1f) },     // Light blue
    };
    
    private void Start()
    {
        transform.localScale *= multiplicationFactor;
        _triggeredObjects = new HashSet<Collider2D>();
        
        // Update the text
        bonusPointsMessage.text = "";
        bonusPointsText.text = "0";
        bonusPointsText.CrossFadeAlpha(0.0f, 0.0f, false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Obstacle")) return;
        if (_triggeredObjects.Contains(other)) return;

        _triggeredObjects.Add(other);

        if (gameObject.activeSelf)
        {
            StartCoroutine(CheckNearMiss(other));
        }
    }
    
    private IEnumerator CheckNearMiss(Collider2D other)
    {
        yield return new WaitForSeconds(secondsToWaitFor);
        if (!transform.parent.gameObject.activeSelf) yield break;
        LogTextChange("Near miss", scoreBonus);

        _triggeredObjects.Remove(other);
    }
    
    private IEnumerator UpdateTextWithFade(string textToDisplay, int bonusPoints, int bonusPointsTemp)
    {
        // Update the text
        bonusPointsMessage.text = textToDisplay;
        bonusPointsText.text = (bonusPointsTemp + bonusPoints).ToString();
        
        // Get the color based on the bonus points
        var textColor = Color.white;
        foreach (var threshold in _colorThresholds.Where(threshold => (bonusPointsTemp + bonusPoints) == threshold.Key))
        {
            textColor = threshold.Value;
            break;
        }

        // Set the text color
        bonusPointsMessage.color = textColor;
        bonusPointsText.color = textColor;

        // Fade in the text instantly
        bonusPointsMessage.CrossFadeAlpha(1.0f, 0.0f, false);
        bonusPointsText.CrossFadeAlpha(1.0f, 0.0f, false);
        
        // Fade out the text
        bonusPointsMessage.CrossFadeAlpha(0.0f, FadeOutTime, false);
        bonusPointsText.CrossFadeAlpha(0.0f, FadeOutTime, false);

        // Wait for the fade-out effect to complete
        yield return new WaitForSeconds(FadeOutTime);
        
        // Check if the text was fully faded out and set the value to "0"
        bonusPointsMessage.text = "";
        bonusPointsText.text = "0";
    }

    private void LogTextChange(string textToDisplay, int bonusPoints)
    {
        if (!int.TryParse(bonusPointsText.text, out var bonusPointsTemp)) return;

        // Stop the previous coroutine if it exists
        if (_textFadeCoroutine != null)
        {
            StopCoroutine(_textFadeCoroutine);
        }

        // Start the coroutine to update the text with fading effect
        _textFadeCoroutine = StartCoroutine(UpdateTextWithFade(textToDisplay, bonusPoints, bonusPointsTemp));

        // Start the coroutine to update the text with fading effect
        StartCoroutine(UpdateTextWithFade(textToDisplay, bonusPoints, bonusPointsTemp));
    }

    private void Update()
    {
        if (bonusPointsText.text == "0")
        {
            bonusPointsText.CrossFadeAlpha(0.0f, 0.0f, false);
        }
    }
    
}