# LLM-Robotics
Using LLM to guide the movement of a surgical arm (stick)

## PickAndPlaceProject
This is the file containing the Unity project and code script
- Asset is the project asset, Script folder inside it containing codes

## Code
- GeminiTestRequest.cs, GeminiTextResponse.cs AIHttpClients.cs are Gemini Driving code
- BarMover.cs the main project code
- BarController.cs, Utilities code for moving objects, and LLM prompt

# Setup Requirements
- Create a Google Cloud account
- Go to Asset/Scrpits/AIHttpClients.cs
- Change the PROJECT_ID to yoour project id
- Modify MODEL_ID and QUERY base on your needs
