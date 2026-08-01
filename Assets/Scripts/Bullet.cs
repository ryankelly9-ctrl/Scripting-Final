using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifetime;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * bulletLifetime * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
       if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
