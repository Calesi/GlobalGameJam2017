using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSoundsController : MonoBehaviour {

    public static BackgroundSoundsController instance;

    public AudioSource soundEffectsSource;
    public AudioClip[] soundEffectsClips;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

	public void ButtonClick()
    {
        soundEffectsSource.clip = soundEffectsClips[0];
        soundEffectsSource.Play();
    }

    public void PaperPickUp()
    {
        soundEffectsSource.clip = soundEffectsClips[1];
        soundEffectsSource.Play();
    }

    public void PaperPutDown()
    {
        soundEffectsSource.clip = soundEffectsClips[2];
        soundEffectsSource.Play();
    }
}
