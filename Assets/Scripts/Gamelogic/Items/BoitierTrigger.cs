using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


//Quand cet objet sera réparé, l'event OnRepaired() va activer tous les GameObjects du Réseau (cams, portes, lumières)
//Ce sera alors au trigger du Réseau d'assigner lui-même les caméras à la tablettes
public class BoitierTrigger : InteractableTrigger
{


    [Space(10)]
    [Header("Scripts & Components :")]
    [Space(10)]


    [SerializeField] Renderer ledRenderer;
    [SerializeField] Material matLedOn;
    [SerializeField] Image fillRepair;
    [SerializeField] GameObject fillCanvas;


    [Space(10)]
    [Header("Repair info :")]
    [Space(10)]


    //[SerializeField] bool destroyOnStart = true;    //Est-ce que la caméra doit être réparée dès le début ?
    [SerializeField] float delayToRepair = 3f;
    [SerializeField] float repairSpeed = 1f;
    float _repairTimer;
    bool repaired = false;

    [Space(10)]

    [SerializeField] UnityEvent onRepaired; //Quand la cam est réparée, on affiche les UIs des cams de sécurité à l'écran
    [SerializeField] UnityEvent onMouseDownDestroyed; //Quand le boîtier nécessite un objet à réparer, un indique au joueur comme objectif de chercher cet objet

    

    public void Start()
    {
        //if (rend)
        //    rend.material = destroyedMat;

        fillRepair.fillAmount = 0f;
        fillCanvas.SetActive(false);
        fillCanvas.SetActive(true);

    }

    public void Repair()
    {
        CheckRequiredItems();
        if(_repairTimer <= 0f)
        {
            //print("start repair"); 
            if(!repaired && hasRequiredItems)
                fillCanvas.SetActive(true);

        }
        if (hasRequiredItems && !repaired)
        {
            if(_repairTimer < delayToRepair)
            {
                _repairTimer += repairSpeed * Time.deltaTime;
                fillRepair.fillAmount = _repairTimer / delayToRepair;
            }
            else
            {
                repaired = true;
                //fillCanvas.SetActive(false);  

                //A faire : Afficher les LED
                ledRenderer.materials = new Material[] { ledRenderer.materials[0], matLedOn };

                _repairTimer = 0f;


                PrintInteractionOnScreen();
                onRepaired?.Invoke();

                //print("repair done");
            }
        }
    }



    //Pour savoir si on doit afficher l'icône d'interaction de l'Ui ou non
    public override bool HasBeenInteracted()
    {
        return repaired;
    }

    Son loadingSound;
    public void PlayLoadingSound(AudioClip clip)
    {
        if(loadingSound == null)
            loadingSound = AudioManager.instance.GetSonFromClip(clip);


        if(hasRequiredItems && !repaired && !PlayerController.isOnTablet && !PlayerController.isOnTalkie)
        {
            AudioManager.instance.Play(clip);
            StartCoroutine(ChangePitchCo());

        }
        else
        {
            AudioManager.instance.Stop(clip);
        }
    }

    private IEnumerator ChangePitchCo()
    {
        //Pour pouvoir retourner dans la boucle au prochain boîtier
        loadingSound.source.pitch = 1f;

        while (loadingSound.source.pitch < 1.2f)
        {

            loadingSound.source.pitch = Mathf.Lerp(.9f, 1.2f, fillRepair.fillAmount);
            //print(loadingSound.source.pitch);
            yield return null;
        }

    }

    public void MouseUp()
    {
        if (!repaired)
        {
            _repairTimer = fillRepair.fillAmount = 0f;
        }
            
    }

    public void MouseDown()
    {
        if (!hasRequiredItems)
        {
            PrintInteractionOnScreen();
            onMouseDownDestroyed?.Invoke();

        }
    }
}
