using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TVEffect : MonoBehaviour
{
    [SerializeField] private Material Mat;
    Material _mat;
    private void Start()
    {
        _mat = new Material(Mat);
    }


    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _mat);
    }


    public Material _Mat
    {
        get { return Mat; }
        set { Mat = value; }
    }
}
