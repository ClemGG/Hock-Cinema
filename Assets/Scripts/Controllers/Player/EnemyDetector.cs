using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;

public class EnemyDetector : MonoBehaviour
{
    #region Variables

    [Space(10)]
    [Header("Scripts & Components :")]
    [Space(10)]

    NavMeshIA ia;
    Transform t;
    [SerializeField] Volume enemyPPV;

    [Space(10)]
    [Header("Detection :")]
    [Space(10)]

    [SerializeField] bool hasBeenDetected = false;
    [HideInInspector] public bool isDead = false;
    [SerializeField] float detectionDst = 10f, collisionDst = 1.5f;
    [SerializeField] UnityEvent onPlayerCaughtEvent;

    NavMeshPathFollower[] enemies;

    [Space(10)]
    [Header("Gizmos :")]
    [Space(10)]

    [SerializeField] protected bool showGizmos;

    #endregion



    // Start is called before the first frame update
    void Start()
    {
        t = transform;
        ia = t.GetComponent<NavMeshIA>();
        enemies = (NavMeshPathFollower[])FindObjectsOfTypeAll(typeof(NavMeshPathFollower));

        //On active le PPV dans la cutscene où la scène se change en noir et blanc
        if(enemyPPV) enemyPPV.gameObject.SetActive(true);
    }


    private void FixedUpdate()
    {
        //Si aucun ennemi n'est activé, alors le joueur n'a pas encore atteint le point de gameplay et on désactive le PPV de l'ennemi
        if (enemies[0].gameObject.activeInHierarchy)
        { 

            #region Enemy detection

            float dst = (GetClosestEnemyPos() - t.position).sqrMagnitude;
            //print(dst);

            hasBeenDetected = dst < detectionDst * detectionDst;
            enemyPPV.weight = Mathf.Lerp(3f, 0f, dst / (detectionDst * detectionDst));

            if (dst < collisionDst * collisionDst && !isDead)
            {
                onPlayerCaughtEvent?.Invoke();
                isDead = true;
                //SceneFader.instance.FadeToScene(SceneFader.GetCurSceneIndex());
                //print("Collision avec le joueur, on démarre le combat.");
            }

            #endregion

        }
        else
        {
            enemyPPV.weight = 0f;
        }
    }

    private Vector3 GetClosestEnemyPos()
    {
        float closestDst = Mathf.Infinity;
        int index = 0;


        for (int i = 0; i < enemies.Length; i++)
        {
            float dst = (t.position - enemies[i].transform.position).sqrMagnitude;
            if(dst < closestDst)
            {
                closestDst = dst;
                index = i;
            }
        }

        return enemies[index].transform.position;
    }


    //Appelée depuis le checkpoint Manager pour permettre à l'event d'être à nouveau appelée
    public void ResetIsDead()
    {
        isDead = false;
    }



#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Gizmos.color = hasBeenDetected ? Color.yellow : Color.blue;
        Gizmos.DrawWireSphere(transform.position, collisionDst);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionDst);


    }

#endif
}
