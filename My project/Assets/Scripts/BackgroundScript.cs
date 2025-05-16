using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public float rotationSpeed = 5.0f;
    public float scaleAmount = 0.5f;  // How much to scale up/down
    public float speed = 2f;          // How fast the scale changes
    public Vector3 baseScale = Vector3.one;
    public bool Party = false;

    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        if (Party)
        {
            float scale = 1 + Mathf.Sin(Time.time) * scaleAmount;
            transform.localScale = baseScale * scale;
        }
    }
}
