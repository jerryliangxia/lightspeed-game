using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Transform centerObject; // Reference to the center object's transform
    public float followSpeed = 3f; // Speed at which the camera follows the player
    public float minZoom = 3f; // Minimum zoom level
    public float maxZoom = 11f; // Maximum zoom level
    public float zoomSpeed = 3f; // Speed at which the camera zooms
    public float maxZoomDistance = 20f; // Maximum distance for zooming

    private Camera _mainCamera; // Reference to the camera component

    private void Start()
    {
        _mainCamera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        var targetPosition = CalculateTargetPosition();
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        var targetZoom = CalculateTargetZoom();
        _mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
    }

    private Vector3 CalculateTargetPosition()
    {
        // Calculate the position between the player and the center object
        var centerPosition = centerObject.position;
        var playerPosition = player.position;
        var targetPosition = (playerPosition + centerPosition) / 2f;

        // Add an offset to slightly move towards the player
        var offset = playerPosition - centerPosition;
        targetPosition += offset.normalized;

        // Keep the camera at its current z position
        targetPosition.z = transform.position.z;

        return targetPosition;
    }

    private float CalculateTargetZoom()
    {
        // Calculate the distance between the player and the center object
        var distance = Vector3.Distance(player.position, centerObject.position);

        // Map the distance between the desired zoom levels
        var targetZoom = Mathf.Lerp(minZoom, maxZoom, distance / maxZoomDistance);

        return targetZoom;
    }
}
