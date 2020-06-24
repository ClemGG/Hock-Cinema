using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CutsceneEvents : MonoBehaviour
{
    [SerializeField] Volume colorPPV, greyPPV, enemyPPV;

    public static CutsceneEvents instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }

        instance = this;
    }


    public void ShowGreyPPV(float changeSpeed)
    {
        StartCoroutine(ChangePPV_Co(true, changeSpeed));
    }
    public void HideGreyPPV(float changeSpeed)
    {
        StartCoroutine(ChangePPV_Co(false, changeSpeed));
    }

    private IEnumerator ChangePPV_Co(bool toGreyscale, float changeSpeed)
    {
        float t = 0f;

        if (toGreyscale)
        {
            greyPPV.gameObject.SetActive(true);
            enemyPPV.gameObject.SetActive(true);
        }


        while(t < 1f)
        {
            t += Time.deltaTime * changeSpeed;
            greyPPV.weight = t;

            yield return null;
        }

        if (!toGreyscale)
        {
            greyPPV.gameObject.SetActive(false);
            enemyPPV.gameObject.SetActive(false);
        }
    }
}
