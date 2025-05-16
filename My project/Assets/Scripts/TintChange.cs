using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class TintChange : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Color red = Color.red;
    public Color blue = Color.blue;
    public Color yellow = Color.yellow;
    public Color green = Color.green;

    public float interval;
    public float alpha;
    private int current = 0;

    public List<Color> colors = new();


    private SpriteRenderer spriteRenderer;
    private float timeElapsed;

    void Start()
    {
        colors.Add(red);
        colors.Add(blue);
        colors.Add(yellow);
        colors.Add(green);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;  // Increase time

        // If time elapsed is greater than the interval, toggle the color
        if (timeElapsed >= interval)
        {
            spriteRenderer.color = new Color(colors[current].r, colors[current].g, colors[current].b, alpha);

            current += 1;

            current = current % colors.Count;

            // Reset timer
            timeElapsed = 0f;
        }
    }
}
