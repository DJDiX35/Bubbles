//
// ������������� ����� ������� � ������� ����� ���� ����������� ��� ������������ ������ ���� ����������
//

using UnityEngine;

public abstract class InputBase : MonoBehaviour
{
    // ����� ����������� ������ � 2D �����������
    public delegate void MoveEvent(Vector2 direction);
    public event MoveEvent MoveEv;
    public void Move(Vector2 direction)
    {
        MoveEv?.Invoke(direction);
    }
}
