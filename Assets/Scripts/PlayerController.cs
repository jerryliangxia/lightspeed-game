using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Singleton instance
    public static PlayerController Instance { get; private set; }
    
    // Position and Rotation Parameters
    public Transform centerObject; // Reference to the center object
    public float minSize = 0.05f; // Minimum size of the player
    public float maxSize = 6f; // Maximum size of the player
    public float maxDistance = 40f; // Maximum distance at which the player is at the minimum size

    // Explosions
    public GameObject explosionEffect;
    private Coroutine _explosionCoroutine;
    private static readonly int CosmicEmissionColor = Shader.PropertyToID("_EmissionColor");

    public void Awake()
    {
        // Set the instance to this object (used for explosion prefab set to white)
        PlayerController save = null;
        if (Instance != null && Instance != this)
        {
            save = Instance;
        }
        Instance = this;
        
        Destroy(save);
    }
    private void Start()
    {
        // Set the explosion color to white (base color)
        var pem = explosionEffect.GetComponent<Renderer>().sharedMaterial;
        pem.SetColor(CosmicEmissionColor, Color.white);
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

    // Handle death of player
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Set the explosion color to current colors
        if (GameController.Instance.isLevelingUp)
        {
            var pemBase = explosionEffect.GetComponent<Renderer>().sharedMaterial;
            pemBase.SetColor(CosmicEmissionColor, GameController.Instance.currentColor[0]); 
        }

        // Instantiate the explosion effect at the collision position
        Instantiate(explosionEffect, gameObject.transform.position, Quaternion.identity);
        
        // Play the "Hand Gun 1" sound effect
        GameController.Instance.PlayExplosion();
    
        // Set game object active to false
        gameObject.SetActive(false);
    
        // Show the restart UI prompt (you can implement this part separately)
        GameController.Instance.GameOver();
    }
}

