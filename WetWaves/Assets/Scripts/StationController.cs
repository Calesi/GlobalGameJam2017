using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour {

    public enum bandwidth
    {
        Low,
        Medium,
        High
    }

    public AudioClip[] stationClips;
    public bool randomiseClipOrder = false;
    public bandwidth stationBandWidth = bandwidth.High;
    public int frequency = 0;

    private AudioSource source;


	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
