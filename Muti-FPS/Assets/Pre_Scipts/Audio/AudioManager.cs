using System;
using UnityEngine;
    
public class AudioManager : MonoBehaviour
{
    
    public Sound[] sounds;
    public static bool IfmuteALL = false;
    public static AudioManager instance;
   
    void Awake()
    {
     
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

    }
    public bool Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        s.source.Play();
        return s.source.isPlaying;
    }
    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        s.source.Pause();
    }
 
         
    public void Mute(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        s.source.mute = true;
    }
    public void UnMute(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        s.source.mute = false;
    }
    public void MuteALL()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            instance.sounds[i].source.mute = true;
        }
        IfmuteALL = true;
    }
    public void UnMuteALL()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            instance.sounds[i].source.mute = false;
        }
        IfmuteALL = false;
    }
}
