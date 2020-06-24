using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbianceTrigger : MonoBehaviour
{

    [Space(10)]
    [Header("Audio :")]
    [Space(10)]

    [SerializeField] Vector2 minMaxVolume = new Vector2(0f, .5f);
    [SerializeField] float maxDst = 50f;
    AudioSource source;

    [Space(10)]
    [Header("Gizmos :")]
    [Space(10)]

    [SerializeField] protected bool showGizmos;



    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Plus le joueur sera proche de la source du bruit, plus celui-ci sera fort
        float dst = (transform.position - PlayerController.t.position).sqrMagnitude;

        //On inverse le calcul de la distance pour rester entre 0 et 1
        source.volume = Mathf.Lerp(minMaxVolume.x, minMaxVolume.y, 1 - (dst / (maxDst * maxDst)));
        //print(source.volume);

    }



#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos)
            return;


        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxDst);

        if(PlayerController.t)
        Gizmos.DrawLine(transform.position, PlayerController.t.position);

    }

#endif
}
