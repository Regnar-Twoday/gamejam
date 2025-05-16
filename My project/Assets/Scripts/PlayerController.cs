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
    public float speedUpTime = 8f;
    public float reverseTime = 10f;
    public float doubleGunTime = 10f;
    public bool reverse = false;
    public bool doubleGun = false;
    public GameObject projectilePrefab;
    public GameObject shield;
    public BackgroundScript background;

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
        if (reverse)
        {
            transform.Rotate(Vector3.forward * rotationInput * rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.forward * -rotationInput * rotationSpeed * Time.deltaTime);
        }

        // Thrust
        if (reverse)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Shoot();
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shoot();
            }
        }
    }

    void FixedUpdate()
    {
        if (reverse)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                rb.AddForce(-transform.up * thrustForce);
            }

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }

            if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W))
            {
                rb.linearVelocity *= deceleration;
            }

            return;
        }

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
        if (doubleGun)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            GameObject projectile2 = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectile.transform.Rotate(0f, 0f, 10);
            projectile2.transform.Rotate(0f, 0f, -10);
        }
        else
        {
            Instantiate(projectilePrefab, transform.position, transform.rotation);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.CompareTag("Asteroid"))
        //{
        //    Destroy(gameObject);
        //}

        if (other.gameObject.CompareTag("Shield"))
        {
            shield.SetActive(true);
            StartCoroutine(ShieldLifeTime(shieldLifetime));
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("SpeedUp"))
        {
            Time.timeScale = 2.5f;
            background.Party = true;
            StartCoroutine(SpeedUp(speedUpTime * Time.timeScale));
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Reverse"))
        {
            reverse = true;
            StartCoroutine(Reverse(reverseTime));
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("DoubleGun"))
        {
            doubleGun = true;
            StartCoroutine(Reverse(doubleGunTime));
            Destroy(other.gameObject);
        }
    }

    public IEnumerator SpeedUp(float lifestime)
    {
        yield return new WaitForSeconds(lifestime);
        Time.timeScale = 1f;
        background.Party = false;
    }

    public IEnumerator ShieldLifeTime(float lifestime)
    {
        yield return new WaitForSeconds(lifestime);
        shield.SetActive(false);
    }

    public IEnumerator Reverse(float lifestime)
    {
        yield return new WaitForSeconds(lifestime);
        reverse = false;
    }

    public IEnumerator Doublegun(float lifestime)
    {
        yield return new WaitForSeconds(lifestime);
        doubleGun = false;
    }
}
