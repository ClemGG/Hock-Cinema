using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class BakeOnStart : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

}
