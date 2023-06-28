using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class ThreadTheNeedleController : MonoBehaviour
{
    // Specific to ThreadTheNeedle
    public GameObject centerObject;
    public float distanceThreshold = 5f; // Adjust this threshold value as needed

    public int multiplicationFactor = 2;
    public float secondsToWaitFor = 0.5f;
    
    private Coroutine _textFadeCoroutine;
    private HashSet<Collider2D> _triggeredObjects;

    private const float FadeOutTime = 1.5f;
    public TextMeshProUGUI bonusPointsMessage;

    public Slider slider;
    public int numberOfThreads;
    
    // Define the color thresholds
    private readonly Dictionary<int, Color> _colorThresholds = new()
    {
        { 2, new Color(1f, 1f, 0.5f) },         // Light yellow
        { 3, new Color(1f, 0.75f, 0.25f) },     // Light orange
        { 4, new Color(1f, 0.5f, 0.5f) },       // Light red
        { 5, new Color(1f, 0.75f, 0.75f) },     // Light pink
        { 6, new Color(0.9f, 0.75f, 1f) },      // Light purple
        { 7, new Color(0.75f, 0.75f, 1f) },     // Light blue
    };

    private void Start()
    {
        transform.localScale *= multiplicationFactor;
        _triggeredObjects = new HashSet<Collider2D>();
        
        // Update the text
        bonusPointsMessage.text = "";
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Obstacle")) return;
        if (_triggeredObjects.Contains(other)) return;

        _triggeredObjects.Add(other);
        
        // Fixes issue of hit box being inactive when game object is destroyed
        if (transform.parent.gameObject.activeSelf)
        {
            StartCoroutine(CheckThreadTheNeedle());
        }
    }
    
    private IEnumerator CheckThreadTheNeedle()
    {
        var centerObjectPosition = centerObject.transform.position;
        
        yield return new WaitForSeconds(secondsToWaitFor);
        if (_triggeredObjects.Count >= 2)
        {
            var objectsArray = new Collider2D[_triggeredObjects.Count];
            _triggeredObjects.CopyTo(objectsArray);

            for (var i = 0; i < objectsArray.Length - 1; i++)
            {
                for (var j = i + 1; j < objectsArray.Length; j++)
                {
                    var objectA = objectsArray[i];
                    var objectB = objectsArray[j];

                    // Get the distance between objectA and objectB
                    var distanceAFromCenter = Vector2.Distance(objectA.transform.position, centerObjectPosition);
                    var distanceBFromCenter = Vector2.Distance(objectB.transform.position, centerObjectPosition);
                    
                    // Check if the distance exceeds the threshold
                    if (!(Math.Abs(distanceAFromCenter - distanceBFromCenter) > distanceThreshold)) continue;
                    
                    // Perform your actions for a successful thread-the-needle maneuver
                    if (!transform.parent.gameObject.activeSelf) yield break;
                    slider.value += 20;
                    
                    // Update the thread count
                    numberOfThreads += 1;
        
                    // Get the color based on the bonus points
                    var colorToDisplay = Color.white;
                    foreach (var threshold in _colorThresholds.Where(threshold => numberOfThreads == threshold.Key))
                    {
                        colorToDisplay = threshold.Value;
                        break;
                    }
                    LogChange(colorToDisplay);
                }
            }
        }
        _triggeredObjects.Clear();
    }

    private IEnumerator UpdateWithFade(Color colorToDisplay)
    {
        //----------------------------------------- SLIDER -----------------------------------------//
        
        // Set the slider color
        slider.fillRect.GetComponent<Image>().color = colorToDisplay;

        // Fade in the color instantly (since it's faded out from before)
        slider.fillRect.GetComponent<Image>().CrossFadeAlpha(1.0f, 0.0f, false);

        //----------------------------------------- TEXT -----------------------------------------//

        bonusPointsMessage.text = numberOfThreads > 1 ? "Thread the Needle x" + numberOfThreads : "Thread the Needle";

        // Set the text color
        bonusPointsMessage.color = colorToDisplay;
    
        // Fade in the text instantly (since it's faded out from before)
        bonusPointsMessage.CrossFadeAlpha(1.0f, 0.0f, false);
        
        // Fade out the text
        bonusPointsMessage.CrossFadeAlpha(0.0f, FadeOutTime, false);

        // Wait for the fade-out effect to complete
        yield return new WaitForSeconds(FadeOutTime);

        // Reset the slider color
        slider.fillRect.GetComponent<Image>().color = Color.white;
        
        // Check if the text was fully faded out and set the value to "0"
        bonusPointsMessage.text = "";
        numberOfThreads = 0;
    }

    private void LogChange(Color colorToDisplay)
    {
        // Stop the previous coroutine if it exists
        if (_textFadeCoroutine != null)
        {
            StopCoroutine(_textFadeCoroutine);
        }
        
        // Start the coroutine to update the text with fading effect
        _textFadeCoroutine = StartCoroutine(UpdateWithFade(colorToDisplay));

        // Start the coroutine to update the text with fading effect
        StartCoroutine(UpdateWithFade(colorToDisplay));
    }
}
