using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundDetector : MonoBehaviour
{
    public static SoundDetector instance;
    NavMeshPlayerDetector[] enemies;

    [Space(10)]
    [Header("Audio :")]
    [Space(10)]

    [SerializeField] Vector2 minMaxGresillementVolume = new Vector2(0f, .75f);
    [SerializeField] float maxGresillementDst = 30f, gresillementEnemyDetection = 20f;
    [SerializeField] AudioClip gresillementClip;
    Son gresillementSon;

    [Space(10)]
    [Header("Gizmos :")]
    [Space(10)]

    [SerializeField] protected bool showGizmos;




    private void Awake()
    {
        instance = this;
        //gameObject.SetActive(false);
    }

    private void Start()
    {
        enemies = (NavMeshPlayerDetector[])FindObjectsOfTypeAll(typeof(NavMeshPlayerDetector));

        //Pour ne pas avoir à la rappeler à chaque frame
        InvokeRepeating("GetClosestEnemyDstToPlayer", 0f, .25f);


        AudioManager.instance.Play(gresillementClip);
        gresillementSon = AudioManager.instance.GetSonFromClip(gresillementClip);
    }



    // Update is called once per frame
    void Update()
    {
        if (PlayerController.hasTablet)
        {
            //Si on n'a pas le talkie, on ne met pas le bruit de grésillement
            float dst = PlayerController.hasTablet ? GetClosestEnemyDstToPlayer() / maxGresillementDst : Mathf.Infinity;
            gresillementSon.source.volume = Mathf.Lerp(minMaxGresillementVolume.y, minMaxGresillementVolume.x, dst);
        }
    }

    public float GetClosestEnemyDstToPlayer()
    {

        float shortestDst = Mathf.Infinity;
        for (int i = 0; i < enemies.Length; i++)
        {
            float curDst = (enemies[i].transform.position - PlayerController.t.position).magnitude;
            if(curDst < shortestDst)
            {
                shortestDst = curDst;
            }
        }

        return shortestDst;
    }


    public bool PlayerIsAudible()
    {
        return GetClosestEnemyDstToPlayer() < gresillementEnemyDetection && (gameObject.activeSelf && PlayerController.hasTablet || PlayerController.isRunning);
    }


#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, gresillementEnemyDetection);

    }

#endif
}
