using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class PlayerController : MonoBehaviour
{
    [Header("Rigidbody and Movement")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private float noMovement = 0.0f;

    [Header("Firing")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletDelay = 3.0f;
    private float bulletDelayReset = 3.0f;
    private float bulletDelayCountdown = 0.1f;
    private float bulletDelayGoal = 0.0f;
    [SerializeField] private Transform fireOrigin;

    [Header("Input")]
    private float verticalInput;
    private float horizontalInput;

    [Header("Components and Dependencies")]
    public Camera PlayerCamera;
    public LayerMask defaultLayer;
    private float lookDirectionY = 0f;
    public Animator PlayerAnimator;
    private bool isRunning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameManager._GameManager.IsDead = false;
        GameManager._GameManager.CurrentPlayerHealth = GameManager._GameManager.StartingPlayerHealth;
        GameManager._GameManager.CurrentKillCount = GameManager._GameManager.StartingKillCount;
        isRunning = true;
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
            if (bulletDelay > bulletDelayGoal)
            {
                bulletDelay -= bulletDelayCountdown;
            }
            HandleFiring();
        }
    }

    private void HandleMovement()
    {
        float HorizontalMove = Input.GetAxis("Horizontal");
        float VerticalMove = Input.GetAxis("Vertical");

        Vector3 playerMovement = new Vector3(HorizontalMove, noMovement, VerticalMove);
        transform.Translate(playerMovement * moveSpeed * Time.deltaTime, Space.World);

        if (HorizontalMove != noMovement || VerticalMove != noMovement)
        {
            PlayerAnimator.SetBool("IsRunning", isRunning);
        }
        else
        {
            PlayerAnimator.SetBool("IsRunning", !isRunning);
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
        if (Input.GetButton("Fire1") && bulletDelay <= bulletDelayGoal)
        {
            BulletFire();
            bulletDelay = bulletDelayReset;
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
            GameManager._GameManager.EnemyHitByPlayer();
            Destroy(collision.gameObject);
        }
    }
}
