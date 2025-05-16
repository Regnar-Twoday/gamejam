using System.Collections;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3.0f;
    public bool moveIn2D = true;
    public float lifetime = 20f;
    

    [Header("Targeting Settings")] 
    public Transform playerTarget; 
    public float aimSpreadAngle = 45.0f; 

    [Header("Optional: Orientation")]
    public bool orientToMoveDirection = true;
    
    private Vector3 movementDirection;
    
    
    void Start()
    {

        
        if (playerTarget == null)
        {
            
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerTarget = playerObject.transform;
                Debug.Log(gameObject.name + ": Found player by tag in Start().", this);
            }
            else
            {
                Debug.LogWarning(gameObject.name + ": Player target not set and could not be found by tag 'Player'. Using fully random direction.", this);
            }
        }
        else
        {
            Debug.Log(gameObject.name + ": Player target was already set (likely by spawner). Player: " + playerTarget.name, this);
        }


        SetInitialRandomDirection();
        
        StartCoroutine(astroidLifeTime(lifetime));
    }

    void SetInitialRandomDirection()
    {
        Vector3 directionToPlayer = Vector3.zero;
        bool playerExistsAndIsDifferentPosition = false;

        if (playerTarget != null)
        {
            
            Vector3 tempDirectionToPlayer = playerTarget.position - transform.position;
            
            if (tempDirectionToPlayer.sqrMagnitude > 0.001f)
            {
                playerExistsAndIsDifferentPosition = true;
                directionToPlayer = tempDirectionToPlayer;
                if (moveIn2D)
                {
                    directionToPlayer.z = 0;
                }
                directionToPlayer.Normalize();
            }
            else
            {
                 Debug.LogWarning(gameObject.name + ": Enemy spawned on top of player or playerTarget is self. Using random direction.", this);
            }
        }

        if (playerExistsAndIsDifferentPosition)
        {
            Debug.Log(gameObject.name + ": Player target is valid. Calculating biased direction. Spread: " + aimSpreadAngle, this);
            if (moveIn2D)
            {
                // Get the angle of directionToPlayer
                float angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
                // Add random spread
                float randomSpread = Random.Range(-aimSpreadAngle / 2f, aimSpreadAngle / 2f);
                float finalAngle = angleToPlayer + randomSpread;
                
                movementDirection = new Vector3(Mathf.Cos(finalAngle * Mathf.Deg2Rad), Mathf.Sin(finalAngle * Mathf.Deg2Rad), 0f);
            }

        }
        else
        {
            Debug.Log(gameObject.name + ": Player target not valid or not found. Using fully random direction.", this);
            if (moveIn2D)
            {
                float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                movementDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            }
            else
            {
                movementDirection = Random.onUnitSphere;
            }
        }
        
        if (movementDirection.sqrMagnitude < 0.001f)
        {
            movementDirection = moveIn2D ? Vector3.right : Vector3.forward; 
             Debug.LogWarning(gameObject.name + ": movementDirection was zero, defaulted to " + movementDirection, this);
        }
        movementDirection.Normalize();

        
        if (orientToMoveDirection && movementDirection != Vector3.zero)
        {
            if (moveIn2D)
            {
                float angleDegrees = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angleDegrees - 90f); // Assumes sprite 'up' is forward
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(movementDirection); // Assumes object 'forward' (Z) is forward
            }
        }
    }

    void Update()
    {
        transform.position += movementDirection * moveSpeed * Time.deltaTime;
    }

    public IEnumerator astroidLifeTime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        if (this != null && gameObject != null) // Check if object hasn't been destroyed by other means
        {
            Destroy(gameObject);
        }
    }
}