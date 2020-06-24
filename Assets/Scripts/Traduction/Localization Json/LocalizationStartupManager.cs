using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clement.Utilities.Strings;

[RequireComponent(typeof(LocalizationManager))]
public class LocalizationStartupManager : MonoBehaviour {


    public string startLanguage = "fr";

    public static LocalizationStartupManager instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        //On assigne d'abord les PlayerPrefs pour que les scripts s'initialisant automatiquement puissent
        //récupérer la langue
        if (!PlayerPrefs.HasKey("langue"))
        {
            PlayerPrefs.SetString("langue", startLanguage);
        }
    }


    
    public void Start () 
    {
        /* Au lieu de mettre directement startLanguage, on assigne les PlayerPrefs dans l'Awake et on
         * passe le résultat.
         * Comme ça, si on a déjà lancé une partie, on pourra récupérer la dernière langue au lieu
         * d'avoir tjs la même.
         * Si on ne l'a jamais lancé, on aura tjs startLanguage de tte façon
         */

        string curLangue = PlayerPrefs.GetString("langue");
        if (!Strings.IsNullOrEmptyOrWhiteSpace(curLangue))
        {
            LocalizationManager.instance.LoadLocalizedText(curLangue);
        }
    }


    private void OnLevelWasLoaded(int level)
    {
        Start();
    }
}
