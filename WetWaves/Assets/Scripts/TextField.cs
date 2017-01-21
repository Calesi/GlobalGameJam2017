using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextField : MonoBehaviour {

    public GameObject textDisplayer;
    string currentText;

    // Get current note text to append to
    void Start () {
        var input = gameObject.GetComponent<InputField>();
        currentText = textDisplayer.GetComponent<Text>().text;
        input.text = currentText;
        input.onEndEdit.AddListener(updateTextdisplay);
    }
	
    //Append the new notes and display them
    public void updateTextdisplay(string newText)
    {
        currentText = currentText + "\n" +  newText;
        textDisplayer.GetComponent<Text>().text = currentText;
        gameObject.SetActive(false);
    }    
}
