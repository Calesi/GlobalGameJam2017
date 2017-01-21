using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeRecorder : MonoBehaviour
{

    public static TapeRecorder instance;

    public bool recording = false;
    private bool playing = false;
    public GameObject[] buttons;
    private int currentButton = 3;

    [Header("Recording Components")]
    public AudioSource[] externalSources;
    public List<AudioSource> sources = new List<AudioSource>();
    public List<List<AudioClip>> sourceClips = new List<List<AudioClip>>();
    public List<float> startingTimestamp = new List<float>();
    public List<float> endingTimestamp = new List<float>();
    public List<int> currentClips = new List<int>();

    void Start()
    {
        for (int i = 0; i < externalSources.Length; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>() as AudioSource;
            sources.Add(newSource);
            List<AudioClip> newClips = new List<AudioClip>();
            sourceClips.Add(newClips);
            currentClips.Add(0);
        }
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
        if (recording)
        {
            Record();
        }
        if (playing)
        {
            Playback();
        }
    }

    void Playback()
    {

        for (int i = 0; i < sources.Count; i++)
        {
            if (!sources[i].isPlaying)
            {

                currentClips[i]++;
                if (currentClips[i] == sourceClips[i].Count)
                {
                    currentClips[i] = 0;
                }
                sources[i].clip = sourceClips[i][currentClips[i]];
                sources[i].Play();
            }
        }
    }

    void Record()
    {
        for (int i = 0; i < externalSources.Length; i++)
        {
            if (sources[i].clip != externalSources[i].clip)
            {
                sources[i].clip = externalSources[i].clip;
                Debug.Log(externalSources[i].clip.name);
                sourceClips[i].Add(externalSources[i].clip);
            }
            if(i == 0)
            {
                //Set the starting time of the new source to match actual source
                for (int j = 0; j < startingTimestamp.Count; j++)
                {
                    sources[j].time = startingTimestamp[j];
                }                
            }
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
                            buttons[currentButton].transform.Translate(new Vector3(0, 0, -0.25f));
                            currentButton = i;
                            buttons[currentButton].transform.Translate(new Vector3(0, 0, 0.25f));
                            switch (currentButton)
                            {
                                case 0: //Record
                                    playing = false;
                                    recording = true;
                                    //Started Recording, take note of starting times for each source
                                    startingTimestamp.Clear();
                                    for (int k = 0; k < sourceClips.Count; k++)
                                    {
                                        sourceClips[k].Clear();
                                    }
                                    for (int j = 0; j < externalSources.Length; j++)
                                    {
                                        startingTimestamp.Add(externalSources[j].time);
                                        sources[j].volume = externalSources[j].volume;
                                    }
                                    break;
                                case 1: //Play
                                    for (int j = 0; j < sources.Count; j++)
                                    {
                                        sources[j].time = startingTimestamp[j];
                                        sources[j].clip = sourceClips[j][0];
                                        sources[j].Play();
                                    }
                                    endingTimestamp.Clear();
                                    for (int l = 0; l < externalSources.Length; l++)
                                    {
                                        endingTimestamp.Add(externalSources[l].time);
                                    }
                                    playing = true;
                                    recording = false;
                                    break;
                                case 2: //Reset
                                    endingTimestamp.Clear();
                                    for (int l = 0; l < externalSources.Length; l++)
                                    {
                                        endingTimestamp.Add(externalSources[l].time);
                                        sources[l].volume = externalSources[l].volume;
                                        sources[l].Stop();
                                    }
                                    startingTimestamp.Clear();
                                    for (int k = 0; k < sourceClips.Count; k++)
                                    {
                                        sourceClips[k].Clear();
                                    }
                                    playing = false;
                                    recording = false;
                                    break;
                                case 3: //Stop
                                    endingTimestamp.Clear();
                                    for (int l = 0; l < externalSources.Length; l++)
                                    {
                                        endingTimestamp.Add(externalSources[l].time);
                                        sources[l].volume = externalSources[l].volume;
                                        sources[l].Stop();
                                    }
                                    playing = false;
                                    recording = false;
                                    break;
                                default:
                                    playing = false;
                                    recording = false;
                                    break;
                            }
                        }
                    }
                }

            }
        }
    }
}
