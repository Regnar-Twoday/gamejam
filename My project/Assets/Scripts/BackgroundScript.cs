using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public float rotationSpeed = 5.0f;

    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
