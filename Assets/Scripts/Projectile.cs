using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private static readonly int CosmicEmissionColor = Shader.PropertyToID("_EmissionColor");

    // Update is called once per frame
    private void Update()
    {
        if (!GameController.Instance.isLevelingUp) return;
        
        StartCoroutine(ActivatePlayerTriggerEffect());
    }
    
    private IEnumerator ActivatePlayerTriggerEffect()
    {
        var obstacleRenderer = GetComponent<MeshRenderer>();
        
        while (GameController.Instance.isLevelingUp && GameController.Instance.levelIncrement < 1f)
        {
            var colorIndex = Mathf.FloorToInt(GameController.Instance.levelIncrement / Constants.ColorFlashDuration) % GameController.Instance.currentColor.Length;
            obstacleRenderer.material.SetColor(CosmicEmissionColor, GameController.Instance.currentColor[colorIndex]);
            yield return null;
        }
        obstacleRenderer.material.SetColor(CosmicEmissionColor, Color.white);
    }
}