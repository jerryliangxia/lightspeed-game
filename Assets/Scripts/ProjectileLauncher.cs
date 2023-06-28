using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    // Projectile array
    public List<GameObject> projectiles;

    // Projectile itself
    public GameObject projectilePrefab; // Prefab for the projectile
    public int projectileCount = 50; // Amount of projectiles
    public int projectileTargetSpeedMultiplier = 4; // For growing the projectile
    
    // Speeds, distance and rate
    public float minSpeed = 5.0f;
    public float maxSpeed = 15.0f;
    public int maxDistance = 100;
    private float _emissionTimer; // Rate in which projectiles are launched. Goes from 0.4 to 0.1 over 8 level ups

    private int _index; // For projectile array (re-use)
    private const int Delay = 5; // For amount of time projectile exists before death
    
    // Rendering
    private static readonly int CosmicEmissionColor = Shader.PropertyToID("_EmissionColor");

    private void Start()
    {
        for (var i = 0; i < projectileCount; i++)
        {
            // Instantiate the projectile prefab
            var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Randomize the size of the projectile
            projectile.transform.localScale = new Vector3(0f, 0f, 1f); // Start with size 0
            projectile.SetActive(false); // Set the projectile as inactive initially
            projectiles.Add(projectile);
        }
    }

    private void Update()
    {
        _emissionTimer += Time.deltaTime;

        if (_emissionTimer >= GameController.Instance.emissionRate)
        {
            // Reset timer
            _emissionTimer = 0f;
            
            // Reset index counter
            _index += 1;
            if (_index == projectileCount) _index = 0;
    
            // Pull from projectile list
            var projectile = projectiles[_index];
            projectile.transform.position = Vector3.zero;
            projectile.SetActive(true);
    
            StartCoroutine(DeactivateDelayed(projectile));
    
            // Generate a random direction vector
            var direction = Random.insideUnitCircle.normalized;
    
            // Generate a random speed for the projectile
            var speed = Random.Range(minSpeed, maxSpeed);
    
            // Set the initial velocity of the projectile to move in the random direction with the random speed
            projectile.GetComponent<Rigidbody2D>().velocity = direction * speed;
            
            StartCoroutine(GrowProjectile(projectile));
        }
    }
    
    private static IEnumerator DeactivateDelayed(GameObject projectile)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(Delay);

        // Deactivate the game object
        projectile.GetComponent<MeshRenderer>().material.SetColor(CosmicEmissionColor, Color.white);
        projectile.SetActive(false);
    }
    
    // Makes the projectile grow from the center; the closer to the center the smaller the size
    private IEnumerator GrowProjectile(GameObject projectile)
    {
        var initialScale = projectile.transform.localScale;
        var targetScale = new Vector3(2f, 2f, 2f); // Target size of the projectile when it is far from the center

        var initialSpeed = projectile.GetComponent<Rigidbody2D>().velocity.magnitude;
        var targetSpeed = projectileTargetSpeedMultiplier * initialSpeed; // Target speed of the projectile when it is far from the center

        while (true)
        {
            // Calculate the distance between the projectile and the centerObject
            var distance = Vector3.Distance(projectile.transform.position, transform.position);

            // Calculate the size based on the distance
            var size = Mathf.Lerp(initialScale.x, targetScale.x, distance / maxDistance);

            // Set the scale of the projectile
            projectile.transform.localScale = new Vector3(size, size, size);

            // Calculate the speed based on the distance
            var speed = Mathf.Lerp(initialSpeed, targetSpeed, distance / maxDistance);

            // Set the velocity of the projectile to move in the current direction with the calculated speed
            projectile.GetComponent<Rigidbody2D>().velocity = projectile.GetComponent<Rigidbody2D>().velocity.normalized * speed;

            yield return null;
        }
    }
    
}
