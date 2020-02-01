using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGlobals : MonoBehaviour
{
    [SerializeField] private float bonusTimeThreshold = 60;
    [SerializeField] private int pointsPerCompletion = 500;

    [SerializeField] private GameObject pointDisplay;
    [SerializeField] private GameObject timerDisplay;
    [SerializeField] private GameObject leftCompletionDisplay;
    [SerializeField] private GameObject leftTimerDisplay;
    [SerializeField] private GameObject rightCompletionDisplay;
    [SerializeField] private GameObject rightTimerDisplay;

    private int points = 0;
    private int leftCompletions = 0;
    private float leftTimer = 0;
    private int rightCompletions = 0;
    private float rightTimer = 0;
    private float elapsedTime = 0;

    public int Points { get => points; }
    public int LeftCompletions { get => leftCompletions; }
    public float LeftTimer { get => leftTimer; }
    public int RightCompletions { get => rightCompletions; }
    public float RightTimer { get => rightTimer; }
    public float Timer { get => elapsedTime; }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        leftTimer += Time.deltaTime;
        rightTimer += Time.deltaTime;
    }

    public void CompleteRight()
    {
        points += pointsPerCompletion + Mathf.RoundToInt((rightTimer / bonusTimeThreshold) * pointsPerCompletion);
        rightTimer = 0;
        rightCompletions++;
    }

    public void CompleteLeft()
    {
        points += pointsPerCompletion + Mathf.RoundToInt((leftTimer / bonusTimeThreshold) * pointsPerCompletion);
        leftTimer = 0;
        leftCompletions++;
    }
}
