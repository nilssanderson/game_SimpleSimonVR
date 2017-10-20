using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectionHandler : MonoBehaviour {

	public int thisButtonNumber;
	public bool selected;

	private GameManager theGM;


	// Use this for initialization
	void Start () {
		theGM = FindObjectOfType<GameManager> ();
	}

	void OnTriggerEnter(Collider other) {
//		Debug.Log ("Something entered me: " + other);
		if (other.gameObject.tag == "hand") {
			theGM.ColorPressed (thisButtonNumber);
			this.selected = true;
		}
	}

	void OnTriggerExit(Collider other) {
//		Debug.Log ("Something exited me: " + other);
		if (other.gameObject.tag == "hand") {
			this.selected = false;
		}
	}
}
