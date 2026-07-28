using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float cameraSmoothingDelay;

    [SerializeField] private Vector3 cameraOffset;
    private Vector3 currentSpeed = Vector3.zero;

    [SerializeField] private float cameraPosCoordX;
    [SerializeField] private float cameraPosCoordY;
    [SerializeField] private float cameraPosCoordZ;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraOffset = new Vector3(cameraPosCoordX, cameraPosCoordY, cameraPosCoordZ);
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        // if no player object, return null so things don't break
        if (player == null)
        {
            return;
        }

        // track player position, then move camera to it through late update instead of normal update so the camera doesn't look super jittery
        Vector3 playerPosition = player.position + cameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref currentSpeed, cameraSmoothingDelay);
    }
}
