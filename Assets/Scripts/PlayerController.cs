using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    private float verticalInput;
    private float horizontalInput;

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

        if (!isDead)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        if (Input.GetButton("Vertical"))
        {
            transform.Translate(Vector3.forward * moveSpeed * verticalInput * Time.deltaTime);
        }
        if (Input.GetButton("Horizontal"))
        {
            transform.Translate(Vector3.right * moveSpeed * horizontalInput * Time.deltaTime);
        }
    }
}
