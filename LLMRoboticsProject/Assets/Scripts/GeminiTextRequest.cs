using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Gemini
{

    public class GeminiTextRequest: AIHttpClient
    {
        public async Task<GeminiTextResponse> SendMsg(params string[] strings)
        {
            FullRequest fullRequest = new FullRequest();
            List<Part> partsList = new List<Part>(); // Create a list to hold Part objects
            Content content = new Content { role = "user"};

            for (int i = 0; i < strings.Length; i++)
            {
                Part part = new Part();
                if (i == 0){
                    part.text = strings[0];

                    // Add the created Part object to the parts list
                    partsList.Add(part);

                }
                else 
                {
                   part.inlineData = new InlineData
                        {
                            mimeType = "image/png",
                            data = strings[i]
                        };
                    // Add the created Part object to the parts list
                    partsList.Add(part);
                }
            }
            Part[] partsArray = partsList.ToArray();
            content.parts = partsArray;
            fullRequest.contents = content;

            // GenerationConfig gc = new GenerationConfig();
            // gc.temperature = 0.1f;
            // gc.topP = 0.9f;
            // gc.topK = 20;
            // gc.candidateCount = 1;
            // gc.maxOutputTokens = 5;
            // fullRequest.generationConfig = gc;

            GeminiTextResponse geminiTextResponse = await base.PostAsync<FullRequest, GeminiTextResponse>(fullRequest, $"https://us-central1-aiplatform.googleapis.com/v1/projects/{PROJECT_ID}/locations/us-central1/publishers/google/models/{MODEL_ID}:{QUERY}");

            //if(geminiTextResponse!=null && geminiTextResponse.candidates.Length>0)
            //    contents.Add(geminiTextResponse.candidates[0].content);

            return geminiTextResponse;
        }
    }

    public class FullRequest
    {
        public Content contents;
        // public GenerationConfig generationConfig;
    }

    public partial class Content
    {
       public Part[] parts { get; set; }
    }

    public class Part
    {
       public string text { get; set; }
       public InlineData inlineData { get; set; }
    }

    public class InlineData
    {
        public string mimeType { get; set; }
        public string data { get; set; } // Base64-encoded data
    }

    public class GenerationConfig
    {
        public float temperature { get; set; }
        public float topP { get; set; }
        public float topK { get; set; }
        public int candidateCount { get; set; }
        public int maxOutputTokens { get; set; }
    }
}