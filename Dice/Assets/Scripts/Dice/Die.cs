using UnityEngine;
using System.Collections;

public class Die : MonoBehaviour {
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
    private float curretRollDuration;

    [HideInInspector]
    public bool rolling;

    public int value = 0;
    private static int rollCounter;

    void Awake() {
        minRotationSpeed = 300f;
        maxRotationSpeed = 600f;

        minRollDuration = .5f;
        maxRollDuration = 2f;
    }

    void OnEnable() {

    }

	// Use this for initialization
	void Start () {
        //_dice = dice;
        _rollRate = rollRate;
        dieRenderer = gameObject.GetComponent<SpriteRenderer>();
        rotator = gameObject.GetComponent<RotateDie>();
	}

    public void ShowDie(bool show) {
        gameObject.SetActive(show);
    }

    public void StartRolling() {
        rotator.rotationSpeed = Random.Range(0, 100) % 2 == 0 ? Random.Range(minRotationSpeed, maxRotationSpeed) :
                                                               -Random.Range(minRotationSpeed, maxRotationSpeed);
        curretRollDuration = Random.Range(minRollDuration, maxRollDuration);
        StartCoroutine(Roll());
    }

    private IEnumerator Roll() {
        ++rollCounter;
        rolling = true;
        rotator.StartRotation();
        StartCoroutine(StopRolling());
        while(rolling) {
            yield return new WaitForSeconds(_rollRate);
            value = Random.Range(0, 6);
            dieRenderer.sprite = dice[value++];
        }
        print(value);
        rotator.StopRotation();
        RollFinished();
    }

    private IEnumerator StopRolling() {
        yield return new WaitForSeconds(curretRollDuration);
        rolling = false;
    }

    private void RollFinished() {
        if(--rollCounter == 0)
            DiceDisplayer.rolling = false;
    }
}
