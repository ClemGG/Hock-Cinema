using PathCreation;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static Clement.Utilities.Maths;

public class NavMeshPathFollower : NavMeshIA
{

    [Space(10)]
    [Header("Path :")]
    [Space(10)]
    
    [SerializeField] protected PathCreator pathToFollow;
    [SerializeField] protected EndOfPathInstruction endOfPathInstruction;
    [SerializeField] protected PathCreator[] avaliablePaths;
    float distanceTravelled;
    Vector3 startPos;
    int currentPathIndex = -1;      //Pour empêcher l'IA de reprendre le même chemin

    [Space(10)]
    [Header("IA :")]
    [Space(10)]

    [SerializeField] float stopDelay = 3f;
    float _stopTimer;
    bool stopped;

    [SerializeField] Vector2 minMaxRandomStopDelay = new Vector2(10f, 20f);
    [SerializeField] UnityEvent onRandomStopEvent;
    float _randomStopDelay = 0f;
    float _randomStopTimer;
    Coroutine randomStopCo;



    protected override void Start()
    {
        base.Start();
        startPos = t.position;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    print($"{name} : {startPos}");
        //    ResetPosition();
        //}
    }


    //Appelée par le checkpoint manager quand le joueur se fait attraper
    public void ResetPosition()
    {

        ia.enabled = false;
        distanceTravelled = 0f;
        dest = pathToFollow.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        t.position = startPos;
        ia.enabled = true;
    }




    protected override void CalculatePathToPlayer()
    {
        /* On récupère à chaque frame la position du joueur pour le suivre en continu.
         */
        dest = PlayerController.t.position;
    }

    protected override void CalculatePatrolPath()
    {
        /* On récupère uniquement les points échantillonnés par le PathCreator
         */
        //print($"{name} : {stopped}");
        if(pathToFollow && !stopped)
        {
            distanceTravelled += ia.speed * Time.fixedDeltaTime;
            dest = pathToFollow.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        }

        if(_randomStopTimer < _randomStopDelay)
        {
            _randomStopTimer += Time.fixedDeltaTime;
        }
        else
        {
            if(randomStopCo == null) randomStopCo = StartCoroutine(OnRandomDelay());
        }
    }




    protected override void MoveToDestination()
    {
        if(!stopped)
        base.MoveToDestination();
    }





    IEnumerator OnRandomDelay()
    {
        //print("random fini");

        yield return StartCoroutine(StopEnemy());


        _randomStopTimer = 0f;
        _randomStopDelay = _randomStopDelay.RandomV2(minMaxRandomStopDelay);
        randomStopCo = null;
        //print("random reprend");
    }



    //Quand le joueur est hors de portée de l'ennemi, on fait s'arrêter celui-ci pendant
    //quelques secondes avant de reprendre son chemin
    protected override void OnPlayerOutOfReach()
    {

        StartCoroutine(StopEnemy());
    }

    //Utilisée quand le joueur est hors de portée et pendant les patrouilles de l'ennemi pour
    //lui donner un comportement plus imprévisible
    private IEnumerator StopEnemy()
    {
        //print("L'ennemi s'arrête");
        _stopTimer = 0f;
        stopped = true;

        while (!isChasingPlayer && _stopTimer < stopDelay)
        {
            _stopTimer += Time.fixedDeltaTime;
            yield return null;
        }

        stopped = false;


        onRandomStopEvent?.Invoke();
        //print("L'ennemi repart");

    }


    //dans le onRandomStopEvent, on peut choisir un circuit aléatoire si l'ennemi doit explorer plusieurs salles déconnectées
    //les unes des autres
    public void ChangePathRandomly()
    {

        int alea = 0;
        while(alea == currentPathIndex)
        {
            alea = Random.Range(0, avaliablePaths.Length);
        }
        pathToFollow = avaliablePaths[alea];
        currentPathIndex = alea;
    }


    //dans le onRandomStopEvent, on peut choisir de faire boucler l'ennemi ou de le faire repartir en arrière
    public void ChangeInstructionRandomly()
    {
        endOfPathInstruction = (EndOfPathInstruction)Random.Range(0, 1);  //On choisit aléatoirement entre reprendre le chemin et faire demi-tour

    }
}
