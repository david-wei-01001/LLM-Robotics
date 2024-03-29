using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using Gemini;
using System;
using System.IO;


public class BarMover : MonoBehaviour
{
    #region Setup
    // Unity API
    public Camera MainCamera;
    public Camera LRCamera;
    public Camera LRCamera1;
    public BarController bar; // Assign single bar script here in the inspector

    // Lock and Synchronization
    private bool lproj;
    // Gemini API
    private GeminiTextRequest geminiTextRequest;

    // Globals
    private string llmOutput;
    private Dir onePos;
    private Dir twoPos;
    private string action1Prompt;
    private string action2Prompt;
   
    float epsilon = 0.0001f; // Define a small threshold value


    void Start()
    {
        lproj = true;
        llmOutput = "";
        onePos = Dir.Up;
        twoPos = Dir.Up;
        action1Prompt = Utilities.actionHeader;
        action2Prompt = Utilities.actionHeader;
        geminiTextRequest = new GeminiTextRequest();
    }

    public enum Dir
    {
        Left,
        Right,
        Up,
        Down,
        Forward,
        Back
    }
    #endregion

    public void Run()
    {
        // StartCoroutine(comunicationWrapper(LRCamera, Utilities.LRBarAsk, 128));
        // StartCoroutine(CubeCubeTestWrapper(188, 42, 0.03f));
        // CameraWrapper();
        StartCoroutine(ExecutionWrapper());
    }

    #region Wrappers
    
    private IEnumerator ExecutionWrapper()
    {
        // AppLeft start at 0.076, 0.022 
        // AppRight start at 0.296, 0.022
        
        // CaptureAndSave(MainCamera, "Init");
        Debug.Log("Program begins.");
        Debug.Log("Determining directions.");
        string xPos, zPos, cubePos; 
        yield return StartCoroutine(DirVote(LRCamera, Utilities.LRBarAsk, 128));
        xPos = llmOutput.ToLower(); // left
        yield return StartCoroutine(DirVote(LRCamera, Utilities.FBBarAsk, 128));
        zPos = llmOutput.ToLower(); // below
        Debug.Log($"The appreratus is located {xPos} {zPos} the red cube.");
        yield return StartCoroutine(DirVote(LRCamera1, Utilities.LRPrompt, 128, 188, 34));
        cubePos = llmOutput.ToLower(); // right
        Debug.Log($"The red cube is located {cubePos} of the green cube.");

        Debug.Log("Moving the appreratus towards the red cube.");
        bool needStepOne = directionDetermination(xPos, zPos, cubePos);
        if (needStepOne)
        {
            yield return StartCoroutine(StepLogic(1, LRCamera, onePos, action1Prompt, 5, 128));
        }
        // CaptureAndSave(MainCamera, "start2");
        yield return StartCoroutine(StepLogic(2, LRCamera, twoPos, action2Prompt, 6, 128));
        Debug.Log("Moving the red cube towards the green cube.");
        // CaptureAndSave(MainCamera, "start3");
        yield return StartCoroutine(StepLogic(3, LRCamera1, getDir(cubePos), Utilities.PosCheck, 0, 128, 188, 34));
        Debug.Log("Done.");
        // CaptureAndSave(MainCamera, "done");
    }

    private IEnumerator comunicationWrapper(String discriminator, Camera camView, string query, int heightReso = 0, int widthReso = 0, float fov = 0, float newXPosition = 0)
    {
        string Data = CaptureCamera(camView, heightReso, widthReso, fov,newXPosition);
        
        yield return StartCoroutine(CommunicationWithGemini(query, Data));
        Debug.Log(discriminator + $"Response is {llmOutput}");
    }

    private IEnumerator CubeCubeTest1Wrapper()
    {
        // Apperatus in test start at 0.22, 0.22
        int[] fovs = new int[] { 34, 38, 42, 46 };
        int[] widths = new int[] { 128, 188, 256 };

        foreach (int fov in fovs)
        {
            Debug.Log($"fov is {fov}");
            foreach (int width in widths)
            {
                Debug.Log($"width is {width}");
                for (int i = 0; i < 5; i++)
                {
                    // Assuming LRCamera1 is a Camera object and Utilities.LRPrompt is a string, replace these with actual references in your code
                    yield return StartCoroutine(comunicationWrapper($"{fov}, {width}", LRCamera1, Utilities.LRPrompt, 128, width, fov));
                }
            }
        }
    }

    private IEnumerator CubeCubeTestWrapper(int width, int fov, float xVal = 0)
    {
        yield return StartCoroutine(StepLogic(3, LRCamera1, Dir.Left, Utilities.PosCheck, 0, 128, width, fov, xVal));
    }

    private IEnumerator CubeCubeTest3Wrapper()
    {
        int[] fovs = new int[] { 42, 46 };
        int[] widths = new int[] { 188, 256 };
        float[] xs = new float[] { -0.05f, -0.03f, 0.03f, 0.05f };

        foreach (int fov in fovs)
        {
            Debug.Log($"fov is {fov}");
            foreach (int width in widths)
            {
                Debug.Log($"width is {width}");
                foreach (float val in xs)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        // Assuming LRCamera1 is a Camera object and Utilities.LRPrompt is a string, replace these with actual references in your code
                        yield return StartCoroutine(comunicationWrapper($"{fov}, {width}, {val}", LRCamera1, Utilities.LRPrompt, 128, width, fov, val));
                    }
                }
                
            }
        }
    }

    private void CameraWrapper()
    {
        CaptureAndSave(LRCamera1, "p1-1", 128, 188, 42, -0.05f);
        CaptureAndSave(LRCamera1, "p1-2", 128, 188, 42, -0.03f);
        CaptureAndSave(LRCamera1, "p1-3", 128, 188, 42, 0.03f);
        CaptureAndSave(LRCamera1, "p1-4", 128, 188, 42, 0.05f);
        CaptureAndSave(LRCamera1, "p2-1", 128, 256, 42, -0.05f);
        CaptureAndSave(LRCamera1, "p2-2", 128, 256, 42, -0.03f);
        CaptureAndSave(LRCamera1, "p2-3", 128, 256, 42, 0.03f);
        CaptureAndSave(LRCamera1, "p2-4", 128, 256, 42, 0.05f);
        
        CaptureAndSave(LRCamera1, "p3-1", 128, 188, 46, -0.05f);
        CaptureAndSave(LRCamera1, "p3-2", 128, 188, 46, -0.03f);
        CaptureAndSave(LRCamera1, "p3-3", 128, 188, 46, 0.03f);
        CaptureAndSave(LRCamera1, "p3-4", 128, 188, 46, 0.05f);
        CaptureAndSave(LRCamera1, "p4-1", 128, 256, 46, -0.05f);
        CaptureAndSave(LRCamera1, "p4-2", 128, 256, 46, -0.03f);
        CaptureAndSave(LRCamera1, "p4-3", 128, 256, 46, 0.03f);
        CaptureAndSave(LRCamera1, "p4-4", 128, 256, 46, 0.05f);
    }
    
    #endregion

    #region Core Functions
    IEnumerator StepLogic(int step, Camera CamView, Dir currDir, string terminationQuery, int adjustment, int heightReso = 0, int widthReso = 0, float fov = 0, float newXPosition = 0)
    {
        // Bar moving
        string Data;
        int moveCount = 0;
        do
        {
            bar.Move(currDir);
            Data = CaptureCamera(CamView, heightReso, widthReso, fov, newXPosition);

            yield return StartCoroutine(CommunicationWithGemini(terminationQuery, Data));
            llmOutput = llmOutput.ToLower();
            if (llmOutput.Contains("yes") && moveCount < 2)
            {
                llmOutput = "no";
                Debug.LogError($"Step {step} sanity check captured error output: {llmOutput}");
            }
            else if (llmOutput.Contains("no") && moveCount > 36)
            {
                Debug.LogError($"Step {step} sanity check captured too much iteration");
                break;
            }
            else
            {
                Debug.Log($"Step {step} response: {llmOutput}");
            }
            moveCount++;
            
            // The following code will print time series of execution 

            // if (moveCount % 3 == 0)
            // {
            //     int quotient = moveCount / 3;
            //     CaptureAndSave(MainCamera, $"s{step}-{quotient}");
            // }
            
        } while (llmOutput.Contains("no"));

        // Manual adjust
        if (adjustment != 0)
        {
            bar.Move(currDir, adjustment);
        }
    }

    IEnumerator CommunicationWithGemini(params string[] strings)
    {
        yield return new WaitUntil(() => lproj);
        lproj = false;

        // Start the async operation in a way that doesn't block the Unity main thread
        // Directly using GenerateContent within Task.Run
        Task<GeminiTextResponse> llmTask = Task.Run(async () => await geminiTextRequest.SendMsg(strings));

        // Wait until the async task has completed
        while (!llmTask.IsCompleted)
        {
            yield return null;
        }

        GeminiTextResponse response = llmTask.Result; // Access the Result property here
        string tempOut = response.candidates[0].content.parts[0].text;
        llmOutput = tempOut.ToLower();

        // Release lock
        lproj = true;
    }
    #endregion

    #region Directions
    
    private bool directionDetermination(string xPos, string zPos, string cubePos)
    {
        if (cubePos.Contains("left"))
        {
            
            if (xPos.Contains("right"))
            {
                onePos = getDir(xPos);
                action1Prompt = action1Prompt + zPos + Utilities.actionTail;
                twoPos = getDir(zPos);
                action2Prompt = action2Prompt + negate(xPos) + " of" + Utilities.actionTail;
                return true;
            }
            else
            {
                twoPos = getDir(zPos);
                action2Prompt = action2Prompt + xPos + " of" + Utilities.actionTail;
                return false;
            }
        }
        else if (cubePos.Contains("right"))
        {
            if (xPos.Contains("left"))
            {
                onePos = getDir(xPos);
                action1Prompt = action1Prompt + zPos + Utilities.actionTail;
                twoPos = getDir(zPos);
                action2Prompt = action2Prompt + negate(xPos) + " of" + Utilities.actionTail;
                return true;
            }
            else
            {
                twoPos = getDir(zPos);
                action2Prompt = action2Prompt + xPos + " of" + Utilities.actionTail;
                return false;
            }
        }
        else if (cubePos.Contains("above"))
        {
            if (zPos.Contains("below"))
            {
                onePos = getDir(zPos);
                action1Prompt = action1Prompt + xPos + " of" + Utilities.actionTail;
                twoPos = getDir(xPos);
                action2Prompt = action2Prompt + negate(zPos) + Utilities.actionTail;
                return true;
            }
            else
            {
                twoPos = getDir(xPos);
                action2Prompt = action2Prompt + zPos + Utilities.actionTail;
                return false;
            }
        }
        else 
        {
            if (zPos.Contains("above"))
            {
                onePos = getDir(zPos);
                action1Prompt = action1Prompt + xPos + " of" + Utilities.actionTail;
                twoPos = getDir(xPos);
                action2Prompt = action2Prompt + negate(zPos) + Utilities.actionTail;
                return true;
            }
            else
            {
                twoPos = getDir(xPos);
                action2Prompt = action2Prompt + zPos + Utilities.actionTail;
                return false;
            }
        }
    }

    IEnumerator DirVote(Camera CamView, string directionQuery, int heightReso = 0, int widthReso = 0, float fov = 0, float newXPosition = 0)
    {
        string Data = CaptureCamera(CamView, heightReso, widthReso, fov, newXPosition);
        int rightCount = 0, leftCount = 0, fCount = 0, bCount = 0;

        //  // Direction Voting
        for (int i = 0; i < 3; i++)
        {
            yield return StartCoroutine(CommunicationWithGemini(directionQuery, Data));
            string direction = llmOutput.ToLower();
            if (direction.Contains("right"))
            {
                rightCount++;
            }
            else if (direction.Contains("left"))
            {
                leftCount++;
            }
            else if (direction.Contains("above"))
            {
                fCount++;
            }
            else if (direction.Contains("below"))
            {
                bCount++;
            }
            else
            {
                Debug.LogError($"Direction query failed with reply {llmOutput}.");
                yield break; // This exits the coroutine
            }
        }

        //Direction Decision
        if (rightCount > 2)
        {
            llmOutput = "right";
        }
        else if (leftCount > 2)
        {
            llmOutput = "left";
        }
        else if (fCount > 2)
        {
            llmOutput = "above";
        }
        else if (bCount > 2)
        {
            llmOutput = "below";
        }
        else
        {
            Debug.LogError($"Direction voting failed with counts right: {rightCount}, left: {leftCount}, front: {fCount}, back: {bCount}.");
            yield break; // This exits the coroutine
        }
    }

    string negate(string pos)
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

    Dir getDir(string pos)
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
    #endregion

    #region Camera Driver
    public void CaptureAndSave(Camera cameraToCapture, string imagePath, int heightReso = 0, int widthReso = 0, float fov = 0, float newXPosition = 0)
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
        Destroy(renderTexture);

        // If you have an existing texture and don't want to keep it in memory, uncomment the following line:
        Destroy(renderResult);
    }


    public string CaptureCamera(Camera cameraToCapture, int heightReso = 0, int widthReso = 0, float fov = 0, float newXPosition = 0)
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
        Destroy(renderTexture);
        Destroy(renderResult);

        string base64String = Convert.ToBase64String(byteArray);

        return base64String;
    }
    #endregion
}