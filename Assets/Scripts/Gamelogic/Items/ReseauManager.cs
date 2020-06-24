using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RectInfo
{
    public Rect rect;
    [HideInInspector] public int curCamID;
}

public class ReseauManager : MonoBehaviour
{
    [Space(10)]
    [Header("Scripts & Components : ")]
    [Space(10)]

    //[SerializeField] Transform camsHolder;
    [SerializeField] Reseau[] reseaux;
    [SerializeField] RectInfo[] rectsOnTablette;


    [Space(10)]
    [Header("Reseaux : ")]
    [Space(10)]

    [SerializeField] bool displayReseauOnStart = false;
    [SerializeField] int ID_ReseauOnStart = 0;
    //[SerializeField] int[] camsToDisplayOnStart;

    //int curRectIndex = 0;

    public static ReseauManager instance;
    public static Reseau curActiveReseau;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;


        for (int i = 0; i < rectsOnTablette.Length; i++)
        {
            rectsOnTablette[i].curCamID = -1;
        }
    }

    private void Start()
    {
        if (displayReseauOnStart)
        {
            EnableReseau(reseaux[ID_ReseauOnStart]);
        }

        
    }






    //Appelée depuis le Réseau pour afficher les caméras quand le joueur entre dans le trigger du Réseau
    public void ActiverCamerasDuReseau(Reseau reseauToEnable, bool active)
    {
        for (int i = 0; i < reseauToEnable.camsToActivate.Length; i++)
        {
            reseauToEnable.camsToActivate[i].EnableCam(active);
            reseauToEnable.camsToActivate[i].SetupCam(rectsOnTablette[i].rect);
        }
    }


    public void EnableReseau(Reseau reseauToEnable)
    {
        curActiveReseau = reseauToEnable;

        foreach (Reseau r in reseaux)
        {
            bool active = r.GetInstanceID() == reseauToEnable.GetInstanceID();
            r.ActiveReseau(active);
            ActiverCamerasDuReseau(r, active);
        }


        //foreach (Transform child in camsHolder)
        //{
        //    child.gameObject.SetActive(false);
        //    child.GetChild(0).gameObject.SetActive(false);
        //    for (int i = 0; i < reseauToEnable.camsToActivate.Length; i++)
        //    {
        //        if (child.GetSiblingIndex() == i)
        //        {
        //            child.gameObject.SetActive(true);
        //            Camera camToSet = child.GetComponent<Camera>();
        //            camToSet.rect = rectsOnTablette[i].rect;
        //            rectsOnTablette[i].curCamID = i;

        //            break;
        //        }

        //        //triggers[i].OnSet(true);
        //    }
        //}
    }

    public void DisableOtherReseaux()
    {
        foreach (Reseau r in reseaux)
        {
            bool current = r.GetInstanceID() == curActiveReseau.GetInstanceID();

            if (!current)
            {
                ActiverCamerasDuReseau(r, false);
            }

        }


        //foreach (Transform child in camsHolder)
        //{
        //    child.gameObject.SetActive(false);
        //    child.GetChild(0).gameObject.SetActive(false);
        //    for (int i = 0; i < reseauToEnable.camsToActivate.Length; i++)
        //    {
        //        if (child.GetSiblingIndex() == i)
        //        {
        //            child.gameObject.SetActive(true);
        //            Camera camToSet = child.GetComponent<Camera>();
        //            camToSet.rect = rectsOnTablette[i].rect;
        //            rectsOnTablette[i].curCamID = i;

        //            break;
        //        }

        //        //triggers[i].OnSet(true);
        //    }
        //}
    }

    //private void SetCurrentRectIndex()
    //{
    //    curRectIndex++;
    //    if (curRectIndex == rectsOnTablette.Length)
    //        curRectIndex = 0;
    //}


}
