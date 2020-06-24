using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackCanvas : MonoBehaviour
{

    public LocalizedComponent interactionLocalizedComponent;

    [Space(20)]

    public Image interactIcon, cartoucheDialogue;
    public TextMeshProUGUI interactionText;

    public TextMeshProUGUI currentObjectiveText;

    public TextMeshProUGUI newObjectiveText;
    public TextMeshProUGUI dialogueText;

    public static FeedbackCanvas instance;

    Coroutine newObjCo, interactionCo;


    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }


    private void Start()
    {
        interactIcon.enabled = false;
        cartoucheDialogue.enabled = false;
        currentObjectiveText.enabled = false;
        interactionText.enabled = false;
        dialogueText.gameObject.SetActive(false);
        newObjectiveText.enabled = false;

    }





    //Pour afficher en bas de l'écran le nouvel objectif à remplir

    public void PrintNewObjective(string bottomText, string topLeftText, float duration)
    {
        if(newObjCo != null)
        {
            StopCoroutine(newObjCo);
        }

        if (!string.IsNullOrEmpty(bottomText))
        {
            cartoucheDialogue.enabled = true;
        }

        newObjCo = StartCoroutine(PrintNewObjectiveCo(bottomText, duration));
        DisplayNewObjective(topLeftText);
    }

    private IEnumerator PrintNewObjectiveCo(string text, float duration)
    {
        newObjectiveText.enabled = true;
        newObjectiveText.text = text;

        yield return new WaitForSeconds(duration);

        cartoucheDialogue.enabled = false;
        newObjectiveText.enabled = false;
        newObjCo = null;
    }




    //Pour afficher le dialogue au talkie

    public void PrintDialogue(string repliqueText)
    {
        //Pour éviter que le cartouche ne se barre
        if (newObjCo != null)
        {
            StopCoroutine(newObjCo);
            newObjCo = null;
        }

        dialogueText.gameObject.SetActive(true);
        dialogueText.text = repliqueText;
    }



    //Pour afficher en bas de l'écran une action qui vient d'être exécutée

    public void PrintInteraction(string text)
    {
        if (interactionCo != null)
        {
            StopCoroutine(interactionCo);
        }
        interactionCo = StartCoroutine(PrintInteractionCo(text));

    }

    private IEnumerator PrintInteractionCo(string translationTag)
    {
        interactionText.enabled = true;

        //Au lieu de changer directement le texte, on passe par le LocalizedComponent à chaque fois qu'un boîtier a besoin d'écrire
        //sa ligne.
        //interactionText.text = text;
        interactionLocalizedComponent.AssignNewComponentData<string>(translationTag, false, true);

        yield return new WaitForSeconds(5f);

        interactionText.enabled = false;

        interactionCo = null;
    }





    //Pour afficher le nouvel objectif en haut à gauche de l'écran
    public void DisplayNewObjective(string topLeftText)
    {
        currentObjectiveText.enabled = true;
        //objectiveScoreText.enabled = true;

        currentObjectiveText.text = topLeftText;
    }
}
