using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject[] gameObjects;
	public float stayLit;
	public float stayLitCounter;
	public float waitBetweenLights;
    public float waitAfterCorrectSequence;
    public float waitAfterIncorrectAttempt;

    public List<int> activeSequence;

	private int colorSelect;
	private float waitBetweenCounter;

	public bool shouldBeLit;
	public bool shouldBeDark;
	public bool gameActive;
	private int positionInSequence;
	private int inputInSequence;


    // Use this for initialization
    void Start () {
		StartGame ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameActive) {
			// Disable colliders until the game starts again
			// @TODO - loop through and disable colliders
//			gameObjects.GetComponent<BoxCollider>().enabled = false;
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
                    // NOTIFICATION SOUND
                    //AkSoundEngine.PostEvent(gameObjects[activeSequence[positionInSequence]].GetComponentInChildren<WwisePositionSetup>().notificationEvent, gameObjects[activeSequence[positionInSequence]]);
                    gameObjects[activeSequence [positionInSequence]].GetComponent<AudioSource> ().PlayOneShot (gameObjects [activeSequence [positionInSequence]].GetComponent<SelectionHandler>().sound, 0.7f);

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
        //AkSoundEngine.PostEvent(gameObjects[activeSequence[positionInSequence]].GetComponentInChildren<WwisePositionSetup>().notificationEvent, gameObjects[activeSequence[positionInSequence]]);

        stayLitCounter = stayLit;
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
                // CORRECT SOUND GOES HERE
                gameObjects [activeSequence [positionInSequence]].GetComponent<AudioSource> ().PlayOneShot (gameObjects [activeSequence [positionInSequence]].GetComponent<SelectionHandler>().sound, 0.7f);
                //AkSoundEngine.PostEvent(gameObjects[activeSequence[positionInSequence]].GetComponentInChildren<WwisePositionSetup>().correctEvent, gameObjects[activeSequence[positionInSequence]]);
                inputInSequence++;

                // If current value is more than the list in sequence
                if (inputInSequence >= activeSequence.Count) {
                    StartCoroutine(WaitAfterSequence(waitAfterCorrectSequence));
                }
			} else {
				Debug.Log ("Wrong");
                //gameActive = false;
                // WRONG SOUND GOES HERE
                gameObjects [activeSequence [positionInSequence]].GetComponent<AudioSource> ().PlayOneShot (gameObjects [activeSequence [positionInSequence]].GetComponent<SelectionHandler>().sound, 0.7f);
                //AkSoundEngine.PostEvent(gameObjects[activeSequence[positionInSequence]].GetComponentInChildren<WwisePositionSetup>().incorrectEvent, gameObjects[activeSequence[positionInSequence]]);
                StartCoroutine(WaitAfterWrong(waitAfterIncorrectAttempt));

            }
		}
	}

    private IEnumerator WaitAfterWrong(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        StartGame();
    }

    private IEnumerator WaitAfterSequence(float waitTime) {
        Debug.Log("Before wait");
        yield return new WaitForSeconds(waitTime);

        Debug.Log("After wait");

        // Set position on start to 0
        positionInSequence = 0;
        inputInSequence = 0;
        // Set a random color
        colorSelect = Random.Range(0, gameObjects.Length);
        // Add random colorSelect number to list
        activeSequence.Add(colorSelect);
        // Set this colors alpha to full
        gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color = new Color(gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.r, gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.g, gameObjects[activeSequence[positionInSequence]].GetComponent<Renderer>().material.color.b, 1f);
        gameObjects[activeSequence[positionInSequence]].GetComponent<AudioSource>().PlayOneShot(gameObjects[activeSequence[positionInSequence]].GetComponent<SelectionHandler>().sound, 0.7f);
        //AkSoundEngine.PostEvent(gameObjects[activeSequence[positionInSequence]].GetComponentInChildren<WwisePositionSetup>().notificationEvent, gameObjects[activeSequence[positionInSequence]]);
        stayLitCounter = stayLit;
        shouldBeLit = true;

        gameActive = false;
    }
}
