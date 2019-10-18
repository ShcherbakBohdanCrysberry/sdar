using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastBehaviour : MonoBehaviour
{
    //private GalleryBehaviour galleryBehaviour;
    public GameObject LeftCube;
    public GameObject RightCube;
    public GameObject CenterCube;

    public void PickImageLeft()
    {
        var galleryBehaviour = new GalleryBehaviour();
        galleryBehaviour.PickImage(LeftCube);
        
    }

    public void PickImageRight()
    {
        var galleryBehaviour = new GalleryBehaviour();
        galleryBehaviour.PickImage(RightCube);

    }

    public void PickImageCenter()
    {
        var galleryBehaviour = new GalleryBehaviour();
        galleryBehaviour.PickImage(CenterCube);

    }
}
