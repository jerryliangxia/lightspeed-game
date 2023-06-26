using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool isThreadTheNeedle;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    // Update is called once per frame
    private void Update()
    {
        if (!GameController.Instance.isLevelingUp) return;
        
        StartCoroutine(ActivatePlayerTriggerEffect());
    }
    
    private IEnumerator ActivatePlayerTriggerEffect()
    {
        GetComponent<CircleCollider2D>().isTrigger = false;
        var obstacleRenderer = GetComponent<MeshRenderer>();
        
        while (!isThreadTheNeedle && GameController.Instance.isLevelingUp && GameController.Instance.levelIncrement < 1f)
        {
            var colorIndex = Mathf.FloorToInt(GameController.Instance.levelIncrement / Constants.ColorFlashDuration) % GameController.Instance.currentColor.Length;
            obstacleRenderer.material.SetColor(EmissionColor, GameController.Instance.currentColor[colorIndex]);
            yield return null;
        }
        obstacleRenderer.material.SetColor(EmissionColor, Color.white);
        GetComponent<CircleCollider2D>().isTrigger = true;
    }
}