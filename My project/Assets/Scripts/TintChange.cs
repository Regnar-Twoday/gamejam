using UnityEngine;

public class TintChange : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Color red = Color.red;
    public Color blue = Color.blue;
    public Color yellow = Color.yellow;
    public Color green = Color.green;

    private SpriteRenderer spriteRenderer;
    private float timeElapsed;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
