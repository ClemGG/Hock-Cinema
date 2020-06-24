using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSetterTrigger : InteractableTrigger
{
    [Space(10)]
    [Header("Scripts & Components :")]
    [Space(10)]


    [SerializeField] MeshRenderer rend;
    [SerializeField] Material setMat, freeMat;
    [HideInInspector] public int linkedSecurityCamID;
    [HideInInspector] public bool set = false;
    [HideInInspector] public bool linkedCamIsDestroyed = false;


    private void Start()
    {
        linkedSecurityCamID = transform.GetSiblingIndex();
    }


    //Appelée par le CamSetterManager
    public void OnSet(bool setOnTablet)
    {
        set = setOnTablet;
        rend.material = set ? setMat : freeMat;

    }

    //public void SetCamAsRepaired(bool repaired)
    //{
    //    linkedCamIsDestroyed = !repaired;
    //    //CamSetterManager.instance.UpdateObjectiveUI();
    //}

    public override bool HasBeenInteracted()
    {
        return set;
    }
}
