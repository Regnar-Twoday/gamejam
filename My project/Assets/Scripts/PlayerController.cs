using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float rotationSpeed = 180f;   // Degrees per second
    public float thrustForce = 5f;
    public float maxSpeed = 15f;
    public float deceleration = 0.98f;
    public float projectileSpeed = 10f;
    public GameObject projectilePrefab;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    void Update()
    {
        // Rotate
        float rotationInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.forward * -rotationInput * rotationSpeed * Time.deltaTime);

        // Thrust
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.up * thrustForce);
        }

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W))
        {
            rb.linearVelocity *= deceleration;
        }
    }

    void Shoot()
    {
        // Spawn the projectile at the ship’s position (or firePoint if assigned)
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);


    }
}
