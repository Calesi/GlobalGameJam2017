using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DialController : MonoBehaviour
{
    //Publics
    public enum RadioEvent { None, StationChanged, VolumeChanged };
    public GameObject dialLeft;
    public GameObject dialRight;
    public GameObject frequencyNotch;
    public Text frequencyText;
    //public TextMesh frequencyText;
    public float masterVolume;

    //Sounds
    public StationController[] stations;
    public AudioSource staticSource;
    public AudioClip radioStatic;

    //Constants
    private readonly float tuneSpeed = 1;
    private readonly float volumeIncrement = 0.1f;
    private readonly float interferenceBase = 2.0f; //Must be >1; lower the number more sudden the audio fade
    private readonly float defaultMasterVol = 0.25f;
    private readonly float percentStaticRand = 0.4f; //Larger means static volume is more randomised
    private readonly float minStaticAugendVol = 0.05f; //Minimum static volume

    //Privates
    private float staticVolModifier = 0.7f;
    private float notchXPosition = -4.0f;
    private float waitForHoldTimer = 0;
    public int currentFrequency;


    void Start()
    {
        masterVolume = defaultMasterVol;
        staticSource.volume = defaultMasterVol;
        UpdateRadio(RadioEvent.StationChanged);
    }

    void Update()
    {
        RadioEvent eventType = GetInput();
        UpdateRadio(eventType);

    }

    RadioEvent GetInput()
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
            return RadioEvent.StationChanged;
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
            return RadioEvent.StationChanged;
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
            return RadioEvent.StationChanged;
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
            return RadioEvent.StationChanged;
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
            return RadioEvent.VolumeChanged;
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
            return RadioEvent.VolumeChanged;
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
            return RadioEvent.VolumeChanged;
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
            return RadioEvent.VolumeChanged;
        }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) //Reset fast timer
        {
            waitForHoldTimer = 0;
        }
        return RadioEvent.None;
    }

    public void UpdateRadio(RadioEvent eventType)
    {
    	if (eventType != RadioEvent.None) {
    		UpdateFrequency();
    		switch (eventType) {
    			case RadioEvent.StationChanged:
    				StationController currentLoudestStation = UpdateAudio();
		    		staticSource.volume = setStaticVol(currentLoudestStation);
		            break;
	           case RadioEvent.VolumeChanged:
		           UpdateAudio();
		           break;
    		}
    	}
    }

    public void UpdateFrequency()
    {

        //calculate correct notch position
        notchXPosition = currentFrequency;
        notchXPosition /= 40; // Scale for some reason
        notchXPosition *= 0.22f; // Max x position
        notchXPosition *= (float)Math.Cos(transform.rotation.eulerAngles.y * Math.PI / 180); // Scale based on rotation
        notchXPosition -= frequencyNotch.transform.position.x; // Relative to where we are now
        notchXPosition += transform.position.x; // Relative to radio
        Vector3 pos = new Vector3(-notchXPosition, 0, 0);
        frequencyNotch.transform.Translate(pos);

        //Calculate frequency into stations e.g. 459 = 45.9
        float actualFrequency = (float)currentFrequency / 10.0f;
        const int BASE_FREQUENCY = 80;
        float displayFrequency = actualFrequency + BASE_FREQUENCY;

        //Display displayFrequency to 1 decimal place
        frequencyText.text = displayFrequency.ToString("0.0");
    }

    StationController UpdateAudio()
    {
    	StationController currentLoudestStation = null;
            AnimatedTexture.instance.rowToAnimate = 2;
            for (int i = 0; i < stations.Length; i++)
        {
        	if (stations[i] != null && stations[i].GetComponent<AudioSource>() != null) {
	            if (Convert.ToSingle(Math.Abs(stations[i].frequency - currentFrequency)) <= ToFrequencyRange(stations[i].stationBandwidth)) //Check if close to a radio station
	            {
	                if (currentFrequency == stations[i].frequency) //Are we right on the station
	                {
                            AnimatedTexture.instance.rowToAnimate = 0;
                            stations[i].GetComponent<AudioSource>().volume = 1.0f * masterVolume;
	                } else {
//	                    stations[i].GetSource().volume = setStationVol(stations[i], currentFrequency);
	                    stations[i].GetComponent<AudioSource>().volume = setStationVol(stations[i], currentFrequency);
                        if (AnimatedTexture.instance.rowToAnimate != 0)
                        {
                            AnimatedTexture.instance.rowToAnimate = 1;
                        }
                        }
	                if (currentLoudestStation == null
	                    	|| stations[i].GetComponent<AudioSource>().volume > currentLoudestStation.GetComponent<AudioSource>().volume) //Keep a value of the current highest volume station
	                {
	                    currentLoudestStation = stations[i];
	                }
	            }
	            else
	            {
	                //Not near a station
                	stations[i].GetComponent<AudioSource>().volume = 0.0f * masterVolume;
                    if (AnimatedTexture.instance.rowToAnimate > 1)
                    {
                        AnimatedTexture.instance.rowToAnimate = 2;
                    }
                    }
        	}
        }
        return currentLoudestStation;
    }
    
    private float setStationVol (StationController station, int rawFrequency) {
    	var stationBandwidth = ToFrequencyRange(station.stationBandwidth);
		float percentageModifier = percentageVolFunc(stationBandwidth, station.frequency, rawFrequency, false);
        return percentageModifier * masterVolume;
    }
    
    private float setStaticVol(StationController currentLoudestStation) {
		float percentageModifier = 1.0f;
		if (currentLoudestStation != null) {
	    	var stationBandwidth = ToFrequencyRange(currentLoudestStation.stationBandwidth);
	    	percentageModifier = percentageVolFunc(stationBandwidth, currentLoudestStation.frequency, currentFrequency, true);
		}
        staticSource.volume = randomiseStaticVol(masterVolume * staticVolModifier * percentageModifier);
        return Math.Min(masterVolume * staticVolModifier, minStaticAugendVol * masterVolume + staticSource.volume);
    }

    private float randomiseStaticVol(float staticVol)
    {
        float randFloat = Convert.ToSingle(new System.Random().NextDouble());
        float newStaticVol = staticVol * (1.0f - percentStaticRand) + staticVol * percentStaticRand * randFloat;
        return newStaticVol;
    }
    
    /**
     * This method applies a function on an exponent value (which represents how close x1 is to x2),
     * to calculate a "y" of this function, and then finally returns the fraction of "y" out of the
     * maximum possible "y" value
     * 
     */
    private float percentageVolFunc(int maxExponent, int x1, int x2, Boolean inverted) {
    	float offset = Convert.ToSingle(Math.Pow(interferenceBase, 0));
    	float exponent = maxExponent - Convert.ToSingle(Math.Abs(x1 - x2));
    	if (inverted) {
    		exponent = maxExponent - exponent;
    	}
        float y = Convert.ToSingle(Math.Pow(interferenceBase, exponent)) - offset;
        float maxY = Convert.ToSingle(Math.Pow(interferenceBase, maxExponent)) - offset;
        float percentageModifier = y / maxY;
    	return percentageModifier;
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
