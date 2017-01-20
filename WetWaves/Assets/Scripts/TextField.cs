using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextField : MonoBehaviour {

    public GameObject textDisplayer;
    string currentText;

    // Use this for initialization
    void Start () {
        var input = gameObject.GetComponent<InputField>();
        currentText = textDisplayer.GetComponent<Text>().text;
        Debug.Log("start");
        input.onEndEdit.AddListener(updateTextdisplay);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updateTextdisplay(string newText)
    {
        currentText = currentText + "\n" +  newText;
        textDisplayer.GetComponent<Text>().text = currentText;

        Debug.Log(currentText);
        Debug.Log("end");
        gameObject.SetActive(false);
    }
    
}
