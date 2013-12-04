using UnityEngine;
using System.Collections;

public class DiceDisplayer : MonoBehaviour {

	private SpriteRenderer dieRenderer;
	public Sprite[] dice;
	public float rollDuration;
	public float rollRate;
	private bool rolling;
	private RotateDie rotator;

	// Use this for initialization
	void Start () {
		dieRenderer = transform.FindByName<SpriteRenderer>("Die");
		rotator = transform.FindByName<RotateDie>("Die");
	}
	
	public void RollDice() {
		dieRenderer.enabled = true;

			if(!rolling){
			StartCoroutine(Roll(Random.Range (0, 6)));
			StartCoroutine(StopRolling());
			rotator.shouldRoll = true;
			}
	}

	private IEnumerator Roll(int diceNumber) {

		rolling = true;
		while(rolling){
			yield return new WaitForSeconds(rollRate);
			dieRenderer.sprite = dice[Random.Range (0, 6)];
		}
		dieRenderer.sprite = dice[diceNumber];
	}

	private IEnumerator StopRolling() {
		yield return new WaitForSeconds(rollDuration);
		rolling = false;
		rotator.shouldRoll = false;

	}
}
