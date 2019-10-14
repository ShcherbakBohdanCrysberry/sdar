using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARImageLoaderBehaviour : MonoBehaviour
{
    public GameObject LCube;
    public GameObject RCube;
    public GameObject CCube;

    public Texture2D DefaultTexture;

    void Start()
    {
        this.SetImage(this.LCube, GameManager.Instance.LeftCubeTexture);
        this.SetImage(this.RCube, GameManager.Instance.RightCubeTexture);
        this.SetImage(this.CCube, GameManager.Instance.CenterCubeTexture);

    }

    private void SetImage(GameObject go, Texture2D texture)
    {

        Material material = go.GetComponent<Renderer>().material;
        if (!material.shader.isSupported) // happens when Standard shader is not included in the build
            material.shader = Shader.Find("Legacy Shaders/Diffuse");

        material.mainTexture = texture != null ? texture : this.DefaultTexture;
    }
    
}
