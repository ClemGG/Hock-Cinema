using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAnimPerso : MonoBehaviour
{
    public Animator a;
    public string[] animNames;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < animNames.Length; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                a.Play(animNames[i]);
            }
        }
    }
}
