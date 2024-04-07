public static class Queries
{
    // Direction Prompt
    public static readonly string LRBarAsk = @"Given the image with a black bar and a red cube, is the black bar on the left or right of the red cube? Please respond with only one word: 'left', or 'right'";
    public static readonly string FBBarAsk = @"Given the image with a black bar and a red cube, is the black bar on the above or below the red cube? Please respond with only one word: 'above', or 'below'";
    public static readonly string LRPrompt = @"Given the image with a red object and a green cube, is the red object on the left or right of the green cube? Please respond with only one word: 'left', or 'right'";
    
    // Action Prompt
    public static readonly string actionHeader = @"Given the image with a black bar and a red cube, is the black bar positioned directly ";
    public static readonly string actionTail = @" the red cube? Only reply with 'yes' or 'no'";
    public static readonly string PosCheck = @"Given the image with a red cube and a green cube, has the red cube reach the green cube? Respond 'yes' if the red cube touches the green cube, 'no' otherwise. Respond with only one word: 'yes' or 'no'";
    

    // When red object is a disk
    public static readonly string LRDiskAsk = @"Given the image with a black bar and a red disk, is the black bar on the left or right of the red disk? Please respond with only one word: 'left', or 'right'";
    public static readonly string FBDiskAsk = @"Given the image with a black bar and a red disk, is the black bar on the above or below the red disk? Please respond with only one word: 'above', or 'below'";
    public static readonly string DiskLRPrompt = @"Given the image with a red disk and a green cube, is the red disk on the left or right of the green cube? Please respond with only one word: 'left', or 'right'";
    
    public static readonly string DiskactionHeader = @"Given the image with a black bar and a red disk, is the black bar positioned directly ";
    public static readonly string DiskactionTail = @" the red disk? Only reply with 'yes' or 'no'";
    public static readonly string DiskPosCheck = @"Given the image with a red disk and a green cube, has the red disk reach the green cube? Respond 'yes' if the red disk is around the green cube, 'no' otherwise. Respond with only one word: 'yes' or 'no'";
  
    // When both the red object and the Apperatus base are disks
    public static readonly string LRDDAsk = @"Given the image with a black onject and a red onject, is the black object on the left or right of the red object? Please respond with only one word: 'left', or 'right'";
    public static readonly string FBDDAsk = @"Given the image with a black disk and a red disk, is the black disk on the above or below the red disk? Please respond with only one word: 'above', or 'below'";
    
    public static readonly string DDactionHeader = @"Given the image with a black disk and a red disk, is the black disk positioned directly ";
    public static readonly string DDactionUp = @"Given the image with a black object and a red object, is the black object almost horizontally aligned with the red object?";
    public static readonly string DDPosCheck = @"Given the image with a two disks and a green cube, does one disk reach the green cube? Respond 'yes' if one disk touch the green cube, 'no' otherwise. Respond with only one word: 'yes' or 'no'";
  
    
}