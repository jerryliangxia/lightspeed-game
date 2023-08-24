using UnityEngine;

public class MoveTowardsSmoothDamp : MonoBehaviour
{
    public float maxMoveSpeed = 100f;
    public float smoothTime = 0.2f;
    public Vector2 regionMin = new (-20.0f, -20.0f);
    public Vector2 regionMax = new (20.0f, 20.0f);
    private Camera _mainCamera;

    private Vector2 _currentVelocity;
    private float _currentSpeed;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    private void Update()
    {
        var playerTransform = transform.position;
        // Calculate the target position based on the mouse position
        var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        // Increase the current speed of the ship based on the distance from the current position to the mouse position
        _currentSpeed = Vector2.Distance(playerTransform, mousePosition);
        // Clamp the current speed to the maximum move speed
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, maxMoveSpeed) + 100f;
        
        var targetPosition = Vector2.SmoothDamp(playerTransform, mousePosition, ref _currentVelocity, smoothTime, _currentSpeed);
    
        // Bound the target position within the specified region
        targetPosition.x = Mathf.Clamp(targetPosition.x, regionMin.x, regionMax.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, regionMin.y, regionMax.y);
    
        transform.position = targetPosition;
    }
}
