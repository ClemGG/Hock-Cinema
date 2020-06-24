using UnityEngine;
using UnityEngine.SceneManagement;

public class NavMeshPlayerDetector : MonoBehaviour
{
    [Space(10)]
    [Header("Scripts & Components :")]
    [Space(10)]

    NavMeshIA ia;
    Transform t;

    [Space(10)]
    [Header("Detection :")]
    [Space(10)]

    [SerializeField] float detectionDst = 10f, collisionDst = 1.5f; 
    [SerializeField] bool hasDetectedPlayer = false;

    [SerializeField] float lostContactDelay = 5f;

    float _lostContactTimer;

    bool shouldRaycast;
    float dst;
    bool eyeContact;
    bool contactLost = false;

    [Space(10)]
    [Header("Gizmos :")]
    [Space(10)]

    [SerializeField] protected bool showGizmos;



    // Start is called before the first frame update
    void Start()
    {
        t = transform;
        ia = t.GetComponent<NavMeshIA>();
        _lostContactTimer = lostContactDelay;
    }


    private void FixedUpdate()
    {

        if (shouldRaycast)
        {
            //On utilise un raycast pour déterminer si un obstacle se trouve entre le joueur et l'ennemi. Si ce n'est pas le cas, l'ennemi attaque.
            //On doit affecter une grande valeur à detectionDst.
            Ray r = new Ray(t.position, PlayerController.t.position);
            Physics.Raycast(t.position, (PlayerController.t.position - t.position), out RaycastHit hit, detectionDst);
            eyeContact = hit.collider.gameObject.layer == LayerMask.NameToLayer("Entity/Player");
            //print($"{name} : {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
        }
        else
        {
            eyeContact = false;
        }

        if (SoundDetector.instance.PlayerIsAudible() && ia.CanReachPlayer())
        {
            eyeContact = true;
        }

        //Si l'ennemi perd de vue le joueur, on lui laisse quelques secondes pour le suivre et le retrouver dans son champ de vision.
        //S'il ne le rattrape pas, l'ennemi perd contact avec le joueur et reprend sa patrouille
        if (eyeContact)
        {
            _lostContactTimer = 0f;
            contactLost = false;
        }
        else if(!eyeContact || PlayerController.isSafe)
        {
            if (_lostContactTimer < lostContactDelay)
            {
                _lostContactTimer += Time.fixedDeltaTime;
            }
            else
            {
                contactLost = true;
            }
        }

        ia.isChasingPlayer = hasDetectedPlayer = !contactLost && !PlayerController.isSafe;
        //print($"contactLost : {contactLost} ; ia.isChasingPlayer : {ia.isChasingPlayer} ; hasDetectedPlayer : {hasDetectedPlayer}");
        //print(eyeContact);


        /* On ne calcule pas la collision avec des Colliders pour éviter que les agents ne se poussent.
         * De plus, OnCollisionEnter nécessite que les Rigibody soient non kinematic, ce qui empêche les IAs de monter les pentes
         * Les Colliders sont donc inutiles puisque les agents ont l'obstacle avoidance activé
         */
        dst = (PlayerController.t.position - t.position).sqrMagnitude;
        if (dst < collisionDst * collisionDst)
        {
            //SceneFader.instance.FadeToScene(SceneFader.GetCurSceneIndex());
            //print("Collision avec le joueur, on démarre le combat.");
        }
    }


    //Quand le joueur entre et sort de la zone de détection de l'ennemi
    private void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.layer == LayerMask.NameToLayer("Entity/Player"))
        {
            shouldRaycast = true;
        }


    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("Entity/Player"))
        {
            shouldRaycast = false;
        }
    }



#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Gizmos.color = hasDetectedPlayer ? Color.yellow : Color.blue;
        Gizmos.DrawWireSphere(transform.position, collisionDst);

        if (Application.isPlaying)
        {
            Gizmos.color = eyeContact ? Color.red : !contactLost ? Color.yellow : Color.green;
            Gizmos.DrawLine(transform.position, PlayerController.t.position);
        }
        

    }

#endif
}
