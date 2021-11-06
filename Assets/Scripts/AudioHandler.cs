using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    private Dictionary<string, AudioSource> AudioSources = new Dictionary<string, AudioSource>();
    
    // Start is called before the first frame update
    void Start()
    {
        var sourceList = gameObject.GetComponents<AudioSource>();
        AudioSources.Add("score", sourceList[0]);
        AudioSources.Add("lose", sourceList[1]);
        AudioSources.Add("ambient_piano", sourceList[2]);
        AudioSources.Add("button_press", sourceList[3]);
        AudioSources.Add("ambient_jungle", sourceList[4]);
        AudioSources.Add("lose_life", sourceList[5]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(string clip)
    {
        AudioSource source = AudioSources[clip];
        source.Play();
    }
}
