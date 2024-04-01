# LLM-Robotics
Using LLM (Google Gemini 1.0 pro vision) to guide the movement of da Vinci Research Kit (dVRK) endowrist (represented as a bar/stick)
The significance of this project lies in the possible later extension of this projoect by using VR/AR to virtualize the da Vinci Research Kit (dVRK) endowrist as our bar, so that although dVRK endowrist is in execution in reality, we can still simply execute this program to manipulate it.
## LLMRoboticsProject
This is the file containing the Unity project and code script
- Asset is the project asset, Script folder inside it containing codes

## Code
- GeminiTestRequest.cs, GeminiTextResponse.cs AIHttpClients.cs are Gemini Driving code
- BarMover.cs the main project code
- BarController.cs, Utilities.cs code for moving objects, and LLM prompt

# Setup Requirements
- Create a Google Cloud account
- Go to Asset/Scrpits/AIHttpClients.cs/MyRegion
- Change the PROJECT_ID to yoour project id
- Modify MODEL_ID and QUERY base on your needs

# Acknowledgements And Modifications
- The setup of this project is based on Unity Robotics Hub -> Pick and Place Tutorial. Link is here:
    https://github.com/Unity-Technologies/Unity-Robotics-Hub/tree/main/tutorials/pick_and_place
- The Network connection with Gemini is based on the following repo with significant changes:
    https://github.com/jackcodewu/GeminiAI.Net

- Changes:
    1. The repo will connect to Google Gemini, but Gemini is not available in many countries, and people has to connect with Google Vertex AI. We rewrite the network connection port to auto-generate crendentials and connect to Google Vertex AI
    2. The repo only allows passing in one part, text-based prompt. We have rewrite the input logic so it can take as many parts as wanted and also able to take in images as input
    3. Please look at Asset/Script/GeminiTestRequest.cs for details of how to pass in other forms of data (eg. file on cloud, videos, functions, etc.)
