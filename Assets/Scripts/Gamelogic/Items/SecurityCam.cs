using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SecurityCam : MonoBehaviour
{
    [SerializeField] Transform feedBackCanvas;
    [SerializeField] Light camLight;
    [SerializeField] Material inactiveMat, activeMat;
    [HideInInspector] public bool isActive;

    [Space(10)]

    [SerializeField] TextMeshProUGUI camNameText, camIDText, reseauIDText;
    [SerializeField] string camName, camID, reseauID;

    Camera cam;
    MeshRenderer rend;


    private void Awake()
    {
        cam = GetComponent<Camera>();
        rend = GetComponent<MeshRenderer>();
        cam.enabled = false;

        //Plus besoin de ça, vu qu'on appelle directement le LocalizedComponent
        //camNameText.text = camName;
        //camIDText.text = string.Format(camID, (transform.GetSiblingIndex()+1) + 4 * transform.parent.GetSiblingIndex());
        //reseauIDText.text = string.Format(reseauID, transform.parent.GetSiblingIndex() + 1);

    }

    public void EnableCam(bool active)
    {
        cam.enabled = active && PlayerController.isOnTablet;
        camLight.enabled = !PlayerController.isOnTablet;
    }

    public void SetupCam(Rect newRect)
    {
        cam.rect = newRect;
    }

}
