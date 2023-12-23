using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {get; private set;}

    public List<Sound> sounds;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    public void PlaySoundByName(string soundName)
    {
        foreach(Sound sound in sounds)
        {
            if(sound.soundName.ToLower().Equals(soundName.ToLower()))
            {
                sound.audioSource.Play();
            }
        }
    }  
}
