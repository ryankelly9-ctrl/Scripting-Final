using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletDelay = 3.0f;
    [SerializeField] private Transform fireOrigin;

    private float verticalInput;
    private float horizontalInput;

    public Camera PlayerCamera;
    public LayerMask defaultLayer;
    private float lookDirectionY = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameManager._GameManager.IsDead = false;
        GameManager._GameManager.CurrentPlayerHealth = GameManager._GameManager.StartingPlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        // If NOT dead, can move and stuff
        if (!GameManager._GameManager.IsDead)
        {
            HandleMovement();
            HandleRotation();
            bulletDelay -= 0.1f;
            HandleFiring();
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
        Ray rayCast = PlayerCamera.ScreenPointToRay(Input.mousePosition);

        // Raycast to check where the mouse position is in the world then rotate the player towards it.
        if (Physics.Raycast(rayCast, out RaycastHit hit, Mathf.Infinity, defaultLayer))
        {
            Vector3 direction = hit.point - transform.position;
            direction.y = lookDirectionY;

            // Checking the mouse direction and turning the player object to face it.
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    // Fire Input
    private void HandleFiring()
    {
        if (Input.GetButton("Fire1") && bulletDelay <= 0.0f)
        {
            BulletFire();
            bulletDelay = 3.0f;
        }
    }

    // Bullet Instantiation
    private void BulletFire()
    {
        if (bulletPrefab == null)
        {
            return;
        }
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, fireOrigin.position, fireOrigin.rotation);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            GameManager._GameManager.PlayerHitByEnemy();
            Destroy(collision.gameObject);
        }
    }
}
