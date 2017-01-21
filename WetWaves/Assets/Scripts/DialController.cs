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
  public float masterVolume = 0f;

  //Sounds
  public StationController[] stations;
  public AudioSource staticSource;
  public AudioClip radioStatic;

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
    GetInput();
    UpdateAudio(currentFrequency);
    UpdateFrequency();
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

    /* VOLUME CONTROL */
    if (Input.GetKeyDown(KeyCode.W)) //Tapped
    {
      if (masterVolume <= 1.0f)
      {
        dialRight.transform.Rotate(new Vector3(0, 1, 0), -10, Space.Self);
        masterVolume += volumeIncrement;
      }
      else
      {
        masterVolume = 1.0f;
      }
    }
    else if (Input.GetKey(KeyCode.W)) //Held
    {
      if (masterVolume <= 1.0f)
      {
        //Wait a short delay to start fast tuning
        waitForHoldTimer += Time.deltaTime;
        if (waitForHoldTimer > 0.2f)
        {
          dialRight.transform.Rotate(new Vector3(0, 1, 0), -10, Space.Self);
          masterVolume += (int)volumeIncrement;
        }
      }
      else
      {
        masterVolume = 1.0f;
      }
    }
    if (Input.GetKeyUp(KeyCode.W)) //Reset fast timer
    {
      waitForHoldTimer = 0;
    }
    if (Input.GetKeyDown(KeyCode.S)) //Tapped
    {
      if (masterVolume >= 0f)
      {
        dialRight.transform.Rotate(new Vector3(0, 1, 0), 10, Space.Self);
        masterVolume -= volumeIncrement;
      }
      else
      {
        masterVolume = 0f;
      }
    }
    else if (Input.GetKey(KeyCode.S)) //Held
    {
      if (masterVolume >= 0f)
      {
        //Wait a short delay to start fast tuning
        waitForHoldTimer += Time.deltaTime;
        if (waitForHoldTimer > 0.2f)
        {
          dialRight.transform.Rotate(new Vector3(0, 1, 0), 10, Space.Self);
          masterVolume -= (int)volumeIncrement;
        }
      }
      else
      {
        masterVolume = 0f;
      }
    }
    if (Input.GetKeyUp(KeyCode.S)) //Reset fast timer
    {
      waitForHoldTimer = 0;
    }
  }

  public void UpdateFrequency()
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

    for (int i = 0; i < stations.Length; i++)
    {
      var stationBandwidth = ToFrequencyRange(stations[i].stationBandwidth);
      if (frequency > stations[i].frequency - stationBandwidth && frequency < stations[i].frequency + stationBandwidth) //Check if close to a radio station
      {
        if (frequency == stations[i].frequency) //Are we right on the station
        {
          stations[i].GetSource().volume = 1.0f * masterVolume;
        }
        else if (frequency > stations[i].frequency - stationBandwidth && frequency < stations[i].frequency)
        {
          switch (stations[i].frequency - frequency) //How close to the station are we?
          {
            case 1:
            stations[i].GetSource().volume = 0.4f * masterVolume;
            break;
            case 2:
            stations[i].GetSource().volume = 0.3f * masterVolume;
            break;
            case 3:
            stations[i].GetSource().volume = 0.2f * masterVolume;
            break;
            case 4:
            stations[i].GetSource().volume = 0.1f * masterVolume;
            break;
            default:
            stations[i].GetSource().volume = 0.0f * masterVolume;
            break;
          }
        }
        else
        {
          switch (frequency - stations[i].frequency)//How close to the station are we?
          {
            case 1:
            stations[i].GetSource().volume = 0.4f * masterVolume;
            break;
            case 2:
            stations[i].GetSource().volume = 0.3f * masterVolume;
            break;
            case 3:
            stations[i].GetSource().volume = 0.2f * masterVolume;
            break;
            case 4:
            stations[i].GetSource().volume = 0.1f * masterVolume;
            break;
            default:
            stations[i].GetSource().volume = 0.0f * masterVolume;
            break;
          }
        }
      }
      else
      {
        //Not near a station
        stations[i].GetSource().volume = 0.0f * masterVolume;
      }
    }
    float tempHeighestVolume = 0;
    for (int i = 0; i < stations.Length; i++)
    {

      print(string.Format("Current station volume: {0}", stations[i].GetSource().volume));
      if(stations[i].GetSource().volume > 0)
      {
        if (stations[i].GetSource().volume > tempHeighestVolume) //Keep a value of the current highest volume station
        {
          tempHeighestVolume = stations[i].GetSource().volume;
        }
      }
    }
    //Set the static volume
    staticSource.volume = 1  * masterVolume - tempHeighestVolume;

  }

  private int ToFrequencyRange(StationController.bandwidth bandwidth)
  {
    switch (bandwidth) {
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
