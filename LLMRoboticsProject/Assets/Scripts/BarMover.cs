using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using Gemini;
using System;
using Directions;
using static Utilities;
using static Queries;

public class BarMover : MonoBehaviour
{
    #region Setup
    // Unity API
    public Camera MainCamera;
    public Camera LRCamera;
    public Camera ABCamera;
    public Camera LRCamera1;
    public Camera SFCamera;
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
    private string PromptEnding;
   
    
    const float k_Wait = 0.1f;


    void Start()
    {
        lproj = true;
        llmOutput = "";
        onePos = Dir.Up;
        twoPos = Dir.Up;

        action1Prompt = actionHeader;
        action2Prompt = actionHeader;
        PromptEnding = actionTail;

        geminiTextRequest = new GeminiTextRequest();
    }

    #endregion

    #region DiskDiskCode
    public void RunDiskDisk()
    {
        StartCoroutine(ExecutionDiskDiskWrapper());
    }

    private IEnumerator ExecutionDiskDiskWrapper()
    {
        action1Prompt = DDactionHeader;
        action2Prompt = DDactionHeader;
        PromptEnding = DiskactionTail;
        // AppLeft start at 0.16, 0.6623, -0.14 
        // AppRight start at 0.3, 0.6623, -0.14
        
        // CaptureAndSave(MainCamera, "Init");
        Debug.Log("Program begins.");
        Debug.Log("Determining directions.");
        string xPos, zPos, cubePos; 
        yield return StartCoroutine(DirVote(LRCamera, LRDDAsk, 128));
        xPos = llmOutput.ToLower(); // right
        yield return StartCoroutine(DirVote(LRCamera, FBDDAsk, 128));
        zPos = llmOutput.ToLower(); // below
        Debug.Log($"The appreratus is located {xPos} {zPos} the red disk.");
        yield return StartCoroutine(DirVote(LRCamera1, DiskLRPrompt, 128, 188, 34));
        cubePos = llmOutput.ToLower(); // right

        // xPos = "right";
        // zPos = "below";
        // cubePos = "right";
        Debug.Log($"The red disk is located {cubePos} of the green cube.");
        bar.Move(Dir.Left, 3);
        Debug.Log("Moving the appreratus towards the red disk.");
        bool needStepOne = directionDetermination(xPos, zPos, cubePos);
        if (needStepOne)
        {
            yield return StartCoroutine(StepLogic(1, LRCamera, onePos, action1Prompt, 5, 128));
        }
        // CaptureAndSave(MainCamera, "start2");

        // might need to change 'horizontally' in DDactionUp to 'vertically'
        yield return StartCoroutine(StepLogic(2, ABCamera, twoPos, DDactionUp, 4, 128));
        Debug.Log("Moving the red disk towards the green cube.");
        // CaptureAndSave(MainCamera, "start3");
        yield return StartCoroutine(SmartStepLogic(3, LRCamera1, SFCamera, Dir.Left, DDPosCheck, 2, 128, 188, 34));
        Debug.Log("Done.");
        // CaptureAndSave(MainCamera, "done");
    }
    #endregion

    #region BarDiskCode
    public void RunBarDisk()
    {
        StartCoroutine(ExecutionBarDiskWrapper());
    }

    /**
    <summary>
    Control Bar like Robotics to puse a disk
    Please use Apperatus 1, and Disk
    Remember to assign Bar Controller to Apperatus
    </summary>
    */
    private IEnumerator ExecutionBarDiskWrapper()
    {
        // AppLeft start at 0.04, 0.64, 0.04 
        // AppRight start at 0.18, 0.64, 0.04
        action1Prompt = DiskactionHeader;
        action2Prompt = DiskactionHeader;
        PromptEnding = DiskactionTail;
        
        // CaptureAndSave(MainCamera, "Init");
        Debug.Log("Program begins.");
        Debug.Log("Determining directions.");
        string xPos, zPos, cubePos; 
        yield return StartCoroutine(DirVote(LRCamera, LRDiskAsk, 128));
        xPos = llmOutput.ToLower(); // left
        yield return StartCoroutine(DirVote(LRCamera, FBDiskAsk, 128));
        zPos = llmOutput.ToLower(); // below
        Debug.Log($"The appreratus is located {xPos} {zPos} the red disk.");
        yield return StartCoroutine(DirVote(LRCamera1, DiskLRPrompt, 128, 188, 34));
        cubePos = llmOutput.ToLower(); // right

        // xPos = "left";
        // zPos = "below";
        // cubePos = "right";
        Debug.Log($"The red disk is located {cubePos} of the green cube.");

        Debug.Log("Moving the appreratus towards the red disk.");
        bool needStepOne = directionDetermination(xPos, zPos, cubePos);
        if (needStepOne)
        {
            yield return StartCoroutine(StepLogic(1, LRCamera, onePos, action1Prompt, 7, 128, 160));
        }
        // when Apperatus start at right, change the adjustment to 3
        yield return StartCoroutine(StepLogic(2, ABCamera, twoPos, action2Prompt, 3, 128));
        Debug.Log("Moving the red disk towards the green cube.");
        yield return StartCoroutine(StepLogic(3, LRCamera1, getDir(cubePos), DiskPosCheck, 0, 128, 188, 34));
        Debug.Log("Done.");
    }
    #endregion

    #region BarCubeCode
    public void RunBarCube()
    {
        StartCoroutine(ExecutionBarCubeWrapper());
    }

    /**
    <summary>
    Control Bar like Robotics to puse a cube
    Please use Apperatus 1, and Target 2
    Remember to assign Bar Controller to Apperatus
    </summary>
    */
    private IEnumerator ExecutionBarCubeWrapper()
    {
        // Apperatus start at 0.04, 0.64, 0.04 
        
        // CaptureAndSave(MainCamera, "Init");
        Debug.Log("Program begins.");
        Debug.Log("Determining directions.");
        string xPos, zPos, cubePos; 
        yield return StartCoroutine(DirVote(LRCamera, LRBarAsk, 128));
        xPos = llmOutput.ToLower(); // left
        yield return StartCoroutine(DirVote(LRCamera, FBBarAsk, 128));
        zPos = llmOutput.ToLower(); // below
        Debug.Log($"The appreratus is located {xPos} {zPos} the red cube.");
        yield return StartCoroutine(DirVote(LRCamera1, LRPrompt, 128, 188, 34));
        cubePos = llmOutput.ToLower(); // right

        
        Debug.Log($"The red cube is located {cubePos} of the green cube.");

        Debug.Log("Moving the appreratus towards the red cube.");
        bool needStepOne = directionDetermination(xPos, zPos, cubePos);
        if (needStepOne)
        {
            yield return StartCoroutine(StepLogic(1, LRCamera, onePos, action1Prompt, 5, 128));
        }
        // CaptureAndSave(MainCamera, "start2");
        yield return StartCoroutine(StepLogic(2, ABCamera, twoPos, action2Prompt, 5, 128));
        Debug.Log("Moving the red cube towards the green cube.");
        // CaptureAndSave(MainCamera, "start3");
        yield return StartCoroutine(StepLogic(3, LRCamera1, getDir(cubePos), PosCheck, 0, 128, 188, 34));
        Debug.Log("Done.");
        // CaptureAndSave(MainCamera, "done");
    }
    #endregion

    #region ExperimentCode
    public void RunExperiments()
    {
        // The following are experiments performed

        StartCoroutine(comunicationWrapper("BarDisk", SFCamera, DDPosCheck, 128, 188, 34));
        // StartCoroutine(CubeCubeTest2Wrapper(188, 42, 0.03f));
        // CameraWrapper();
        // CaptureAndSave(LRCamera, "shoot", 128, 128);
        // BarCubeTest4Wrapper();
        // StartCoroutine(BarCubeTest5Wrapper());
        // StartCoroutine(BarCubeTest6Wrapper("right", 128, 14));
        CaptureAndSave(SFCamera, "shoot",  128, 188, 34);



        // Scratch paper

        // StartCoroutine(TempWrapper());
    }

    private IEnumerator comunicationWrapper(String discriminator, Camera camView, string query, int heightReso = 0, int widthReso = 0, float fov = 0, float newXPosition = 0)
    {
        string Data = CaptureCamera(camView, heightReso, widthReso, fov,newXPosition);
        
        yield return StartCoroutine(CommunicationWithGemini(query, Data));
        Debug.Log(discriminator + $"Response is {llmOutput}");
    }

    /**
    <summary>
    Scratch Paper
    </summary>
    */
     private IEnumerator TempWrapper()
    {
        // yield return null;


        yield return StartCoroutine(SmartStepLogic(3, LRCamera1, SFCamera, Dir.Left, DDPosCheck, 2, 128, 188, 34));
    }
    #endregion

    #region TestWrapper
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
                    // Assuming LRCamera1 is a Camera object and LRPrompt is a string, replace these with actual references in your code
                    yield return StartCoroutine(comunicationWrapper($"{fov}, {width}", LRCamera1, LRPrompt, 128, width, fov));
                }
            }
        }
    }

    private IEnumerator CubeCubeTest2Wrapper(int width, int fov, float xVal = 0)
    {
        yield return StartCoroutine(StepLogic(3, LRCamera1, Dir.Left, PosCheck, 0, 128, width, fov, xVal));
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
                        // Assuming LRCamera1 is a Camera object and LRPrompt is a string, replace these with actual references in your code
                        yield return StartCoroutine(comunicationWrapper($"{fov}, {width}, {val}", LRCamera1, LRPrompt, 128, width, fov, val));
                    }
                }
                
            }
        }
    }

    private IEnumerator BarCubeTest5Wrapper()
    {
        int[] widths = new int[] { 128, 188};

        foreach (int width in widths)
        {
            Debug.Log($"width is {width}");
            for (int i = 0; i < 5; i++)
            {
                // Assuming LRCamera1 is a Camera object and LRPrompt is a string, replace these with actual references in your code
                yield return StartCoroutine(comunicationWrapper($"{width}", LRCamera, LRBarAsk, 128, width));
            }
        }
        bar.Move(Dir.Right, 2);
        foreach (int width in widths)
        {
            Debug.Log($"width is {width}");
            for (int i = 0; i < 5; i++)
            {
                // Assuming LRCamera1 is a Camera object and LRPrompt is a string, replace these with actual references in your code
                yield return StartCoroutine(comunicationWrapper($"{width}", LRCamera, LRBarAsk, 128, width));
            }
        }
        bar.Move(Dir.Right, 10);
        foreach (int width in widths)
        {
            Debug.Log($"width is {width}");
            for (int i = 0; i < 5; i++)
            {
                // Assuming LRCamera1 is a Camera object and LRPrompt is a string, replace these with actual references in your code
                yield return StartCoroutine(comunicationWrapper($"{width}", LRCamera, LRBarAsk, 128, width));
            }
        }
        bar.Move(Dir.Right, 2);
        foreach (int width in widths)
        {
            Debug.Log($"width is {width}");
            for (int i = 0; i < 5; i++)
            {
                // Assuming LRCamera1 is a Camera object and LRPrompt is a string, replace these with actual references in your code
                yield return StartCoroutine(comunicationWrapper($"{width}", LRCamera, LRBarAsk, 128, width));
            }
        }
    }

    private IEnumerator BarCubeTest6Wrapper(string xPos, int width = 128, int shift = 0)
    {
        bar.Move(Dir.Right, shift);
        string zPos = "below"; // below
        string cubePos = "right"; // right
        bool needStepOne = directionDetermination(xPos, zPos, cubePos);
        if (needStepOne)
        {
            yield return StartCoroutine(StepLogic(1, LRCamera, onePos, action1Prompt, 5, 128, width));
        }
        // CaptureAndSave(MainCamera, "start2");
        yield return StartCoroutine(StepLogic(2, ABCamera, twoPos, action2Prompt, 5, 128));
    }
    #endregion

    #region CameraTestWrapper
        public void CameraWrapper()
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
        
        public void BarCubeTest4Wrapper()
        {
            CaptureAndSave(LRCamera, "t1-1", 128);
            CaptureAndSave(LRCamera, "t1-2", 128, 188);
            bar.Move(Dir.Right, 2);
            CaptureAndSave(LRCamera, "t2-1", 128);
            CaptureAndSave(LRCamera, "t2-2", 128, 188);
            bar.Move(Dir.Right, 10);
            CaptureAndSave(LRCamera, "t3-1", 128);
            CaptureAndSave(LRCamera, "t3-2", 128, 188);
            bar.Move(Dir.Right, 2);
            CaptureAndSave(LRCamera, "t4-1", 128);
            CaptureAndSave(LRCamera, "t4-2", 128, 188);
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
            if (llmOutput.Contains("yes") && moveCount <= 2)
            {
                Debug.LogError($"Step {step} sanity check captured error output: {llmOutput}");
                llmOutput = "no";
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

    IEnumerator SmartStepLogic(int step, Camera CamView, Camera SFCamera, Dir currDir, string terminationQuery, int adjustment, int heightReso = 0, int widthReso = 0, float fov = 0, float newXPosition = 0)
    {
        // Bar moving
        string Data, Data2;
        int moveCount = 0;
        do
        {
            bar.Move(currDir);
            Data = CaptureCamera(CamView, heightReso, widthReso, fov, newXPosition);
            Data2 = CaptureCamera(SFCamera, heightReso, widthReso, fov, newXPosition);
            yield return StartCoroutine(CommunicationWithGemini("is the red object on the same height as the black object? Respond with 'same', 'above', or 'below'.", Data));
            Debug.Log($"Height response is {llmOutput}");
            
            if (llmOutput.Contains("above"))
            {
                Dir adjustDir = negDir(currDir);
                bar.Move(adjustDir, 3);
                yield return new WaitForSeconds(k_Wait);
                bar.Move(Dir.Forward, 4);
                yield return new WaitForSeconds(k_Wait);
            }
            else if (llmOutput.Contains("below"))
            {
                Dir adjustDir = negDir(currDir);
                bar.Move(adjustDir, 3);
                yield return new WaitForSeconds(k_Wait);
                bar.Move(Dir.Back, 4);
                yield return new WaitForSeconds(k_Wait);
            }
            yield return StartCoroutine(CommunicationWithGemini(terminationQuery, Data2));
            llmOutput = llmOutput.ToLower();
            if (llmOutput.Contains("yes") && moveCount <= 2)
            {
                Debug.LogError($"Step {step} sanity check captured error output: {llmOutput}");
                llmOutput = "no";
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
                action1Prompt = action1Prompt + zPos + PromptEnding;
                twoPos = getDir(zPos);
                action2Prompt = action2Prompt + negate(xPos) + " of" + PromptEnding;
                return true;
            }
            else
            {
                twoPos = getDir(zPos);
                action2Prompt = action2Prompt + xPos + " of" + PromptEnding;
                return false;
            }
        }
        else if (cubePos.Contains("right"))
        {
            if (xPos.Contains("left"))
            {
                onePos = getDir(xPos);
                action1Prompt = action1Prompt + zPos + PromptEnding;
                twoPos = getDir(zPos);
                action2Prompt = action2Prompt + negate(xPos) + " of" + PromptEnding;
                return true;
            }
            else
            {
                twoPos = getDir(zPos);
                action2Prompt = action2Prompt + xPos + " of" + PromptEnding;
                return false;
            }
        }
        else if (cubePos.Contains("above"))
        {
            if (zPos.Contains("below"))
            {
                onePos = getDir(zPos);
                action1Prompt = action1Prompt + xPos + " of" + PromptEnding;
                twoPos = getDir(xPos);
                action2Prompt = action2Prompt + negate(zPos) + PromptEnding;
                return true;
            }
            else
            {
                twoPos = getDir(xPos);
                action2Prompt = action2Prompt + zPos + PromptEnding;
                return false;
            }
        }
        else 
        {
            if (zPos.Contains("above"))
            {
                onePos = getDir(zPos);
                action1Prompt = action1Prompt + xPos + " of" + PromptEnding;
                twoPos = getDir(xPos);
                action2Prompt = action2Prompt + negate(zPos) + PromptEnding;
                return true;
            }
            else
            {
                twoPos = getDir(xPos);
                action2Prompt = action2Prompt + zPos + PromptEnding;
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
    #endregion
}