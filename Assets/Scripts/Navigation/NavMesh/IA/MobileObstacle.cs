using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileObstacle : MonoBehaviour
{
    Transform t;
    bool stopMovement = false;

    [SerializeField] bool X, Y, Z;
    [SerializeField] float dst = 5f;

    private Vector3 _startPosition;

    void Start()
    {
        t = transform;
        _startPosition = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            stopMovement = !stopMovement;

        if (!stopMovement)
        {
            transform.position = _startPosition + new Vector3(X ? Mathf.Sin(Time.time) * dst : 0f, 
                                                                Y ? Mathf.Sin(Time.time) * dst : 0f, 
                                                                Z ? Mathf.Sin(Time.time) * dst : 0f);

        }
    }
}
