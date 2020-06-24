using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Clement.Persistence.Localization;
using Clement.Utilities.Strings;
//using UnityEditor;

public class LocalizedComponent : MonoBehaviour {

    public string key;
    // N'utiliser le paramètre que si l'on veut changer les données sans changer la clé
    public void ChangeComponentData () 
    {
        if (Strings.IsNullOrEmptyOrWhiteSpace(key))
            return;

        LocalizedItem item = LocalizationManager.instance.GetLocalizedData(key);


        switch (item.type)
        {
            case LocalizedItemType.Null:

                Debug.Log(item.ConvertAs<string>());
                goto case LocalizedItemType.String;

            case LocalizedItemType.String:

                Text text = GetComponent<Text>();
                TextMeshProUGUI textMesh = GetComponent<TextMeshProUGUI>();
                string str = item.ConvertAs<string>();

                if (text) text.text = str;
                else if (textMesh) textMesh.text = str;

                break;


            case LocalizedItemType.Sprite:

                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                Image i = GetComponent<Image>();
                Sprite sp = item.ConvertAs<Sprite>();

                if (sr) sr.sprite = sp;
                else if (i) i.sprite = sp;

                break;

            case LocalizedItemType.Audio:

                AudioSource a = GetComponent<AudioSource>();
                AudioClip aud = item.ConvertAs<AudioClip>();

                if (a) a.clip = aud;

                break;


            /* Pour les assets qui ont été enregistrées dans le fichier json, on n'enregistre que le chemin d'accès vers l'asset dans le dossier Resources.
             * Comme ce dossier est créé à part dans le build, on pourra y accéder avec le chemin et récupérer l'asset avec Resources.Load() au lieu de
             * renvoyer l'asset directement (le fichier renvoir l'instanceID de l'asset, et vu qu'elle change quand on en crée une nouvelle, on ne peut pas sérialiser
             * une asset directement).
             */



            //Ne fonctionne que dans l'éditeur

            //case LocalizedItemType.MonoBehaviour:

            //    MonoScript m = item.ConvertAs<MonoScript>();
            //    Type T = m.GetType();
            //    //print(T);
            //    Parent t = (Parent)gameObject.AddComponent(T); // Utiliser une classe parent dont dérive les classes qui doivent changer selon la traduction

            //    break;
        }
    }




    // N'utiliser le paramètre que si l'on veut récupérer les données sans changer la clé
    public T GetComponentData<T>()
    {
        if (Strings.IsNullOrEmptyOrWhiteSpace(key))
            return default;

        LocalizedItem item = LocalizationManager.instance.GetLocalizedData(key);
        return item.ConvertAs<T>();

    }

    // N'utiliser le paramètre que si l'on veut récupérer les données sans changer la clé
    public T GetComponentDataFromResources<T>() where T : class
    {
        if (Strings.IsNullOrEmptyOrWhiteSpace(key))
            return default;

        LocalizedItem item = LocalizationManager.instance.GetLocalizedData(key);
        return Resources.Load(item.ConvertAs<string>()) as T;
        //return item.ConvertAs<T>();

    }




    // N'utiliser le paramètre que si l'on veut changer les données sans changer la clé
    public T AssignNewComponentData<T>(string newString, bool changeKey, bool changeData)
    {

        if (Strings.IsNullOrEmptyOrWhiteSpace(newString))
            return default;


        string originalKey = key;
        key = newString;

        if (changeData)
        {
            ChangeComponentData();
        }

        T returnedData = GetComponentData<T>();


        if (!changeKey)
            key = originalKey;

        return returnedData;
    }


}
