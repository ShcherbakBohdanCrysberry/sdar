using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager instance;

    public Texture2D LeftCubeTexture;
    public Texture2D RightCubeTexture;
    public Texture2D CenterCubeTexture;

    private static object syncRoot = new Object();

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new GameManager();
                }
            }
            return instance;
        }
    }

}
