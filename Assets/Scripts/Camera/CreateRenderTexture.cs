using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRenderTexture : MonoBehaviour
{
    private static float screenWidth;
    private static float screenHeight;


    private void Start()
    {
#if UNITY_EDITOR
        screenHeight = 1080;
        screenWidth = 1920;
#endif
        
        
#if !UNITY_EDITOR
        screenHeight = Screen.height;
                screenWidth = Screen.width;
#endif
        
        
    }


    public static Texture2D RTImage()
    {
        var rect = new Rect(0, 0, screenWidth, screenHeight);

        var renderTexture = new RenderTexture((int)screenWidth, (int)screenHeight, 24);
        Debug.Log(renderTexture.width + "" + renderTexture.height);
        
        
        var screenShot = new Texture2D((int) screenWidth, (int) screenHeight, TextureFormat.RGBA32, false);

        Camera.main.targetTexture = renderTexture;
        Camera.main.Render();

        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        Camera.main.targetTexture = null;
        RenderTexture.active = null;
       
        
        Destroy(renderTexture);
        renderTexture = null;

        return screenShot;
    }
}