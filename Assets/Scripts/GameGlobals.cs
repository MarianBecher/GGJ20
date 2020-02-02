using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UberAudio;

public class GameGlobals : MonoBehaviour
{
    [Header("Balancing")]
    [SerializeField] private float bonusTimeThreshold = 60;
    [SerializeField] private int pointsPerCompletion = 500;
    [SerializeField] private int maxMobProgress = 300;
    [SerializeField] private int monsterStrengthAddition = 60;
    [SerializeField] private float mobScalingFactor = 2;
    [SerializeField] private float timePenalty = 5;
    [SerializeField] private float fightBackSpeed = 1;

    [Header("Referenzen")]
    [SerializeField] private GameObject pointDisplay;
    [SerializeField] private GameObject timerDisplay;
    [SerializeField] private GameObject leftCompletionDisplay;
    [SerializeField] private GameObject rightCompletionDisplay;
    [SerializeField] private GameObject scoreScreenDisplay;
    [SerializeField] private GameObject victoryDisplay;
    [SerializeField] private GameObject defeatDisplay;
    [SerializeField] private GameObject fightDisplay;
    [SerializeField] private Image mobProgressDisplay;
    [SerializeField] private WorkingBench leftBench;
    [SerializeField] private WorkingBench rightBench;

    private int points = 0;
    private int leftCompletions = 0;
    private float leftTimer = 0;
    private int rightCompletions = 0;
    private float rightTimer = 0;
    private float elapsedTime = 0;
    private float mobProgress = 150;
    private float fightBackValue = 0;
    private bool gamedone = false;

    private TextMeshProUGUI timerDisplayTextElement;
    private TextMeshProUGUI pointDisplayTextElement;
    private TextMeshProUGUI leftCountDisplayTextElement;
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
        leftCountDisplayTextElement = leftCompletionDisplay.GetComponent<TextMeshProUGUI>();
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
        mobProgress += Time.deltaTime * mobScalingFactor;
        fightDisplay.transform.localPosition = new Vector3((mobProgress - (maxMobProgress / 2)) * -2, 0, 0);

        float frameChange = fightBackValue * fightBackSpeed * Time.deltaTime;
        fightBackValue = fightBackValue > 0 ? fightBackValue - frameChange : 0;
        mobProgress = frameChange > 0 ? mobProgress - frameChange : mobProgress;
        //Update all displays
        mobProgressDisplay.fillAmount = mobProgress / maxMobProgress;

        scoreScreenDisplay.SetActive(GameOver());
        if (!gamedone && scoreScreenDisplay.active)
        {
            timerDisplayTextElement.text = string.Format("Elapsed Time: {0:0.0}", elapsedTime);
            pointDisplayTextElement.text = string.Format("Points: {0:0.}", Mathf.Max(points - (elapsedTime * timePenalty), 0));
            leftCountDisplayTextElement.text = string.Format("{0}x", leftCompletions);
            rightCountDisplayTextElement.text = string.Format("{0}x", rightCompletions);
            defeatDisplay.SetActive(mobProgress >= maxMobProgress);
            victoryDisplay.SetActive(mobProgress <= 0);
            AudioManager.Instance.Play(mobProgress <= 0 ? "GameWon" : "GameOver");
            gamedone = true;
        }
    }

    private bool GameOver()
    {
        if (mobProgress <= 0 || mobProgress >= maxMobProgress)
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
        //mobProgress -= monsterStrengthAddition;
        fightBackValue += monsterStrengthAddition;
    }

    public void CompleteLeft()
    {
        points += pointsPerCompletion + Mathf.RoundToInt(Mathf.Max(((bonusTimeThreshold - leftTimer) / bonusTimeThreshold), 0) * pointsPerCompletion);
        leftTimer = 0;
        leftCompletions++;
        //mobProgress -= monsterStrengthAddition;
        fightBackValue += monsterStrengthAddition;
    }
}
