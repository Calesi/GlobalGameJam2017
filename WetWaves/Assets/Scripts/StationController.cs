using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour
{

    public enum bandwidth
    {
        Low,
        Medium,
        High
    }

    public AudioClip[] stationClips;
    public bool randomiseClipOrder = false;
    public bandwidth stationBandwidth = bandwidth.High;
    public int frequency = 0;

    private AudioSource source;

    public AudioSource GetSource()
    {
        return this.source;
    }
    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying)
        {
            if (randomiseClipOrder)
            {
                RandomiseClipOrder();
            }
            else
            {
                PlayNextClip();
            }
        }
    }

    void PlayNextClip()
    {
        int currentClip = 0;
        for (int i = 0; i < stationClips.Length; i++)
        {
            if (stationClips[i] == source.clip)
            {
                currentClip = i + 1;
            }
        }
        if (currentClip >= stationClips.Length)
        {
            currentClip = 0;
        }
        source.clip = stationClips[currentClip];
        source.Play();
    }

    void RandomiseClipOrder()
    {
        int rand = Random.Range(0, stationClips.Length);
        source.clip = stationClips[rand];
        source.Play();
    }
}
