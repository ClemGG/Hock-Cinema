using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class CutsceneEvents : MonoBehaviour
{
    #region Variables

    [Space(10)]
    [Header("Cutscene noir et blanc :")]
    [Space(10)]


    [SerializeField] Volume colorPPV;
    [SerializeField] Volume greyPPV;
    [SerializeField] Volume enemyPPV;


    [Space(10)]
    [Header("Cutscene finale :")]
    [Space(10)]

    public float playerCutsceneSpd = 3f;
    [SerializeField] GameObject playerNormal, playerCutscene, ennemies;
    bool shouldMovePlayer;  //Une fois la cutscene lancée, on indique à l'Update de faire avancer le joueur

    public static CutsceneEvents instance;

    #endregion


    #region Mono
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }

        instance = this;
    }



    #endregion

    #region Cutscenes

    //Appelée par le trigger du local technique pour lancer le changement de Post process
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





    //Appelée par le trigger du couloir pour lancer la cinématique de fin
    public void ShowFinalCutscene()
    {
        StartCoroutine(ShowFinalCutsceneCo());
        
    }

    private IEnumerator ShowFinalCutsceneCo()
    {
        //On attend que le scenefader ait fait sa transition pour cacher le joueur de base et les ennemis, et afficher le joueur de la cinématique.
        //Ensuite, on fait juste diriger le joueur vide vers l'escalier jusqu'à ce qu'il touche le trigger lançant la transition vers la scène suivante.

        yield return StartCoroutine(SceneFader.instance.FadeOutCo());

        playerNormal.SetActive(false);
        ennemies.SetActive(false);
        playerCutscene.SetActive(true);

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(SceneFader.instance.FadeInCo());
    }

    #endregion
}
