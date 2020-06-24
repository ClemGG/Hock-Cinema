using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reseau : MonoBehaviour
{
    public BoitierTrigger linkedBoitier;
    public bool reseauActif = false;

    public SecurityCam[] camsToActivate;
    public DoorTrigger[] doorsToActivate;
    public Light[] lightsToActivate;



    //Appelée par le boîtier quand il est réparé.
    //Les caméras sont activées par le RéseauManager quand le joueur entre dans le trigger du Réseau
    public void ActiveReseau(bool active)
    {

        reseauActif = active;

        for (int i = 0; i < doorsToActivate.Length; i++)
        {
            if(active)
            doorsToActivate[i].UnlockDoor();
        }
        for (int i = 0; i < lightsToActivate.Length; i++)
        {
            lightsToActivate[i].enabled = active;
        }

    }


    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Entity/Player"))
        {
            if (linkedBoitier.HasBeenInteracted())
            {
                ReseauManager.instance.EnableReseau(this);
            }
        }
    }

}
