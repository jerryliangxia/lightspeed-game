using UnityEngine;

public class AlignParticles : MonoBehaviour
{
    public Transform centerObject; // Reference to the center object
    public GameObject speedEffect;
    private ParticleSystem _particleSystem; // Reference to the Particle System component
    private ParticleSystem.ShapeModule _shapeModule; // Reference to the Shape Module of the Particle System
    
    // Public variables
    public float minEmissionRate = 5f; // Minimum emission rate of the Particle System
    public float maxEmissionRate = 75f; // Maximum emission rate of the Particle System
    public float maxDistance = 10f;     // Maximum distance at which the emission rate is at the minimum value

    private void Start()
    {
        _particleSystem = speedEffect.GetComponent<ParticleSystem>();
        _shapeModule = _particleSystem.shape;
    }

    private void LateUpdate()
    {
        var centerObjectPosition = centerObject.position;
        var playerTransformPosition = transform.position;
        
        // Calculate the distance between the player and the center
        var distance = Vector2.Distance(playerTransformPosition, centerObjectPosition);

        // Calculate the emission rate based on the distance
        var emissionRate = Mathf.Lerp(maxEmissionRate, minEmissionRate, distance / maxDistance);

        // Set the emission rate of the Particle System
        var emission = _particleSystem.emission;
        emission.rateOverTime = emissionRate;

        // Calculate the scale based on the distance
        var shapeScaleY = Mathf.Lerp(0.5f, 4f, distance / maxDistance);

        // Set the Y-scale of the particle system's shape
        _shapeModule.scale = new Vector3(_shapeModule.scale.x, shapeScaleY, _shapeModule.scale.z);
    }
    
}
