using UnityEngine;

/// <summary>
/// Game settings. Model. Some of the settings are edited directly from the editor.
/// Logically, everything is not changeable, except for the state of the game. Perhaps it is worth taking it out of the Game somehow.
/// Also, according to the MVC logic, events should be in the data model, but it could be moved into a separate model purely for events.
/// </summary>
public enum GameState { MainMenu, GameStart, GamePlay, GameLose }
public class Settings : MonoBehaviour
{
    #region Game Settings

    [System.Serializable]
    public class Game
    {
        private GameState _state = GameState.MainMenu;
        public GameState State {
            get
            {
                return _state;
            } 
            set
            {
                if (_state != value)     // A state change event should only be sent if the state has REALLY changed.
                {
                    _state = value;
                    StateChanged();
                }
                else
                {
                    _state = value;
                }
            }
        }

        #region Events block

        public delegate void StateChangedEvent();
        public event StateChangedEvent StateChangedEv;
        public void StateChanged() { StateChangedEv?.Invoke(); }

        public delegate void StartEvent();
        public event StartEvent StartEv;
        public void Start() { StartEv?.Invoke(); }

        public delegate void CloseEvent();
        public event CloseEvent CloseEv;
        public void Close() { CloseEv?.Invoke(); }
        #endregion
    }
    public readonly Game game = new Game();
    #endregion


    #region Level Events

    [System.Serializable]
    public class Level
    {
        public delegate void StartEvent();
        public event StartEvent StartEv;
        public void Start() { StartEv?.Invoke(); }

        public delegate void EndEvent();
        public event EndEvent EndEv;
        public void End() { EndEv?.Invoke(); }

    }
    public readonly Level level = new Level();
    #endregion


    #region Input Settings

    [System.Serializable]
    public class InputSettings
    {
        [SerializeField]
        private float _acceleration = 0.1f;
        public float Acceleration { get { return _acceleration; } }

        [SerializeField]
        private float _maxSpeed = 1f;
        public float MaxSpeed { get { return _maxSpeed; } }
    }
    [SerializeField]
    private InputSettings _input = new InputSettings();
    public InputSettings Input { get { return _input; } }
    #endregion


    #region Player Settings

    [System.Serializable]
    public class PlayerSettings
    {

        [SerializeField]
        private float _startSize = 1f;
        public float StartSize { get { return _startSize; } }

        #region Player Events
        public enum BorderType { Horizontal, Vertical } // Need move the enumerator to a different location. The request string is too long.

        public delegate void HitBorderEvent(BorderType borderType);
        public event HitBorderEvent HitBorderEv;
        public void HitBorder(BorderType borderType) { HitBorderEv?.Invoke(borderType); }
        #endregion
    }
    [SerializeField]
    private PlayerSettings _player = new PlayerSettings();
    public PlayerSettings Player { get { return _player; } }
    #endregion


    #region Bubbles Settings

    [System.Serializable]
    public class BubblesSettings
    {
        [SerializeField]
        private int _startCount = 10;
        public int StartCount { get { return _startCount; } }

        [SerializeField]
        private float _timeCountMultiplyer = 0.1f;
        public float TimeCountMultiplyer { get { return _timeCountMultiplyer; } }

        [SerializeField]
        private float _spawnDelay = 0.5f;
        public float SpawnDelay { get { return _spawnDelay; } }
    }
    // Possible to change this to "method + number caching" in the controllers used.
    public BubblesSettings bubbles = new BubblesSettings();
    #endregion
}
