using UnityEngine;
using System.Collections;

public class Notebook : MonoBehaviour {

    public GameObject textfield;

    void OnMouseDown()
    {
        // Display text input field and piece of note paper
        textfield.SetActive(true);
    }
}
