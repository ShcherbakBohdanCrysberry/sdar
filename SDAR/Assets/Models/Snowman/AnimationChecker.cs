using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChecker : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(transform.GetChild(0).transform.gameObject.GetComponent<MeshRenderer>().enabled == false)
        {
            if (transform.gameObject.GetComponent<SnowManAnimatorBehaviour>())
            {
                Destroy(this.transform.gameObject.GetComponent<SnowManAnimatorBehaviour>());
            }
        }
        else
        {
            if (transform.gameObject.GetComponent<SnowManAnimatorBehaviour>() == null)
            {
                transform.gameObject.AddComponent<SnowManAnimatorBehaviour>();
            }
        }
    }
}
