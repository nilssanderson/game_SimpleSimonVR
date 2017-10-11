using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Material[] colors;
	public float stayLit;
	public float stayLitCounter;

	private int colorSelect;


	// Use this for initialization
	void Start () {
		StartGame ();
	}
	
	// Update is called once per frame
	void Update () {
		// stayLitCounter is greater than 0 decrement it over time
		if (stayLitCounter > 0) {
			stayLitCounter -= Time.deltaTime;
		} else {
			// Set this colors alpha to half
			colors[colorSelect].color = new Color(colors[colorSelect].color.r, colors[colorSelect].color.g, colors[colorSelect].color.b, .5f);
		}
	}

	public void StartGame() {
		// Set a random color
		colorSelect = Random.Range(0, colors.Length);
		// Set this colors alpha to full
		colors[colorSelect].color = new Color(colors[colorSelect].color.r, colors[colorSelect].color.g, colors[colorSelect].color.b, 1f);

		stayLitCounter = stayLit;
	}

	public void ColorPressed(int whichButton) {
		if (colorSelect == whichButton) {
			Debug.Log ("Correct");
		} else {
			Debug.Log ("Wrong");
		}
	}
}
