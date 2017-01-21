using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeRecorder : MonoBehaviour
{

    public static TapeRecorder instance;

    private bool playing = false;
    public GameObject[] buttons;
    private int currentButton = 3;

    [Header("Recording Components")]
    public AudioSource[] externalSources;
    public AudioSource source;
    public List<AudioClip> sourceClips = new List<AudioClip>();
    public int currentClip = 0;
    private float timestamp = 0;

    void Start()
    {
        source = GetComponent<AudioSource>();
        currentButton = 5;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        CheckClicked();
        if (playing)
        {
            Playback();
        }
    }

    void Playback()
    {
        if (!source.isPlaying)
        {
            for (int i = 0; i < sourceClips.Count; i++)
            {
                if (sourceClips[i] == source.clip)
                {
                    currentClip = i + 1;
                }
            }
            if (currentClip == sourceClips.Count)
            {
                currentClip = 0;
            }
            source.clip = sourceClips[currentClip];
            source.Play();
        }
    }

    void Record()
    {
        int selectedStation = 0;
        source.volume = 0;
        for (int i = 0; i < externalSources.Length; i++)
        {
            if (externalSources[i].volume > externalSources[selectedStation].volume)
            {
                selectedStation = i;
                source.volume = externalSources[selectedStation].volume;
            }
        }
        for (int i = 0; i < externalSources[selectedStation].GetComponent<StationController>().stationClips.Length; i++)
        {
            sourceClips.Add(externalSources[selectedStation].GetComponent<StationController>().stationClips[i]);
        }
    }

    void CheckClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50.0f))
            {
                GameObject objectHit = hit.collider.gameObject;
                if (objectHit.tag == "Button")
                {
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        
                        if (buttons[i] == objectHit)
                        {
                            if (currentButton < 4)
                            {
                                buttons[currentButton].transform.Translate(new Vector3(0, 0.075f, 0));
                            }

                            switch (i)
                            {
                                case 0: //Record 
                                    if (currentButton == 1 && !playing)
                                    {
                                        buttons[currentButton].transform.Translate(new Vector3(0, -0.075f, 0));
                                    }
                                    playing = false;
                                    timestamp = 0;
                                    for (int k = 0; k < sourceClips.Count; k++)
                                    {
                                        sourceClips.Clear();
                                    }
                                    Record();
                                    source.Stop();
                                    break;
                                case 1: //Play and pause
                                    if (!playing)
                                    {
                                        if (sourceClips.Count > 0)
                                        {
                                            if(currentButton == i)
                                            {
                                                buttons[currentButton].transform.Translate(new Vector3(0, -0.075f, 0));
                                            }
                                            playing = true;
                                            source.clip = sourceClips[currentClip];
                                            source.Play();
                                            source.time = timestamp;
                                        }
                                    }
                                    else if (currentButton == i && playing)
                                    {
                                        playing = false;
                                        timestamp = source.time;
                                        source.Stop();
                                        buttons[currentButton].transform.Translate(new Vector3(0, 0.075f, 0));
                                    }
                                    break;
                                case 2: //Stop
                                    if(currentButton == 1 && !playing)
                                    {
                                        buttons[currentButton].transform.Translate(new Vector3(0, -0.075f, 0));
                                    }
                                    playing = false;
                                    timestamp = source.time;
                                    source.Stop();
                                    break;
                                case 3: //Rewind
                                    if (currentButton == 1 && !playing)
                                    {
                                        buttons[currentButton].transform.Translate(new Vector3(0, -0.075f, 0));
                                    }
                                    currentClip = 0;
                                    playing = false;
                                    timestamp = 0;
                                    source.Stop();
                                    break;
                                default:
                                    break;
                            }
                            currentButton = i;
                            buttons[currentButton].transform.Translate(new Vector3(0, -0.075f, 0));
                        }
                    }
                }

            }
        }
    }
}
