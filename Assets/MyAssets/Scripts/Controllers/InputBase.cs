//
// Универсальный класс который в будущем может быть использован для наследования любого типа управления
//

using UnityEngine;

public abstract class InputBase : MonoBehaviour
{
    // Эвент перемещения игрока в 2D координатах
    public delegate void MoveEvent(Vector2 direction);
    public event MoveEvent MoveEv;
    public void Move(Vector2 direction)
    {
        MoveEv?.Invoke(direction);
    }
}
