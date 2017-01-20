using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialController : MonoBehaviour
{
    //Publics
    public GameObject dialLeft;
    public GameObject dialRight;
    public GameObject frequencyNotch;
    public Text frequencyText;

    //Sounds
    public AudioSource[] stationSources;
    public int[] stations; //There MUST be the same amount of stationSources as there are stations and put them in the correct order to get the correct station playing on the station wanted
    public AudioSource staticSource;
    public AudioClip radioStatic;    

    //Privates
    private float notchXPosition = -4.0f;
    private float waitForHoldTimer = 0;
    private float tuneSpeed = 1;
    private int currentFrequency;

    void Start()
    {
        currentFrequency = 0;
    }

    void Update()
    {
        GetInput();
        UpdateAudio(currentFrequency);

    }

    void GetInput()
    {

        //Check if single tap or held
        if (Input.GetKeyDown(KeyCode.A)) //Tapped
        {
            if (currentFrequency >= 1)
            {
                dialLeft.transform.Rotate(new Vector3(0, 1, 0), tuneSpeed, Space.Self);
                currentFrequency -= (int)tuneSpeed;
            }
            else
            {
                currentFrequency = 0;
            }
            UpdateFrequency();
        }
        else if (Input.GetKey(KeyCode.A)) //Held
        {
            if (currentFrequency >= 1)
            {
                //Wait a short delay to start fast tuning
                waitForHoldTimer += Time.deltaTime;
                if (waitForHoldTimer > 0.2f)
                {
                    dialLeft.transform.Rotate(new Vector3(0, 1, 0), tuneSpeed, Space.Self);
                    currentFrequency -= (int)tuneSpeed;
                }
            }
            else
            {
                currentFrequency = 0;
            }
            UpdateFrequency();
        }
        if (Input.GetKeyUp(KeyCode.A)) //Reset fast timer
        {
            waitForHoldTimer = 0;
        }

        //Check if single tap or held
        if (Input.GetKeyDown(KeyCode.D)) //Tapped
        {
            if (currentFrequency <= 799)
            {
                dialLeft.transform.Rotate(new Vector3(0, 1, 0), -tuneSpeed, Space.Self);
                currentFrequency += (int)tuneSpeed;
            }
            else
            {
                currentFrequency = 800;
            }
            UpdateFrequency();
        }
        else if (Input.GetKey(KeyCode.D)) //Held
        {
            if (currentFrequency <= 799)
            {
                //Wait a short delay to start fast tuning
                waitForHoldTimer += Time.deltaTime;
                if (waitForHoldTimer > 0.2f)
                {
                    dialLeft.transform.Rotate(new Vector3(0, 1, 0), -tuneSpeed, Space.Self);
                    currentFrequency += (int)tuneSpeed;
                }
            }
            else
            {
                currentFrequency = 800;
            }
            UpdateFrequency();
        }
        if (Input.GetKeyUp(KeyCode.D)) //Reset fast timer
        {
            waitForHoldTimer = 0;
        }
    }

    void UpdateFrequency()
    {

        //calculate correct notch position
        notchXPosition = currentFrequency;
        notchXPosition /= 100;
        notchXPosition -= 4.0f;
        notchXPosition -= frequencyNotch.transform.position.x;
        Vector3 pos = new Vector3(notchXPosition, 0, 0);
        frequencyNotch.transform.Translate(pos);

        //Calculate frequency into stations e.g. 459 = 45.9
        float actualFrequency = (float)currentFrequency;
        actualFrequency /= 10;

        //Keep the length of the string the same at all times
        if (actualFrequency == 0)
        {
            frequencyText.text = "00.0";
        }
        else if (actualFrequency < 10)
        {
            frequencyText.text = "0" + actualFrequency.ToString();
        }
        else
        {
            frequencyText.text = actualFrequency.ToString();
        }
    }

    void UpdateAudio(int frequency)
    {

        for (int i = 0; i < stations.Length; i++)
        {
            if (frequency > stations[i] - 5 && frequency < stations[i] + 5) //Check if close to a radio station
            {
                if (frequency == stations[i]) //Are we right on the station
                {
                    stationSources[i].volume = 1;
                }
                else if (frequency > stations[i] - 5 && frequency < stations[i])
                {
                    switch (stations[i] - frequency) //How close to the station are we?
                    {
                        case 1:
                            stationSources[i].volume = 0.4f;
                            break;
                        case 2:
                            stationSources[i].volume = 0.3f;
                            break;
                        case 3:
                            stationSources[i].volume = 0.2f;
                            break;
                        case 4:
                            stationSources[i].volume = 0.1f;
                            break;
                        default:
                            stationSources[i].volume = 0.0f;
                            break;
                    }
                }
                else
                {
                    switch (frequency - stations[i])//How close to the station are we?
                    {
                        case 1:
                            stationSources[i].volume = 0.4f;
                            break;
                        case 2:
                            stationSources[i].volume = 0.3f;
                            break;
                        case 3:
                            stationSources[i].volume = 0.2f;
                            break;
                        case 4:
                            stationSources[i].volume = 0.1f;
                            break;
                        default:
                            stationSources[i].volume = 0.0f;
                            break;
                    }
                }
            }
            else
            {
                //Not near a station
                stationSources[i].volume = 0;
            }
        }
        float tempHeighestVolume = 0;
        for (int i = 0; i < stations.Length; i++)
        {
            if(stationSources[i].volume > 0)
            {
                if (stationSources[i].volume > tempHeighestVolume) //Keep a value of the current highest volume station
                {
                    tempHeighestVolume = stationSources[i].volume;
                }
            }            
        }
        //Set the static volume
        staticSource.volume = 1 - tempHeighestVolume;

    }

    
}
