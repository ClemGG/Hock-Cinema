using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickering : MonoBehaviour
{
    [SerializeField] float flickeringSpeed = 20f;
    [SerializeField] float flickeringIntensity = .7f;
    [SerializeField] int flickeringIterations = 5;

    Light light;
    float startIntensity;



    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        startIntensity = light.intensity;
    }

    //Appelée dans les cutscenes pour faire clignoter les lumières
    public void Flicker()
    {
        StartCoroutine(FickerCo());
    }

    private IEnumerator FickerCo()
    {
        for (int i = 0; i < flickeringIterations; i++)
        {
            float t = 0f;

            while(t < 1f)
            {
                t += Time.fixedDeltaTime * flickeringSpeed;
                light.intensity = Mathf.Lerp(startIntensity * flickeringIntensity, startIntensity, t);

                yield return null;

            }

            while (t > 0f)
            {
                t -= Time.fixedDeltaTime * flickeringSpeed;
                light.intensity = Mathf.Lerp(startIntensity * flickeringIntensity, startIntensity, t);

                yield return null;

            }

            yield return null;
        }
    }
}
