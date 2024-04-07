using UnityEngine;
using System;
using System.IO;
using Directions;

public static class Utilities
{
    const float epsilon = 0.0001f; // Define a small threshold value

    #region DirectionCode
    public static string negate(string pos)
    {
        if (pos.Contains("left"))
        {
            return "right";
        }
        else if (pos.Contains("right"))
        {
            return "left";
        }
        else if (pos.Contains("above"))
        {
            return "below";
        }
        else
        {
            return "above";
        }
    }

    public static Dir getDir(string pos)
    {
        if (pos.Contains("left"))
        {
            return Dir.Right;
        }
        else if (pos.Contains("right"))
        {
            return Dir.Left;
        }
        else if (pos.Contains("above"))
        {
            return Dir.Back;
        }
        else
        {
            return Dir.Forward;
        }
    }

    public static Dir negDir(Dir curr)
    {
        if (curr == Dir.Left)
        {
            return Dir.Right;
        }
        else if (curr == Dir.Right)
        {
            return Dir.Left;
        }
        else if (curr == Dir.Forward)
        {
            return Dir.Back;
        }
        else
        {
            return Dir.Forward;
        }
    }
    #endregion

    #region Camera Driver
    public static void CaptureAndSave(Camera cameraToCapture, string imagePath, int heightReso = 0, int widthReso = 0, float fov = 0, float newXPosition = 0)
    {
        // Create a RenderTexture with the same dimensions as the screen
        RenderTexture renderTexture;
        if (heightReso == 0)
        {
            renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        }
        else if (widthReso == 0)
        {
            renderTexture = new RenderTexture(heightReso, heightReso, 24);
            
        }
        else
        {
            renderTexture = new RenderTexture(widthReso, heightReso, 24);
        }
        
        cameraToCapture.targetTexture = renderTexture;
        
        if (Math.Abs(fov) >= epsilon)
        {
            cameraToCapture.fieldOfView = fov;
        }

        // Change the camera's x position
        if (Math.Abs(newXPosition) >= epsilon)
        {
            Vector3 currentPosition = cameraToCapture.transform.position;
            cameraToCapture.transform.position = new Vector3(newXPosition, currentPosition.y, currentPosition.z);
        }

        // Force camera to render
        cameraToCapture.Render();

        // Create a new Texture2D with the camera's dimensions
        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        
        // Set the RenderTexture as the active RenderTexture
        RenderTexture.active = renderTexture;
        
        // Read the pixels from the RenderTexture and apply them to the Texture2D
        renderResult.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        renderResult.Apply();

        // Encode the Texture2D to a PNG
        byte[] byteArray = renderResult.EncodeToPNG();

        // Save the PNG to disk
        string fullPath = Path.Combine(Application.dataPath, imagePath + ".png");
        File.WriteAllBytes(fullPath, byteArray);

        Debug.Log("Saved Image to " + fullPath);

        // Clean up
        RenderTexture.active = null;
        cameraToCapture.targetTexture = null;
        renderTexture.Release();

        // If you have an existing texture and don't want to keep it in memory, uncomment the following line:
    }


    public static string CaptureCamera(Camera cameraToCapture, int heightReso = 0, int widthReso = 0, float fov = 0, float newXPosition = 0)
    {
        // Create a RenderTexture with the same dimensions as the screen
        RenderTexture renderTexture;
        if (heightReso == 0)
        {
            renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        }
        else if (widthReso == 0)
        {
            renderTexture = new RenderTexture(heightReso, heightReso, 24);
            
        }
        else
        {
            renderTexture = new RenderTexture(widthReso, heightReso, 24);
        }
        
        cameraToCapture.targetTexture = renderTexture;
        
        if (Math.Abs(fov) >= epsilon)
        {
            cameraToCapture.fieldOfView = fov;
        }

        // Change the camera's x position
        if (Math.Abs(newXPosition) >= epsilon)
        {
            Vector3 currentPosition = cameraToCapture.transform.position;
            cameraToCapture.transform.position = new Vector3(newXPosition, currentPosition.y, currentPosition.z);
        }
        
        // Force camera to render
        cameraToCapture.Render();

        // Create a new Texture2D with the camera's dimensions
        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);

        // Set the RenderTexture as the active RenderTexture
        RenderTexture.active = renderTexture;

        // Read the pixels from the RenderTexture and apply them to the Texture2D
        renderResult.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        renderResult.Apply();

        // Encode the Texture2D to a PNG
        byte[] byteArray = renderResult.EncodeToPNG();

        // Clean up
        RenderTexture.active = null;
        cameraToCapture.targetTexture = null;
        renderTexture.Release();

        string base64String = Convert.ToBase64String(byteArray);

        return base64String;
    }
    #endregion
}