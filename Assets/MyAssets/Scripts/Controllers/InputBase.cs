using UnityEngine;


/// <summary>
/// A generic class that in the future can be used to inherit any type of control
/// Can be changed to non-abstract, if we need to have central universal input controller.
/// </summary>
public abstract class InputBase : MonoBehaviour
{
    // Player movement event in 2D coordinates
    public delegate void MoveEvent(Vector2 direction);
    public event MoveEvent MoveEv;
    public void Move(Vector2 direction)
    {
        MoveEv?.Invoke(direction);
    }
}
