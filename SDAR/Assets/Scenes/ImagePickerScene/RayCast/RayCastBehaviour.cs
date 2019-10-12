using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastBehaviour : MonoBehaviour
{
    private GalleryBehaviour galleryBehaviour;
    public GameObject[] gos;
    private void Start()
    {
        this.galleryBehaviour = this.gameObject.GetComponent<GalleryBehaviour>();
    }

    private void Update()
    {
        //for (var i = 0; i < Input.touchCount; ++i)
        //{
        //    if (Input.GetTouch(i).phase == TouchPhase.Began)
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
        //        RaycastHit hit;
        //        if (Physics.Raycast(ray, out hit))
        //        {
        //            if(hit.collider.name == "LeftCube" || hit.collider.name == "RightCube") {
        //                Debug.Log("Hit on : " + hit.collider.name);

        //                galleryBehaviour.PickImage(gos);
        //            }
        //        }
        //    }
        //}
    }

    public void PickImages()
    {
        galleryBehaviour.PickImage(gos);
    }
}
