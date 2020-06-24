using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

//Cette classe est appelée automatiquement par les UIElements quand la souris exécute une action.
//Pour chaque type d'action, on crée un UnityEvent qui sera rempli dans l'inspector pour plus de flexibilité.
//On crée aussi un event pour la fonction Start() si on a besoin d'intialiser des paramètres

public class PointerClickEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{
    public UnityEvent onStartEvent,
                      onPointerEnterEvent,
                      onPointerExitEvent,
                      onPointerUpEvent,
                      onPointerDownEvent,
                      onPointerClickEvent;


    private void Start()
    {
        onStartEvent?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnterEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExitEvent?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUpEvent?.Invoke();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDownEvent?.Invoke();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        onPointerClickEvent?.Invoke();
    }
}
