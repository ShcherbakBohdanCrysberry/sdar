using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneRedirecter : MonoBehaviour
{
    public void ToImagePickerScene()
    {
        SceneManager.LoadScene(1);
    }

    public void ToArScene()
    {
        SceneManager.LoadScene(0);
    }
}
