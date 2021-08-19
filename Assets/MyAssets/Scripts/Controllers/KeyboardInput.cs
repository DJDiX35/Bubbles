//
// Универсальный контроллер управления с клавиатуры
//

using UnityEngine;

public class KeyboardInput : InputBase
{
    private Settings.Game _game;

    private float _acceleration = 0f;
    private float _maxSpeed = 0f;
    private Vector2 direction = Vector2.zero;


    public void Init(Settings settings)
    {
        _game = settings.game;

        _acceleration = settings.Input.Acceleration;
        _maxSpeed = settings.Input.MaxSpeed;

        Subscribe(settings);
    }
    private void Subscribe(Settings settings)
    {
        settings.Player.HitBorderEv -= InstantStopMoving;
        settings.Player.HitBorderEv -= InstantStopMoving;
        settings.Player.HitBorderEv += InstantStopMoving;
    }

    // Update is called once per frame
    void Update()
    {
        if (_game.State != GameState.GamePlay) return;


        if (Input.GetKey(KeyCode.D))
        {
            if (direction.x < _maxSpeed) direction.x += _acceleration * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (direction.x > -_maxSpeed) direction.x -= _acceleration * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (direction.y < _maxSpeed) direction.y += _acceleration * 1.5f * Time.deltaTime; // ìû - ïóçûðü. Ïîäúåì âñåãäà ëåã÷å ÷åì ñïóñê.
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (direction.y > -_maxSpeed) direction.y -= _acceleration * Time.deltaTime;
        }


        if (direction != Vector2.zero)
        {
            Move(direction);
            SlowDownAtKeyRelease();
        }
    }

    /// <summary>
    /// Если все кнопки отпущены - постепенно замедляем движение
    /// </summary>
    private void SlowDownAtKeyRelease()
    {
        // замедление по оси X
        if (direction.x != 0 && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            direction.x = Mathf.MoveTowards(direction.x, 0, _acceleration * 2f * Time.deltaTime);
        }
        // замедление по оси Y
        if (direction.y != 0 && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            // так как у нас в управлении пузырь - замедление вниз более активное, чем вверх
            if (direction.y > 0)
                direction.y = Mathf.MoveTowards(direction.y, 0, _acceleration * 0.5f * Time.deltaTime);
            else
                direction.y = Mathf.MoveTowards(direction.y, 0, _acceleration * 1.5f * Time.deltaTime);
        }
    }

    private void InstantStopMoving(Settings.PlayerSettings.BorderType borderType)
    {
        if (borderType == Settings.PlayerSettings.BorderType.Horizontal) direction.x = 0;
        if (borderType == Settings.PlayerSettings.BorderType.Vertical) direction.y = 0;
    }
}
