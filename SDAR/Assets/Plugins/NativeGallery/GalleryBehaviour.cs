using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
using UnityEngine.Android;
#endif


public class GalleryBehaviour : MonoBehaviour
{
    private void Start()
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageRead);
            }
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            }
#endif
    }

    public IEnumerator TakeScreenshotAndSave()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        // Save the screenshot to Gallery/Photos
        Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(ss, "GalleryTest", "Image.png"));

        // To avoid memory leaks
        Destroy(ss);
    }

    public void PickImage(GameObject [] gosToShowImage)
    {
        NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((paths) =>
        {
            if (paths != null)
            {
                if (paths.Length > 0)
                {
                    for (int i = 0; i <= paths.Length - 1; i++)
                    {
                        if (gosToShowImage.Length - 1 < i) continue;
                        // Create Texture from selected image
                        Texture2D texture = NativeGallery.LoadImageAtPath(paths[i], 512);
                        if (texture == null)
                        {
                            Debug.Log("Couldn't load texture from " + paths[i]);
                            return;
                        }
                        //quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);
                      
                        Material material = gosToShowImage[i].GetComponent<Renderer>().material;
                        if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                            material.shader = Shader.Find("Legacy Shaders/Diffuse");

                        material.mainTexture = texture;
                    }
                }
            }
        }, "Select a PNG image", "image/png");

        Debug.Log("Permission result: " + permission);
    }
}