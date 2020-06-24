using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class DoorTrigger : InteractableTrigger
{

    [Space(10)]
    [Header("Scripts & Components :")]
    [Space(10)]


    MeshRenderer[] rends;
  
    NavMeshLink navMeshLink;
    Animator doorAnim;


    [Space(10)]
    [Header("Porte :")]
    [Space(10)]

    [SerializeField] UnityEvent onOpenedEvent, onClosedEvent, onLockedEvent;
    [SerializeField] bool unlockOnStart = false;    //Si c'est à true, la porte n'est pas reliée à un Réseau et s'ouvre seule. Si elle a un objet, on la déverouille quand on clique dessus

    bool opened = false;
    bool unlocked = false;




    private void Start()
    {
        rends = GetComponentsInChildren<MeshRenderer>();
        navMeshLink = GetComponent<NavMeshLink>();
        if (navMeshLink) navMeshLink.enabled = false;
        doorAnim = GetComponent<Animator>();


        if (unlockOnStart)
        {
            UnlockDoor();
        }

    }


    public override bool HasBeenInteracted()
    {
        //return opened;
        return false;
    }

    public void OpenDoor()
    {
        CheckRequiredItems();

        //Si on a des objets, on veut débloquer la porte quand on clique dessus
        if (!unlocked && hasRequiredItems && linkedCollectibles.Length > 0)
        {
            UnlockDoor();
            return;
        }

        if (unlocked && hasRequiredItems)
        {
            //Si l'anim de la porte est terminée, alors on peut la relancer
            if (doorAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > .99f)
            {
                doorAnim.Play(opened ? "a_close_door" : "a_open_door");
                ANIM_OpenDoor();
            }
        }
        else
        {
            PrintInteractionOnScreen();
        }


    }

    public void UnlockDoor()
    {

        unlocked = true;
        if (navMeshLink) navMeshLink.enabled = true;


    }

    //Appelée dans les cutscenes pour refermer les portes
    public void ResetDoor()
    {
        opened = false;
        foreach (Transform child in transform)
        {
            child.GetComponent<BoxCollider>().enabled = true;
        }
        doorAnim.Play("a_close_door");
        //unlocked = unlockOnStart;

    }
    

    public void ANIM_OpenDoor()
    {
        //print(opened ? "on ferme" : "on ouvre");
        //TODO : Alterner l'état ouvert et fermé
        //Il suffit juste de remplacer les true et false par !opened mais je l'enlève pour le proto tant qu'on a pas d'anim
        opened = !opened;
        foreach (Transform child in transform)
        {
            //child.GetComponent<BoxCollider>().enabled = !opened;
        }

        if (navMeshLink) navMeshLink.enabled = opened;

        if (opened)
        {
            onOpenedEvent?.Invoke();
        }
        else
        {
            onClosedEvent.Invoke();
        }

        //On fait reculer le joueur pour éviter qu'il ne se prenne la porte dans la tronche
        FindObjectOfType<PlayerController>().MoveAwayFromDoor(transform);

    }



    public override void PrintInteractionOnScreen()
    {
        FeedbackCanvas.instance.PrintInteraction(unlocked && hasRequiredItems ? hasItemsText : needsItemsText);

        if(!unlocked || !hasRequiredItems)
        {
            onLockedEvent?.Invoke();
        }
    }
}
