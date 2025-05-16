using Uni
    tyEngine;

public class RandomMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3.0f;
    public bool moveIn2D = true;

    [Header("Optional: Movement Boundaries")]
    public bool useBoundaries = false;
    public Vector2 minBounds = new Vector2(-10, -5);
    public Vector2 maxBounds = new Vector2(10, 5);

    private Vector3 movementDirection;
    private float timer;

    // Add this variable to store the camera reference
    public Camera mainCamera;

    public RigidBody rb;
    void Start()
    {
        // Find and store the main camera
        mainCamera = Camera.main; // This finds the camera tagged "MainCamera"

        // It's good practice to check if the camera was actually found
        if (mainCamera == null)
        {
            Debug.LogError("RandomMover: Main Camera not found! Make sure your main camera is tagged 'MainCamera'.");
            // You might want to disable this script if the camera is essential for its logic
            // enabled = false;
            return; // Exit Start if no camera, to prevent further errors
        }
        
        
    
    }

    void Update()
    {

 
    }



}