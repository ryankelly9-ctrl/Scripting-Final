using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    private float verticalInput;
    private float horizontalInput;

    public Camera playerCamera;
    public LayerMask defaultLayer;
    private float lookDirectionY = 0f;

    public bool isDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        // If NOT dead, can move and stuff
        if (!isDead)
        {
            HandleMovement();
            HandleRotation();
        }
    }

    private void HandleMovement()
    {
        if (Input.GetButton("Vertical"))
        {
            transform.Translate(Vector3.forward * moveSpeed * verticalInput * Time.deltaTime, Space.World);
        }
        if (Input.GetButton("Horizontal"))
        {
            transform.Translate(Vector3.right * moveSpeed * horizontalInput * Time.deltaTime, Space.World);
        }
    }

    private void HandleRotation()
    {
        Ray rayCast = playerCamera.ScreenPointToRay(Input.mousePosition);

        // Raycast to check where the mouse position is in the world then rotate the player towards it.
        if (Physics.Raycast(rayCast, out RaycastHit hit, Mathf.Infinity, defaultLayer))
        {
            Vector3 direction = hit.point - transform.position;
            direction.y = lookDirectionY;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
}
