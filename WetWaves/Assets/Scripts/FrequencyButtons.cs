using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyButtons : MonoBehaviour {

    public int[] buttonFrequencys;
    public int currentButton = 0;
    public GameObject[] buttons;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < buttonFrequencys.Length; i++)
        {
            buttonFrequencys[i] = 0;
        }
	}
	
	// Update is called once per frame
	void Update () {
        //if (!TapeRecorder.instance.recording)
        //{
        //    CheckClicked();
        //    SetButtonFrequency();
        //}
	}

    void CheckClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500.0f))
            {
                GameObject objectHit = hit.collider.gameObject;
                if (objectHit.tag == "Button")
                {
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        if (buttons[i] == objectHit)
                        {
                            buttons[currentButton].transform.Translate(new Vector3(0, 0, 0.150f));
                            currentButton = i;
                            buttons[currentButton].transform.Translate(new Vector3(0, 0, -0.150f));
                            GetComponent<DialController>().currentFrequency = buttonFrequencys[currentButton];
                            GetComponent<DialController>().UpdateRadio(DialController.RadioEvent.StationChanged);
                        }
                    }
                }

            }
        }
    }

    void SetButtonFrequency()
    {
        buttonFrequencys[currentButton] = GetComponent<DialController>().currentFrequency;
    }
}
