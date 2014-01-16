using UnityEngine;
using System.Collections;

public class Die : MonoBehaviour
{
    //private static Sprite[] _dice;

    public Sprite[] dice;

    private static float _rollRate;
    public float rollRate;

    public SpriteRenderer dieRenderer { get; private set; }
    private RotateDie rotator { get; set; }
    public float minRotationSpeed { get; set; }
    public float maxRotationSpeed { get; set; }

    public float minRollDuration { get; set; }
    public float maxRollDuration { get; set; }
    private float currentRollDuration;

    [HideInInspector]
    public bool rolling;

    public bool selected;
    public int value = 0;
    private static int rollCounter;
    public static int selectedCounter;

    void Awake()
    {
        minRotationSpeed = 300f;
        maxRotationSpeed = 600f;

        minRollDuration = .5f;
        maxRollDuration = 2f;
    }

    void OnEnable()
    {

    }

    // Use this for initialization
    void Start()
    {
        //_dice = dice;
        _rollRate = rollRate;
        dieRenderer = gameObject.GetComponent<SpriteRenderer>();
        rotator = gameObject.GetComponent<RotateDie>();
        selected = false;
        selectedCounter = 0;
    }

    public void ShowDie(bool show)
    {
        gameObject.SetActive(show);
    }

    public void StartRolling()
    {
        rotator.rotationSpeed = Random.Range(0, 100) % 2 == 0 ? Random.Range(minRotationSpeed, maxRotationSpeed) :
                                                               -Random.Range(minRotationSpeed, maxRotationSpeed);
        currentRollDuration = Random.Range(minRollDuration, maxRollDuration);
        StartCoroutine(Roll());
    }

    private IEnumerator Roll()
    {
        ++rollCounter;
        rolling = true;
        rotator.StartRotation();
        StartCoroutine(StopRollingTimer());
        while (rolling)
        {
            yield return new WaitForSeconds(_rollRate);
            value = Random.Range(0, 6);
            dieRenderer.sprite = dice[value++];
        }
        RollFinished();
    }

    private IEnumerator StopRollingTimer()
    {
        yield return new WaitForSeconds(currentRollDuration);
        rolling = false;
    }

    private void RollFinished()
    {
        //print(value);
        GameManager.dieValues[value - 1]++;
        rotator.StopRotation();
        if (--rollCounter == 0)
        {
            DiceDisplayer.rolling = false;
            ScoreChecker.InitialChecker();

        }
    }

    void OnMouseUpAsButton()
    {
        selected = !selected;
        Enlarge();
        Debug.Log(selected);
        //foreach (GameObject value in Die.selectedDice)
        //Debug.Log(value);
  
    }

    void Enlarge()
    {
        if (selected)
        {
            gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0f);
            selectedCounter++;

        }
        else
        {
            gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0f);
            selectedCounter--;
        }
    }
}
