using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LangageBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Space(10)]
    [Header("Scripts & Components : ")]
    [Space(10)]

    [SerializeField] LangageBtn[] otherBtns;
    Image image, select;

    [Space(10)]
    [Header("Langue : ")]
    [Space(10)]

    [SerializeField] string langue;
    [SerializeField] Sprite enabledImg, disabledImg;


    


    // Start is called before the first frame update
    void Start()
    {
        select = transform.GetChild(0).GetComponent<Image>();
        image = transform.GetChild(1).GetComponent<Image>();
        ChangerSprite(PlayerPrefs.GetString("langue"));

        select.enabled = false;
    }


    //Appelé depuis le bouton
    public void ChangerLangue()
    {
        string curLangue = PlayerPrefs.GetString("langue");

        //Si on a choisi une langue différente, on change les sprites des boutons et on appelle le LocalizationManager
        if (curLangue != langue)
        {
            PlayerPrefs.SetString("langue", langue);
            ChangerSprite(langue);

            LocalizationManager.instance.LoadLocalizedText(langue);

            for (int i = 0; i < otherBtns.Length; i++)
            {
                otherBtns[i].ChangerSprite(langue);
            }
        }
        
    }

    public void ChangerSprite(string curLangue)
    {
        image.sprite = curLangue == langue ? enabledImg : disabledImg;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        select.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        select.enabled = true;
    }
}
