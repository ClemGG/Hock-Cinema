using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class InteractableTrigger : MonoBehaviour
{
    [Space(10)]
    [Header("Trigger : ")]
    [Space(10)]

    [HideInInspector] public bool hasRequiredItems = true;
    [TextArea(1, 3)] [SerializeField] protected string hasItemsText, needsItemsText;


    [Space(10)]
    [Header("Components : ")]
    [Space(10)]


    [SerializeField] protected CollectibleTrigger[] linkedCollectibles;
    public UnityEvent onInteractedEvent, onMouseDownEvent, onMouseUpEvent;    //Appelée par le playerController pour activer les mécanismes (cams, portes...)


    private void Start()
    {
        CheckRequiredItems();
    }


    public void CheckRequiredItems()
    {
        hasRequiredItems = true;


        for (int i = 0; i < linkedCollectibles.Length; i++)
        {
            if (!linkedCollectibles[i].collected)
            {
                hasRequiredItems = false;
                break;
            }
        }


    }

    public abstract bool HasBeenInteracted();



    //Affiche une ligne à l'écran indiquant l'action exécutée
    public virtual void PrintInteractionOnScreen()
    {
        FeedbackCanvas.instance.PrintInteraction(hasRequiredItems ? hasItemsText : needsItemsText);
    }



}
