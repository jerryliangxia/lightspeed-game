using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class PlayerController : MonoBehaviour
{
    // Position and Rotation Parameters
    public Transform centerObject; // Reference to the center object
    public float minSize = 0.05f; // Minimum size of the player
    public float maxSize = 6f; // Maximum size of the player
    public float maxDistance = 40f; // Maximum distance at which the player is at the minimum size

    // Explosions
    public GameObject explosionEffect;
    private Coroutine _explosionCoroutine;

    // Audio/Sfx
    public AudioSource source;
    public AudioClip explosionSfx;

    private void Start()
    {
        // Sfx
        source.clip = explosionSfx;
    }

    private void Update()
    {
        var playerTransform = transform;
        var centerObjectPosition = centerObject.position;
        var playerTransformPosition = playerTransform.position;
        var playerLocalScale = playerTransform.localScale;

        // Calculate the distance between the player and the center
        var distance = Vector3.Distance(playerTransformPosition, centerObjectPosition);

        // Calculate the size based on the distance
        var size = Mathf.Lerp(minSize, maxSize, distance / maxDistance);

        // Set the scale of the player object
        transform.localScale = new Vector3(size, playerLocalScale.y, playerLocalScale.z);
        
        var direction = centerObjectPosition - playerTransformPosition;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the Particle System to match the calculated angle
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Instantiate the explosion effect at the collision position
        Instantiate(explosionEffect, gameObject.transform.position, Quaternion.identity);
        
        print("From Explosion: " + GameController.Instance.isLevelingUp);
        
        // Play the "Hand Gun 1" sound effect
        MusicClass.PlayExplosion();
    
        // Set game object active to false
        gameObject.SetActive(false);
    
        // Show the restart UI prompt (you can implement this part separately)
        GameController.Instance.GameOver();
    }
}

