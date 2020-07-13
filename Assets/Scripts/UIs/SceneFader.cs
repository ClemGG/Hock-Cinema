using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] Image fadeImg;


    public static SceneFader instance;



    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        if(!fadeImg)
            fadeImg = transform.GetChild(0).GetComponent<Image>();

        if (fadeImg)
            fadeImg.gameObject.SetActive(true);

        StartCoroutine(FadeInCo());
    }









    public void FadeToScene(int index)
    {
        StartCoroutine(FadeOutToSceneCo(index));

    }

    public void FadeToQuit()
    {
        StartCoroutine(FadeToQuitCo());
    }

    public IEnumerator FadeInCo()
    {
        fadeImg.gameObject.SetActive(true);
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, t);
            yield return null;
        }
        fadeImg.gameObject.SetActive(false);

    }


    public IEnumerator FadeOutCo()
    {
        fadeImg.gameObject.SetActive(true);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;
            fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, t);
            yield return null;
        }


    }
    public IEnumerator FadeOutToSceneCo(int index)
    {
        fadeImg.gameObject.SetActive(true);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;
            fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, t);
            yield return null;
        }

        SceneManager.LoadScene(index);

    }
    private IEnumerator FadeToQuitCo()
    {
        fadeImg.gameObject.SetActive(true);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;
            fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, t);
            yield return null;
        }

        Application.Quit();
    }

    public static int GetCurSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    public static string GetCurSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

}
