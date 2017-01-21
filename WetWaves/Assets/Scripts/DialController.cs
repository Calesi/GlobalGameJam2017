using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DialController : MonoBehaviour
{
    //Publics
    public GameObject dialLeft;
    public GameObject dialRight;
    public GameObject frequencyNotch;
    public Text frequencyText;
    //public TextMesh frequencyText;
    public float masterVolume = 0f;

    //Sounds
    public StationController[] stations;
    public AudioSource staticSource;
    public AudioClip radioStatic;

    //Constants
    private readonly float interferenceBase = 2.0f;

    //Privates
    private float notchXPosition = -4.0f;
    private float waitForHoldTimer = 0;
    private float tuneSpeed = 1;
    private float volumeIncrement = 0.1f;
    public int currentFrequency;


    void Start()
    {
        currentFrequency = 0;
    }

    void Update()
    {
        if (!TapeRecorder.instance.recording)
        {
            GetInput();
            UpdateAudio(currentFrequency);
            UpdateFrequency();
        }
        
    }

    void GetInput()
    {

        //Check if single tap or held
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) //Tapped
        {
            if (currentFrequency >= 1)
            {
                dialLeft.transform.Rotate(new Vector3(0, 0, 1), -tuneSpeed, Space.Self);
                currentFrequency -= (int)tuneSpeed;
            }
            else
            {
                currentFrequency = 0;
            }
            UpdateFrequency();
            return;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) //Held
        {
            if (currentFrequency >= 1)
            {
                //Wait a short delay to start fast tuning
                waitForHoldTimer += Time.deltaTime;
                if (waitForHoldTimer > 0.2f)
                {
                    dialLeft.transform.Rotate(new Vector3(0, 0, 1), -tuneSpeed, Space.Self);
                    currentFrequency -= (int)tuneSpeed;
                }
            }
            else
            {
                currentFrequency = 0;
            }
            UpdateFrequency();
            return;
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) //Reset fast timer
        {
            waitForHoldTimer = 0;
        }

        //Check if single tap or held
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) //Tapped
        {
            if (currentFrequency <= 799)
            {
                dialLeft.transform.Rotate(new Vector3(0, 0, 1), tuneSpeed, Space.Self);
                currentFrequency += (int)tuneSpeed;
            }
            else
            {
                currentFrequency = 800;
            }
            UpdateFrequency();
            return;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) //Held
        {
            if (currentFrequency <= 799)
            {
                //Wait a short delay to start fast tuning
                waitForHoldTimer += Time.deltaTime;
                if (waitForHoldTimer > 0.2f)
                {
                    dialLeft.transform.Rotate(new Vector3(0, 0, 1), tuneSpeed, Space.Self);
                    currentFrequency += (int)tuneSpeed;
                }
            }
            else
            {
                currentFrequency = 800;
            }
            UpdateFrequency();
            return;
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) //Reset fast timer
        {
            waitForHoldTimer = 0;
        }

        /* VOLUME CONTROL */
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) //Tapped
        {
            if (masterVolume <= 1.0f)
            {
                dialRight.transform.Rotate(new Vector3(0, 0, 1), -10, Space.Self);
                masterVolume += volumeIncrement;
            }
            else
            {
                masterVolume = 1.0f;
            }
            return;
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) //Held
        {
            if (masterVolume <= 1.0f)
            {
                //Wait a short delay to start fast tuning
                waitForHoldTimer += Time.deltaTime;
                if (waitForHoldTimer > 0.2f)
                {
                    dialRight.transform.Rotate(new Vector3(0, 0, 1), -10, Space.Self);
                    masterVolume += (int)volumeIncrement;
                }
            }
            else
            {
                masterVolume = 1.0f;
            }
            return;
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) //Reset fast timer
        {
            waitForHoldTimer = 0;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) //Tapped
        {
            if (masterVolume >= 0f)
            {
                dialRight.transform.Rotate(new Vector3(0, 0, 1), 10, Space.Self);
                masterVolume -= volumeIncrement;
            }
            else
            {
                masterVolume = 0f;
            }
            return;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) //Held
        {
            if (masterVolume >= 0f)
            {
                //Wait a short delay to start fast tuning
                waitForHoldTimer += Time.deltaTime;
                if (waitForHoldTimer > 0.2f)
                {
                    dialRight.transform.Rotate(new Vector3(0, 0, 1), 10, Space.Self);
                    masterVolume -= (int)volumeIncrement;
                }
            }
            else
            {
                masterVolume = 0f;
            }
            return;
        }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) //Reset fast timer
        {
            waitForHoldTimer = 0;
        }
    }

    public void UpdateFrequency()
    {

        //calculate correct notch position
        notchXPosition = currentFrequency;
        notchXPosition /= 40;
        notchXPosition *= 0.22f;
        notchXPosition -= frequencyNotch.transform.position.x;
        Vector3 pos = new Vector3(-notchXPosition, 0, 0);
        frequencyNotch.transform.Translate(pos);

        //Calculate frequency into stations e.g. 459 = 45.9
        float actualFrequency = (float)currentFrequency;
        actualFrequency /= 10;
        const int BASE_FREQUENCY = 80;
        float displayFrequency = actualFrequency + BASE_FREQUENCY;

        //Keep the length of the string the same at all times
        if (actualFrequency > -0.05 && actualFrequency < 0.05)
        {
            frequencyText.text = string.Format("{0}.0", BASE_FREQUENCY);
        }
        else if (displayFrequency < 10 && displayFrequency == Mathf.FloorToInt(displayFrequency))
        {
            frequencyText.text = "0" + displayFrequency.ToString() + ".0";
        }
        else if (displayFrequency < 10)
        {
            frequencyText.text = "0" + displayFrequency.ToString();
        }
        else if (displayFrequency == Mathf.FloorToInt(displayFrequency))
        {
            frequencyText.text = displayFrequency.ToString() + ".0";
        }
        else
        {
            frequencyText.text = displayFrequency.ToString();
        }
    }

    void UpdateAudio(int frequency)
    {

    	float tempHeighestVolume = 0;
        for (int i = 0; i < stations.Length; i++)
        {
            var stationBandwidth = ToFrequencyRange(stations[i].stationBandwidth);
            if (Convert.ToSingle(Math.Abs(stations[i].frequency - frequency)) <= stationBandwidth) //Check if close to a radio station
            {
                if (frequency == stations[i].frequency) //Are we right on the station
                {
                    stations[i].GetSource().volume = 1.0f * masterVolume;
                } else {
        //          float baseModifier = 0.1f;
                    float exponent = (stationBandwidth - Convert.ToSingle(Math.Abs(stations[i].frequency - frequency)));
                    float fraction = Convert.ToSingle(Math.Pow(interferenceBase, exponent)) - 1.0f;
                    float outOf = Convert.ToSingle(Math.Pow(interferenceBase, stationBandwidth)) - 1.0f;
                    float percentageModifier = fraction / outOf;
                    stations[i].GetSource().volume = (percentageModifier) * masterVolume;
                }
                if (stations[i].GetSource().volume > tempHeighestVolume) //Keep a value of the current highest volume station
                {
                    tempHeighestVolume = stations[i].GetSource().volume;
                }
            }
            else
            {
                //Not near a station
                stations[i].GetSource().volume = 0.0f * masterVolume;
            }
        }
        //Set the static volume
        staticSource.volume = 1 * masterVolume - tempHeighestVolume;

    }

    private int ToFrequencyRange(StationController.bandwidth bandwidth)
    {
        switch (bandwidth)
        {
            case StationController.bandwidth.High:
                return 5;
            case StationController.bandwidth.Medium:
                return 3;
            case StationController.bandwidth.Low:
                return 1;
            default:
                return 3;
        }
    }
}
