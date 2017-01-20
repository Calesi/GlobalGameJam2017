using UnityEngine;
using System.Collections;

public class Notebook : MonoBehaviour {

    public GameObject textfield;
    public GameObject notepad;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        // Display text input field and piece of note paper
        textfield.SetActive(true);
    }
}
