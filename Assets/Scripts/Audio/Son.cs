using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Son
{
    public string tag;
    public AudioClip clip;
    [HideInInspector] public AudioClip lastClip;    //Pour nous permettre de changer le tag
    [HideInInspector] public AudioSource source;

    [Range(0f, 1f)] public float volume = .5f;

    public bool loop, playOnAwake;

    

}
