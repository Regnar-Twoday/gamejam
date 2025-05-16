using System;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    public TextMeshProUGUI score;
    private void OnEnable()
    {
        scoreObject.onScore += UpdateScoreText;
    }

    private void OnDisable()
    {
        scoreObject.onScore -= UpdateScoreText;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ScoreObject scoreObject;

    public void UpdateScoreText()
    {
        score.text = $"Score: {scoreObject.score}";
    }
}
