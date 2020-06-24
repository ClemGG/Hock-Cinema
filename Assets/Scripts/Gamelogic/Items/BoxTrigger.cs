using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class BoxTrigger : MonoBehaviour
{
    [SerializeField] string entityTag = "Entity/Player";
    [SerializeField] bool destroyOnEnter, destroyOnExit;
    [SerializeField] UnityEvent onTriggerEnterEvent, onTriggerStayEvent, onTriggerExitEvent;

    BoxCollider bc;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider>();
        bc.isTrigger = true;
    }


    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag(entityTag))
        {
            onTriggerEnterEvent?.Invoke();
        }

        if (destroyOnEnter) Destroy(gameObject);
    }
    private void OnTriggerStay(Collider c)
    {
        if (c.CompareTag(entityTag))
        {
            onTriggerStayEvent?.Invoke();
        }
    }
    private void OnTriggerExit(Collider c)
    {
        if (c.CompareTag(entityTag))
        {
            onTriggerExitEvent?.Invoke();
        }

        if (destroyOnExit) Destroy(gameObject);
    }

}
