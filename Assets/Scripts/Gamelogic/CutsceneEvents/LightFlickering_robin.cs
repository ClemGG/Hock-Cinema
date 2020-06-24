
using System.Collections;

using UnityEngine;

public class LightFlickering_robin : MonoBehaviour
{
    Light FlickerLight;
    [SerializeField] float MinIntensity = 0f;
    [SerializeField] float MaxIntensity = 20f;

    [SerializeField] float MinWaitTime = 0f;
    [SerializeField] float MaxWaitTime = 20f;

    // Start is called before the first frame update
    void Start()
    {
        FlickerLight = GetComponent<Light>();
        StartCoroutine(Flashing());

    }
    IEnumerator Flashing()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(MinWaitTime, MaxWaitTime));
            FlickerLight.intensity = Random.Range(MinIntensity, MaxIntensity);
        }
    }

}
