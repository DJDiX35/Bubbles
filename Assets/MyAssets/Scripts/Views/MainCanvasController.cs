//
// ���������� �� ���� ������������� �� ������ UI � �� �������������� � �����.
// � ��������� ����� ��������, ��� ��� �� ���� ��� � ��� �� ���������� ������,
// �, ����, ����������, ������� ������ ������ �� ����� ����������� (�����-������).
// �� ���� ���� ��������������� - ��������� ������� ������ � ��������� UI ���� �� ������ �� �������.
// �� ���� ������� �� ����������� ���������� ������������. ������� ���������� ������������.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasController : MonoBehaviour
{
    private LevelController _levelController;

    [Header("Panels")]
    [SerializeField]
    private GameObject _mainMenuPanel;



    public void Init(LevelController levelController)
    {
        _levelController = levelController;

        ShowMainMenu();
    }

    /// <summary>
    /// ��������� ������ ������ � ������ ����������� �� ���������� ������.
    /// ���� ������� ������ �� ������. ����� ������� ������������ ������.
    /// </summary>
    public void LevelStart()
    {
        if (_mainMenuPanel.activeSelf) _mainMenuPanel.SetActive(false);

        if (_levelController != null) _levelController.LevelStart();    // �������� �� ��������� :D
    }

    private void ShowMainMenu()
    {
        if (!_mainMenuPanel.activeSelf) _mainMenuPanel.SetActive(true);
    }
}
