// BarController.cs
using UnityEngine;

// If the BarMover is within a namespace, make sure to include that here
// using YourNamespace;

public class BarController : MonoBehaviour
{
    const float teleSize = 0.01f;

    // Movement function that takes a direction and an amount
    public void Move(BarMover.Dir direction, int amount = 1)
    {
        Vector3 moveVector = Vector3.zero;
        float shift = amount * teleSize;

        // Determine the direction
        switch (direction)
        {
            case BarMover.Dir.Left:
                moveVector = new Vector3(-shift, 0f, 0f);
                break;
            case BarMover.Dir.Right:
                moveVector = new Vector3(shift, 0f, 0f);
                break;
            case BarMover.Dir.Forward:
                moveVector = new Vector3(0f, 0f, shift);
                break;
            case BarMover.Dir.Back:
                moveVector = new Vector3(0f, 0f, -shift);
                break;
            case BarMover.Dir.Up:
                moveVector = new Vector3(0f, shift, 0f);
                break;
            case BarMover.Dir.Down:
                moveVector = new Vector3(0f, -shift, 0f);
                break;
        }

        // Apply the movement to the bar's position
        transform.position += moveVector;
    }
}
