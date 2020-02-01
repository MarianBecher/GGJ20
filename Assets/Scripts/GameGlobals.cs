using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameGlobals : MonoBehaviour
{
    [Header("Balancing")]
    [SerializeField] private float bonusTimeThreshold = 60;
    [SerializeField] private int pointsPerCompletion = 500;
    [SerializeField] private int maxMobProgress = 300;
    [SerializeField] private int monsterStrengthAddition = 60;

    [Header("Referenzen")]
    [SerializeField] private GameObject pointDisplay;
    [SerializeField] private GameObject timerDisplay;
    [SerializeField] private GameObject leftCompletionDisplay;
    [SerializeField] private GameObject rightCompletionDisplay;
    [SerializeField] private GameObject scoreScreenDisplay;
    [SerializeField] private GameObject victoryDisplay;
    [SerializeField] private Image mobProgressDisplay;
    [SerializeField] private Image monsterStrengthDisplay;
    [SerializeField] private WorkingBench leftBench;
    [SerializeField] private WorkingBench rightBench;

    private int points = 0;
    private int leftCompletions = 0;
    private float leftTimer = 0;
    private int rightCompletions = 0;
    private float rightTimer = 0;
    private float elapsedTime = 0;
    private float mobProgress = 150;
    private float monsterStrength = 150;

    private TextMeshProUGUI timerDisplayTextElement;
    private TextMeshProUGUI pointDisplayTextElement;
    private TextMeshProUGUI leftCountDisplayTextElement;
    private TextMeshProUGUI rightCountDisplayTextElement;
    private TextMeshProUGUI victoryDisplayTextElement;

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
        leftCountDisplayTextElement = leftCompletionDisplay.GetComponent<TextMeshProUGUI>();
        rightCountDisplayTextElement = rightCompletionDisplay.GetComponent<TextMeshProUGUI>();
        victoryDisplayTextElement = victoryDisplay.GetComponent<TextMeshProUGUI>();
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
        monsterStrength -= Time.deltaTime;
        mobProgress += Time.deltaTime;

        //Update all displays
        mobProgressDisplay.fillAmount = mobProgress / maxMobProgress;
        monsterStrengthDisplay.fillAmount = monsterStrength / maxMobProgress;

        scoreScreenDisplay.SetActive(GameOver());
        if (scoreScreenDisplay.active)
        {
            timerDisplayTextElement.text = string.Format("Elapsed Time: {0:0.0}", elapsedTime);
            pointDisplayTextElement.text = string.Format("Points: {0}", points);
            leftCountDisplayTextElement.text = string.Format("Gregor finished {0} monsters.", leftCompletions);
            rightCountDisplayTextElement.text = string.Format("Frank finished {0} monsters.", rightCompletions);
            victoryDisplayTextElement.text = monsterStrength >= maxMobProgress ? "Victory!" : "Defeat";
        }
    }

    private bool GameOver()
    {
        if (monsterStrength >= maxMobProgress || mobProgress >= maxMobProgress)
        {
            Time.timeScale = 0;
            return true;
        }
        return false;
    }

    public void CompleteRight()
    {
        points += pointsPerCompletion + Mathf.RoundToInt(Mathf.Max(((bonusTimeThreshold - rightTimer) / bonusTimeThreshold), 0) * pointsPerCompletion);
        rightTimer = 0;
        rightCompletions++;
        monsterStrength += monsterStrengthAddition;
        mobProgress -= monsterStrengthAddition;
    }

    public void CompleteLeft()
    {
        points += pointsPerCompletion + Mathf.RoundToInt(Mathf.Max(((bonusTimeThreshold - leftTimer) / bonusTimeThreshold), 0) * pointsPerCompletion);
        leftTimer = 0;
        leftCompletions++;
        monsterStrength += monsterStrengthAddition;
        mobProgress -= monsterStrengthAddition;
    }
}
