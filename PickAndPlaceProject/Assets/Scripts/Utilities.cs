public class Utilities
{
    // Direction Prompt
    public static readonly string LRBarAsk = @"Given the image with a black bar and a red cube, is the black bar on the left or right of the red cube? Please respond with only one word: 'left', or 'right'";
    public static readonly string FBBarAsk = @"Given the image with a black bar and a red cube, is the black bar on the above or below the red cube? Please respond with only one word: 'above', or 'below'";
    public static readonly string LRPrompt = @"Given the image with a red object and a green cube, is the red object on the left or right of the green cube? Please respond with only one word: 'left', or 'right'";
    
    // Action Prompt
    public static readonly string actionHeader = @"Given the image with a black bar and a red cube, is the black bar positioned directly ";
    public static readonly string actionTail = @" the red cube? Only reply with 'yes' or 'no'";
    public static readonly string PosCheck = @"Given the image with a red cube and a green cube, has the red cube reach the green cube? Respond 'yes' if the red cube touches the green cube, 'no' otherwise. Respond with only one word: 'yes' or 'no'";
    
}