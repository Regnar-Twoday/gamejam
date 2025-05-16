using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab; // Assign your Enemy prefab here
    public float spawnMargin = 2.0f; // How far outside the camera view to spawn
    public float spawnInterval = 0.5f; // Time between spawns, 0 for instant spawn

    [Header("Targeting")] // This helps organize the Inspector
    public Transform playerTransform;

    [Header("Powerups")]
    public GameObject speed; 
    public GameObject reverse; 
    public GameObject shield; 
    public GameObject gun; 
    
    public List<GameObject> powerups = new List<GameObject>(); 

    [Header("Camera")]
    public Camera mainCamera; // Assign your main camera

    private float minX, maxX, minY, maxY;
    

    void Start()
    {
        powerups.Add(speed);
        powerups.Add(reverse);
        powerups.Add(shield);
        powerups.Add(gun);
        
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("EnemySpawner: Main Camera not found and not assigned! Disabling spawner.", this);
                enabled = false; // Disable script if no camera
                return;
            }
        }

        if (enemyPrefab == null)
        {
            Debug.LogError("EnemySpawner: Enemy Prefab not assigned! Disabling spawner.", this);
            enabled = false;
            return;
        }

        // Find player if not assigned in Inspector
        if (playerTransform == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerTransform = playerObject.transform;
                Debug.Log("EnemySpawner: Player found by tag 'Player'.", this);
            }
            else
            {
                // It's okay if the player isn't found; the Enemy script has a fallback.
                // However, the "aim towards player" feature won't work without it.
                Debug.LogWarning("EnemySpawner: Player Transform not assigned in Inspector and could not be found by tag 'Player'. Enemies will use fully random direction or their own fallback.", this);
            }
        }

        CalculateCameraBoundaries();

        if (spawnInterval > 0)
        {
            StartCoroutine(SpawnEnemiesWithDelay());
        }
        else
        {
            SpawnAllEnemiesAtOnce();
        }
        
        StartCoroutine(SpawnPowerups());
    }

    // Update can be removed if not used
    // void Update()
    // {
    // }

    void CalculateCameraBoundaries()
    {
        if (mainCamera.orthographic) // For Orthographic Camera (2D)
        {
            float camHeight = mainCamera.orthographicSize * 2;
            float camWidth = camHeight * mainCamera.aspect;

            minX = mainCamera.transform.position.x - camWidth / 2;
            maxX = mainCamera.transform.position.x + camWidth / 2;
            minY = mainCamera.transform.position.y - camHeight / 2;
            maxY = mainCamera.transform.position.y + camHeight / 2;
        }
        else // For Perspective Camera (3D)
        {
            float spawnPlaneDistance = 10f; // Default distance
            // A more robust way to determine distance for perspective:
            // Consider the plane your enemies should spawn on.
            // If player exists, maybe spawn them at player's Z depth?
            // For now, we use a fixed distance or camera's z (if negative, looking towards origin)
            if (playerTransform != null) {
                // Attempt to use a plane slightly in front of the camera, at player's Z if reasonable
                // This is still a simplification for true frustum spawning
                Plane gamePlane = new Plane(Vector3.forward, new Vector3(0,0, playerTransform.position.z));
                Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Center of screen
                float distanceToPlane;
                if (gamePlane.Raycast(ray, out distanceToPlane))
                {
                    spawnPlaneDistance = distanceToPlane;
                } else if (mainCamera.transform.position.z < 0) {
                     spawnPlaneDistance = Mathf.Abs(mainCamera.transform.position.z);
                }
            } else if (mainCamera.transform.position.z < 0) {
                 spawnPlaneDistance = Mathf.Abs(mainCamera.transform.position.z);
            }


            Vector3 bottomLeftScreen = new Vector3(0, 0, spawnPlaneDistance);
            Vector3 topRightScreen = new Vector3(1, 1, spawnPlaneDistance); // Use viewport coords for perspective

            Vector3 bottomLeftWorld = mainCamera.ViewportToWorldPoint(bottomLeftScreen);
            Vector3 topRightWorld = mainCamera.ViewportToWorldPoint(topRightScreen);

            minX = bottomLeftWorld.x;
            maxX = topRightWorld.x;
            minY = bottomLeftWorld.y;
            maxY = topRightWorld.y;
        }
    }
    

    IEnumerator SpawnEnemiesWithDelay()
    {
        while (true)
        {
            SpawnSingleEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnAllEnemiesAtOnce()
    {
        while (true)
        {
            SpawnSingleEnemy();
        }
    }

    void SpawnSingleEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Cannot spawn enemy, prefab is null.", this);
            return;
        }

        Vector3 spawnPosition = GetRandomSpawnPositionOutsideBounds();
        GameObject newEnemyGO = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        spawnInterval *= 0.98f;

        Enemy enemyScript = newEnemyGO.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            // Pass the player reference if it exists (it might be null if not found/assigned)
            enemyScript.playerTarget = this.playerTransform;
        }
        else
        {
            Debug.LogWarning("Spawned enemy prefab does not have an Enemy script component attached!", newEnemyGO);
        }
    }

    IEnumerator SpawnPowerups()
    {
        while (true)
        {
            createPowerups();
        
            yield return new WaitForSeconds(5);
        }
    }
    void createPowerups()
    {
        if (powerups == null)
        {
            Debug.LogError("Cannot spawn powerups, prefab is null.", this);
            return;
        }

        Vector3 spawnPosition = GetRandomSpawnPositionOutsideBounds();
        Instantiate(powerups[Random.Range(0, 4)], spawnPosition, Quaternion.identity);
        spawnInterval *= 0.98f;
    }

    Vector3 GetRandomSpawnPositionOutsideBounds()
    {
        Vector3 spawnPos = Vector3.zero;
        int side = Random.Range(0, 4); // 0: Top, 1: Bottom, 2: Left, 3: Right

        // Using the pre-calculated boundaries
        // The spawnMargin ensures they are outside the direct view
        // The inner range for the random coordinate ensures they spawn along the edge, not just corners

        switch (side)
        {
            case 0: // Top: Y is fixed above maxY + margin, X is random within camera view width
                spawnPos = new Vector3(Random.Range(minX, maxX), maxY + spawnMargin, 0);
                break;
            case 1: // Bottom: Y is fixed below minY - margin, X is random within camera view width
                spawnPos = new Vector3(Random.Range(minX, maxX), minY - spawnMargin, 0);
                break;
            case 2: // Left: X is fixed left of minX - margin, Y is random within camera view height
                spawnPos = new Vector3(minX - spawnMargin, Random.Range(minY, maxY), 0);
                break;
            case 3: // Right: X is fixed right of maxX + margin, Y is random within camera view height
                spawnPos = new Vector3(maxX + spawnMargin, Random.Range(minY, maxY), 0);
                break;
        }
        return spawnPos;
    }
    }
}
