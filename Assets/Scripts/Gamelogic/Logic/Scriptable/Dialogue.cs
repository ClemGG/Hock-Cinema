using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Dialogue", menuName = "Gameplay/Dialogue")]
public class Dialogue : ScriptableObject
{
    public bool useTalkie;
    public AudioClip dialogueClip;
    public Replique[] repliques;
}

[System.Serializable]
public class Replique
{
    public string characterName;
    [TextArea(1,3)] public string text;
    public float textOnScreenDuration = 4f;
}
