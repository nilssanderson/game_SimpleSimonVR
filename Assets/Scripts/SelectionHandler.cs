using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectionHandler : MonoBehaviour {

	public int thisButtonNumber;
	public bool selected;
	public AudioClip sound;
  AudioSource audioSource;

	private Material theMaterial;
	private GameManager theGM;


	// Use this for initialization
	void Start () {
		theMaterial = GetComponent<Renderer>().material;
		theGM = FindObjectOfType<GameManager> ();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Something entered me: " + other);
		if (other.gameObject.tag == "hand") {
			if (theGM.GetComponent<GameManager> ().gameActive) {
				theMaterial.color = new Color(theMaterial.color.r, theMaterial.color.g, theMaterial.color.b, 1f);
				theGM.ColorPressed (thisButtonNumber);
				audioSource.PlayOneShot(sound, 0.7F);
				this.selected = true;
			}
		}
	}

	void OnTriggerExit(Collider other) {
		Debug.Log ("Something exited me: " + other);
		if (other.gameObject.tag == "hand") {
			theMaterial.color = new Color(theMaterial.color.r, theMaterial.color.g, theMaterial.color.b, .5f);
			this.selected = false;
		}
	}
}
