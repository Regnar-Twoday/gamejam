using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float rotationSpeed = 180f;   // Degrees per second
    public float thrustForce = 5f;
    public float maxSpeed = 15f;
    public float deceleration = 0.98f;
    public float projectileSpeed = 10f;
    public float shieldLifetime = 8f;
    public GameObject projectilePrefab;
    public GameObject shield;

    [SerializeField] PolygonCollider2D collider;

    private Rigidbody2D rb;


    private Camera mainCam;
    private float halfWidth;
    private float halfHeight;

    void Start()
    {
        mainCam = Camera.main;

        // Get camera bounds in world units
        halfHeight = mainCam.orthographicSize;
        halfWidth = halfHeight * mainCam.aspect;

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        if (pos.x > halfWidth)
            pos.x = -halfWidth;
        else if (pos.x < -halfWidth)
            pos.x = halfWidth;

        if (pos.y > halfHeight)
            pos.y = -halfHeight;
        else if (pos.y < -halfHeight)
            pos.y = halfHeight;

        transform.position = pos;

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Asteroid"))
        {
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Shield"))
        {
            shield.SetActive(true);
            StartCoroutine(ShieldLifeTime(shieldLifetime * Time.timeScale));
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("SpeedUp"))
        {
            Time.timeScale = 2.5f;
            StartCoroutine(SpeedUp(shieldLifetime * Time.timeScale));
            Destroy(other.gameObject);
        }
    }

    public IEnumerator SpeedUp(float lifestime)
    {
        yield return new WaitForSeconds(lifestime);
        Time.timeScale = 1f;
    }

    public IEnumerator ShieldLifeTime(float lifestime)
    {
        yield return new WaitForSeconds(lifestime);
        shield.SetActive(false);
    }
}
