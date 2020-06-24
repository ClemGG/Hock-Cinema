using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueAndObjectiveList
{
    public Dialogue DialogueFR, DialogueEN;
    public Objective ObjectiveFR, ObjectiveEN;
}


//Ce script passe après FeedbackCanvas dans l'ordre d'exécution
public class DialogueCaller : MonoBehaviour
{
    [SerializeField] DialogueAndObjectiveList dialogueAndObjectiveList;
    [SerializeField] bool playOnStart;


    [SerializeField] UnityEvent onCalledEvent; //Si jamais on a besoin d'activer des trucs en plus



    private void Start()
    {
        if (playOnStart)
            CallDialogue();
    }


    //Appelé quand le joueur effectue une action précise (générateur réparé, objet ramassé, etc...)
    //Ce script lance un nouveau Dialogue via le DialogueManager et lui passe un Objectif additionnel affiché en haut à gauche de l'écran
    //(Pour indiquer au joueur ce qu'il doit faire)
    public void CallDialogue()
    {
        onCalledEvent?.Invoke();

        string langue = PlayerPrefs.GetString("langue");
        (Dialogue dialogue, Objective objective) =
            (
            langue == "fr" ? dialogueAndObjectiveList.DialogueFR : dialogueAndObjectiveList.DialogueEN,
            langue == "fr" ? dialogueAndObjectiveList.ObjectiveFR : dialogueAndObjectiveList.ObjectiveEN
            );

        DialogueManager.instance.PlayDialogue(dialogue, objective);
        Destroy(this);    //Pour éviter que l'objectif ne se rappelle deux fois
    }

}
