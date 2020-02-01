using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeField] private WorkingBench leftBench;
    [SerializeField] private WorkingBench rightBench;

    private int points = 0;
    private int leftCompletions = 0;
    private float leftTimer = 0;
    private int rightCompletions = 0;
    private float rightTimer = 0;
    private float elapsedTime = 0;

    private TextMeshProUGUI timerDisplayTextElement;
    private TextMeshProUGUI pointDisplayTextElement;
    private TextMeshProUGUI leftTimerDisplayTextElement;
    private TextMeshProUGUI leftCountDisplayTextElement;
    private TextMeshProUGUI rightTimerDisplayTextElement;
    private TextMeshProUGUI rightCountDisplayTextElement;

    public int Points { get => points; }
    public int LeftCompletions { get => leftCompletions; }
    public float LeftTimer { get => leftTimer; }
    public int RightCompletions { get => rightCompletions; }
    public float RightTimer { get => rightTimer; }
    public float Timer { get => elapsedTime; }

    private void Awake()
    {
        timerDisplayTextElement = timerDisplay.GetComponent<TextMeshProUGUI>();
        pointDisplayTextElement = pointDisplay.GetComponent<TextMeshProUGUI>();
        leftTimerDisplayTextElement = leftTimerDisplay.GetComponent<TextMeshProUGUI>();
        leftCountDisplayTextElement = leftCompletionDisplay.GetComponent<TextMeshProUGUI>();
        rightTimerDisplayTextElement = rightTimerDisplay.GetComponent<TextMeshProUGUI>();
        rightCountDisplayTextElement = rightCompletionDisplay.GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {

        leftBench.OnBodyCompleted += CompleteLeft;
        rightBench.OnBodyCompleted += CompleteRight;
    }

    void OnDisable()
    {

        leftBench.OnBodyCompleted -= CompleteLeft;
        rightBench.OnBodyCompleted -= CompleteRight;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        leftTimer += Time.deltaTime;
        rightTimer += Time.deltaTime;

        //Update all displays
        timerDisplayTextElement.text = string.Format("{0:0.0}", elapsedTime);
        pointDisplayTextElement.text = points.ToString();
        leftTimerDisplayTextElement.text = string.Format("{0:0.0}", leftTimer);
        rightTimerDisplayTextElement.text = string.Format("{0:0.0}", rightTimer);
        leftCountDisplayTextElement.text = leftCompletions.ToString();
        rightCountDisplayTextElement.text = rightCompletions.ToString();
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
