using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Objective", menuName = "Gameplay/Objective")]
[System.Serializable]
public class Objective : ScriptableObject
{

    //Le nom en bas de l'écran
    [TextArea(2, 3)] public string bottomtext = "New Objective : \n";

    //Le nom en haut à gauche de l'écran
    [TextArea(2, 3)] public string topLeftText;

    //Le nombre de secondes pendant lesquelles l'objectif sera affiché en bas de l'écran
    public float displayOnScreenDuration = 5f;

}
