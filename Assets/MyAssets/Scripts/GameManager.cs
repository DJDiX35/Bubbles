using UnityEngine;

/// <summary>
/// GameManager is Application-Type class. The only entry point to the application.
/// GameManager initializes all other scripts, and also gives them only those links that are required for these scripts to work.
/// Nobody knows about anything other than what the GameManager gave them upon initialization.
/// Thus, we avoid global variables, Service Locator, DI and other possible noodle-like things.
/// </summary>
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

        // init models/pools
        _bubblesPool.Init(_playerController);

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
