using System;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ScoreObject")]
public class ScoreObject : ScriptableObject
{
    public TextMeshProUGUI score;

    public event Action onScore;
    public int myScore = 0;
    
    public void Reset()
    {
        myScore = 0;
        onScore?.Invoke();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void AddToScore()
    {
        myScore++;
        onScore?.Invoke();
    }
}
