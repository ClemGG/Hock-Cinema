using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(LocalizationStartupManager))]
public class LocalizationManager : MonoBehaviour
{

    [HideInInspector] public LocalizedComponent[] componentsToModify;
    public Dictionary<string, LocalizedItem> localizedDictionary;

    public string fileGenericName = "traduction_", fileExtension = "..json", missingtextString = "Donnée localisée non trouvée pour le tag \"{0} \"!";
    public string currentLanguage;




    public static LocalizationManager instance;
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }





    public void LoadLocalizedText(string fileLanguage)
    {
        currentLanguage = fileLanguage;
        //print(LocalizationManager.instance.currentLanguage);

        string fileName = string.Format("{0}{1}{2}", fileGenericName, fileLanguage, fileExtension);

        localizedDictionary = new Dictionary<string, LocalizedItem>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName).Replace('\\', '/');


        if (!File.Exists(filePath))
        {
            Debug.Log("Erreur : Le fichier \"" + fileName + "\" est introuvable dans le dossier \"" + filePath + "\".");
        }
        else
        {
            string dataAsJson = File.ReadAllText(filePath);
            LocalizedAssetArray loadedData = JsonUtility.FromJson<LocalizedAssetArray>(dataAsJson);

            for (int i = 0; i < loadedData.data.Length; i++)
            {
                localizedDictionary.Add(loadedData.data[i].key, loadedData.data[i].item);
            }

            //Debug.Log("Données chargées, le dictionnaire contient " + localizedDictionary.Count + " entrées.");
            Translate();
        }

    }



    public LocalizedItem GetLocalizedData(string key)
    {
        string resultString = string.Format(missingtextString, key);
        LocalizedItem resultItem = new LocalizedItem(LocalizedItemType.Null, resultString);

        if (key == null)
        {
            return resultItem;
        } 

        if (localizedDictionary.ContainsKey(key))
        {
            resultItem = localizedDictionary[key];
        }

        return resultItem;
    }



    private void Translate()
    {
        componentsToModify = (LocalizedComponent[])Resources.FindObjectsOfTypeAll(typeof(LocalizedComponent));
        ChangeAllComponentsInScene();

    }

    private void ChangeAllComponentsInScene()
    {
        for (int i = 0; i < componentsToModify.Length; i++)
        {
            componentsToModify[i].ChangeComponentData();
        }
    }
}
