using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    [SerializeField] CircleCollider2D circleCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid")) {
            gameObject.SetActive(false);
        }
    }
}
