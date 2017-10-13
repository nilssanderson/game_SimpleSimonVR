using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject[] gameObjects;
	public float stayLit;
	public float stayLitCounter;
	public float waitBetweenLights;
	public float waitAfterSequence;
	public float waitAfterSequenceCounter;
	public float waitAfterGameOver;
	public float waitAfterGameOverCounter;

	public List<int> activeSequence;

	private int colorSelect;
	private float waitBetweenCounter;

	public bool shouldBeLit;
	public bool shouldBeDark;
	public bool gameActive;
	private int positionInSequence;
	private int inputInSequence;
	private bool sequenceFinished;
	private bool gameOver;


	// Use this for initialization
	void Start () {
		StartGame ();
	}

	// Update is called once per frame
	void Update () {
		if (!gameActive) {
			// Disable colliders until the game starts again
			// @TODO - loop through and disable colliders
			foreach (GameObject gameObject in gameObjects) {
				gameObject.GetComponent<MeshCollider> ().enabled = false;
			}
		} else {
			foreach (GameObject gameObject in gameObjects) {
				gameObject.GetComponent<MeshCollider> ().enabled = true;
			}
		}

		if (gameOver == true) {
			gameActive = false;
			waitAfterGameOverCounter -= Time.deltaTime;

			if (waitAfterGameOverCounter < 0) {
				StartGame ();
				waitAfterGameOverCounter = waitAfterGameOver;
			}
		}

		if (sequenceFinished == true) {
			gameActive = false;
			waitAfterSequenceCounter -= Time.deltaTime;

			if (waitAfterSequenceCounter < 0) {
				// Set position on start to 0
				positionInSequence = 0;
				inputInSequence = 0;
				// Set a random color
				colorSelect = Random.Range(0, gameObjects.Length);
				// Add random colorSelect number to list
				activeSequence.Add(colorSelect);
				// Set this colors alpha to full
				gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color = new Color(gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.r, gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.g, gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.b, 1f);
				gameObjects [activeSequence [positionInSequence]].GetComponent<AudioSource> ().PlayOneShot (gameObjects [activeSequence [positionInSequence]].GetComponent<SelectionHandler>().sound, 0.7f);

				stayLitCounter = stayLit;
				shouldBeLit = true;

				sequenceFinished = false;
				waitAfterSequenceCounter = waitAfterSequence;
			}
		}

		// stayLitCounter is greater than 0 decrement it over time
		if (shouldBeLit == true) {
			stayLitCounter -= Time.deltaTime;

			if (stayLitCounter < 0) {
				// Set this colors alpha to half
				gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color = new Color(gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.r, gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.g, gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.b, .5f);
				shouldBeLit = false;

				// Looking at the pause
				shouldBeDark = true;
				waitBetweenCounter = waitBetweenLights;
				// Increment the positionInSequence
				positionInSequence++;
			}
		}

		if (shouldBeDark == true) {
			waitBetweenCounter -= Time.deltaTime;

			// If positionInSequence is greater or equal to activeSequence length (count as its a list)
			if (positionInSequence >= activeSequence.Count) {
				shouldBeDark = false;
				gameActive = true;
			} else {
				if (waitBetweenCounter < 0) {
					// Set this colors alpha to full
					gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color = new Color(gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.r, gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.g, gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.b, 1f);
					gameObjects [activeSequence [positionInSequence]].GetComponent<AudioSource> ().PlayOneShot (gameObjects [activeSequence [positionInSequence]].GetComponent<SelectionHandler>().sound, 0.7f);

					stayLitCounter = stayLit;
					shouldBeLit = true;
					shouldBeDark = false;
				}
			}
		}
	}

	public void StartGame() {
		// Empty list
		activeSequence.Clear();
		// Set position on start to 0
		positionInSequence = 0;
		inputInSequence = 0;
		// Set a random color
		colorSelect = Random.Range(0, gameObjects.Length);
		// Add random colorSelect number to list
		activeSequence.Add(colorSelect);
		// Set this colors alpha to full
		gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color = new Color(gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.r, gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.g, gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.b, 1f);
		gameObjects [activeSequence [positionInSequence]].GetComponent<AudioSource> ().PlayOneShot (gameObjects [activeSequence [positionInSequence]].GetComponent<SelectionHandler>().sound, 0.7f);

		stayLitCounter = stayLit;
		waitAfterSequenceCounter = waitAfterSequence;
		gameOver = false;
		waitAfterGameOverCounter = waitAfterGameOver;
		shouldBeLit = true;
	}

	public void ColorPressed(int whichButton) {
		// Sequence has stopped
		if (gameActive) {
			// Set this colors alpha to full
			//			gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color = new Color(gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.r, gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.g, gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.b, 1f);
			//			gameObjects [activeSequence [positionInSequence]].GetComponent<AudioSource> ().PlayOneShot (gameObjects [activeSequence [positionInSequence]].GetComponent<SelectionHandler>().sound, 0.7f);

			if (activeSequence[inputInSequence] == whichButton) {
				Debug.Log ("Correct");

				StartCoroutine (Wait (.5f));
				inputInSequence++;

				// If current value is more than the list in sequence
				if (inputInSequence >= activeSequence.Count) {
					sequenceFinished = true;
				}

			} else {
				Debug.Log ("Wrong");

				StartCoroutine (Wait (.5f));

				gameActive = false;
				gameOver = true;
			}
		}
	}

	public void ResetOpacitys() {
		foreach (GameObject gameObject in gameObjects) {
			gameObject.GetComponent<Renderer> ().material.color = new Color (gameObject.GetComponent<Renderer> ().material.color.r, gameObject.GetComponent<Renderer> ().material.color.g, gameObject.GetComponent<Renderer> ().material.color.b, .5f);
		}
	}

	private IEnumerator Wait(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		ResetOpacitys ();
	}
}