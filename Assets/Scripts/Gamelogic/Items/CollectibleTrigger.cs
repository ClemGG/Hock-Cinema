using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectibleTrigger : MonoBehaviour
{
    [Space(10)]
    [Header("Scripts & Components :")]
    [Space(10)]

    [SerializeField] InteractableTrigger linkedMecanism;
    GameObject itemObject;

    [Space(10)]
    [Header("Info :")]
    [Space(10)]

    [SerializeField] string pickedUpText = "Vous avez ramassé...";
    [SerializeField] UnityEvent onPickedUpEvent;
    [HideInInspector] public bool collected = false;


    // Start is called before the first frame update
    void Start()
    {
        itemObject = transform.GetChild(0).gameObject;
        itemObject.SetActive(true);
        collected = false;
    }


    public void Pickup()
    {
        if (!collected)
        {
            itemObject.SetActive(false);
            collected = true;

            if(linkedMecanism)
                linkedMecanism.CheckRequiredItems();

            PrintInteractionOnScreen();

            onPickedUpEvent?.Invoke();
            gameObject.SetActive(false);    //Pour éviter que le trigger ne fasse chier les autres
        }

    }


    public void PrintInteractionOnScreen()
    {
        FeedbackCanvas.instance.PrintInteraction(pickedUpText);
    }


}
