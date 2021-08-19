//
// Application-��� ������. ����������� ������ - ������ A-MVC � ��������� �� ��������� �����.
// GameManager - ������ ����� ����� � ����������, ������� �������������� ��� ��� ��������� �������,
// ������ �������� �� ������ �� ������, ������� ����� ����� ��� ������.
// ����� �� ����� �� � ��� ����� ����, ��� �� ����������� GameManager ��� �������������.
// ��� ����� �������� ���������� ����������, Service Locator, DI � ������ ��������� ����������� �����. 
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [Header("Models / Pools / Settings")]
    [SerializeField]
    private Settings _gameSettings;

    [SerializeField]
    private BubblesPool _bubblesPool;


    [Header("Views")]
    [SerializeField]
    private MainCanvasController _mainCanvasController;


    [Header("Controllers")]
    [SerializeField]
    private Timer _timer;

    [SerializeField]
    private KeyboardInput _keyboardInput;

    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private LevelController _levelController;

    [SerializeField]
    private BubblesController _bubblesController;


    void Start()
    {
        // init views/views controllers
        _mainCanvasController.Init(_levelController);

        // init controllers
        _timer.Init(_gameSettings.level);
        _keyboardInput.Init(_gameSettings);
        _playerController.Init(_gameSettings, _keyboardInput);
        _levelController.Init(_gameSettings);
        _bubblesController.Init(_gameSettings, _bubblesPool, _timer);

        // Call Event that game/application started
        _gameSettings.game.Start();
    }

    private void OnApplicationQuit()
    {
        _gameSettings.game.Close();
    }
}
