using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityStandardAssets.Characters.ThirdPerson;

public abstract class NavMeshIA : MonoBehaviour
{
    [Space(10)]
    [Header("Scripts & Components :")]
    [Space(10)]

    protected ThirdPersonCharacter tpc;
    protected NavMeshAgent ia;
    protected Transform t;

    [Space(10)]
    [Header("IA :")]
    [Space(10)]

    public bool debug = false;  //Pour empêcher l'ennemi de bouger

    public bool isChasingPlayer = false;
    [SerializeField] protected bool hasReachedDestination = false;
    [SerializeField] protected bool isPlayerOutOfReach = true;
    protected Vector3 dest;



    [Space(10)]
    [Header("Events :")]
    [Space(10)]



    public UnityEvent onIaDestinationReached;
    public UnityEvent onPlayerOutOfReach;




    protected virtual void Start()
    {
        t = transform;
        ia = GetComponent<NavMeshAgent>();
        ia.updateRotation = false;
        tpc = GetComponent<ThirdPersonCharacter>();

        //onPlayerOutOfReach += OnPlayerOutOfReach;
        onPlayerOutOfReach.AddListener(OnPlayerOutOfReach);
    }


    protected virtual void FixedUpdate()
    {
        if (debug)
            return;

        /* La bool isChasingPlayer est remplie quand le joueur entre dans la zone de détection de l'ennemi (s'il en a une)
         */

        if (!isChasingPlayer)
        {
            /*Si le joueur est poursuivi et qu'il quitte la zone de détection de l'IA, celle-ci doit s'arrêter immédiatement.
             * Donc on place dest à la position de l'IA
             */
            if (!isPlayerOutOfReach)
            {
                onPlayerOutOfReach?.Invoke();
            }
            isPlayerOutOfReach = true;

            CalculatePatrolPath();

        }
        else
        {
            isPlayerOutOfReach = false;
            CalculatePathToPlayer();
        }

        MoveToDestination();


    }

    protected abstract void CalculatePathToPlayer();
    protected abstract void CalculatePatrolPath();
    protected virtual void MoveToDestination()
    {
        NavMeshPath np = new NavMeshPath();
        ia.CalculatePath(dest, np);

        //Si le joueur est derrière une porte, l'ennemi continue sa patrouille
        //On le modifiera plus tard si jamais on en a besoin pour le faire ouvrir les portes et poursuivre le joueur
        if (np.status == NavMeshPathStatus.PathComplete)
            ia.SetDestination(dest);
        else
        {
            CalculatePatrolPath();
            ia.CalculatePath(dest, np);

        }



        if (ia.remainingDistance > ia.stoppingDistance)
        {
            tpc.Move(ia.desiredVelocity, false, false);
            /* La vélocité prend en compte les animations du personnage. Donc si on veut qu'il ralentisse quand la souris est proche
             * il faut une animation de marche.
             */
            hasReachedDestination = false;
        }
        else
        {
            tpc.Move(Vector3.zero, false, false);
            //Pour indiquer au script si l'IA a atteint sa destination

            
            if (!hasReachedDestination)
            {
                onIaDestinationReached?.Invoke();
            }
            hasReachedDestination = true;
        }


    }

    protected virtual void OnPlayerOutOfReach()
    {
        dest = t.position;
    }

    public bool CanReachPlayer()
    {
        NavMeshPath np = new NavMeshPath();
        ia.CalculatePath(PlayerController.t.position, np);

        return np.status == NavMeshPathStatus.PathComplete;
    }

}
